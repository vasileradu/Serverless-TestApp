param (
    [Parameter(Mandatory=$true)][string]$path,
	[Parameter(Mandatory=$true)][string]$output
)

### helper functions
function Add-JmeterContent ($metricName, $inputFile, $output, $tags, $newStartDateMs) {


	# Copies column-header-name onto each row, before the corresponding:
	# note: assumes timestamp is last column
	#metricName,col1-name=row1-val-name,col2-name=row1-val-name timestamp
	#metricName,col1-name=row2-val-name,col2-name=row2-val-name timestamp

	$delimiter = ",";
	$timestampIndex = 0;
	$httpCodeIndex = 3;
	$scenarioIndex = 4;
	$success = 5;	
	$headers = (Get-Content -Path $inputFile -TotalCount 1).Split($delimiter);
	$headers = Rename-JmeterHeader($headers)
	$filterTagsIndexes = @(2);
	$additionalMs = [long]0;
	$usersPerScenario = @(0,0,0,0);	
	$lines = 0;

	Get-Content $inputFile | Select-Object -Skip 1 | Sort-Object -Property @{Expression = {$_.Split($delimiter)[$timestampIndex]}} | ForEach-Object {
		
		if($_ -match 'Cleanup Storage Windows') {
			return
		}
				
		$values = $_.Split($delimiter)		
		$lastValue = ""
		
		$starTags = ""; # filtreable
		$endTags = ""; # non-filtreable
		
				
		for ($index = 0; $index -lt $values.count; $index++) {
			
			if($index -eq $timestampIndex) {
				
				$ts = [long]$values[$index];
				
				## translate timestamp
				if($additionalMs -eq 0) {
					$additionalMs = [long]$newStartDateMs - [long]$ts;
				}
				
				$ts = [long]$ts + [long]$additionalMs;
				
				## convert timestamp from ms to ns
				$lastValue = [string]$ts + "000000";							
				
				continue ## go to next line;
			}
			
			$key=$headers[$index];
			$value=$values[$index];
			
			# special parsing
			switch ($index) {				
				$httpCodeIndex {
					if($value.length -gt 3) {
						$value = 444 # no code was sent out;
					}
				}
				$scenarioIndex {
					## extract scenario name and threads;
					$scenarioValues = $value.Split(" ");
					$value = $scenarioValues[0] # scenario name will be added later;
					
					$scenarioNumber = [int]$value.Replace("Scenario", "") - [int]1;
															
					# number of threards (users) added now;
					$threads = [int]$scenarioValues[1].Split("-")[1];
					$allThreads = 0;
					
					if($threads -gt $usersPerScenario[$scenarioNumber]){
						$usersPerScenario[$scenarioNumber] = $threads;
					}
										
					for ($i = 0; $i -lt $usersPerScenario.count; $i++) {
						$allThreads += $usersPerScenario[$i]
					}				
					
					$endTags += "startedThreads=" + $allThreads + $delimiter
				}
				$success {
					# key should be 'errorCount'
					if($value -eq "true") {
						$value = 0
					} else {
						$value = 1
					}					
				}
			}			
			
			if(-not($value -match '^\d+$')) {
				# not a number
				$value = """" + $value + """"
				$value = $value.Replace(" ", "_")
			}			
			
			if($filterTagsIndexes.Contains($index)) {
				$starTags += $delimiter + $key + "=" + $value
			} else {
				$endTags += $key + "=" + $value
				# do not add delimiter after last value.
				if($index -lt $values.count - 1) {
					$endTags += $delimiter
				}
			}			
		}
		
		$newRow = "$metricName," + $tags + $starTags + " " + $endTags + " " + $lastValue;
		$lines++;
		
		return "$newRow`n"
	} | Add-Content -Path $output -NoNewline -Encoding "ascii"
	
	return $lines;
}

function Rename-JmeterHeader($header) {
	
	$renames = @{
		success='errorCount';
		elapsed='responseTime';
		label='requestName'}
		
	foreach ($item in $renames.GetEnumerator()) {
		$header = $header.Replace($($item.Name), $($item.Value))
	}
	
	return $header;	
}

function Add-TelegrafContent($inputFile, $output, $tags, $newStartDateMs) {
# Adds new tags right after first comma-separated-value
	$delimiter = ",";
	$lines = 0;
	$additionalMs = [long]0;
	
	Get-Content $inputFile | ForEach-Object {
    
		$values = $_.Split($delimiter);
		$metricName=$values[0];
		$newRow = $metricName + $delimiter + $tags;
		
		## translate timestamp
		## timestamp is the very last value, separated by empty space;
		$lastValues = $values[-1].Split(" "); 
		$ts = [math]::round([long]$lastValues[-1]/1000000) #convert to milliseconds;
		
		if($additionalMs -eq 0) {
			$additionalMs = [long]$newStartDateMs - [long]$ts;
		}
		
		$ts = [long]$ts + [long]$additionalMs;
		
		## convert timestamp from ms to ns
		$lastValues[-1] = [string]$ts + "000000";		
		$values[-1] = $lastValues -join ' '
		
		for ($index = 1; $index -lt $values.count; $index++) {
		
			$newRow += $delimiter + $values[$index];
		}
		
		$lines++;
		
		return "$newRow`n"
		
	} | Add-Content -Path $output -NoNewline -Encoding "ascii"
	
	return $lines;
}
#jmeter filename: jmeter.{tags*}.{environment}.{application}.results

$phase = $path.Split("\")[-1];

# used for translating dates / timestamps;
$epoch = Get-Date -Year 1970 -Month 1 -Day 1 -Hour 0 -Minute 0 -Second 0
$newStartDate = Get-Date -Date "2020-01-01 00:00:01Z"
$newStartDateMs = [math]::truncate($newStartDate.Subtract($epoch).TotalMilliSeconds)

$lines = 0;

if (!(Test-Path $output)) {
	New-Item -ItemType File -Force -Path $output
}

Get-ChildItem $path | Where-Object {($_.Name.StartsWith("jmeter") -and ($_.Extension -eq ".results"))} | ForEach-Object {
	# detect tags
	$environment=$_.Name.Split(".")[-3]
	$application=$_.Name.Split(".")[-2]
	$tags="phase=$phase,environment=$environment,application=$application";
	
	# add jmeter content
	$lines += Add-JmeterContent -metricName "requestsRaw" -inputFile $_.FullName -output $output -tags $tags -newStartDateMs $newStartDateMs
	
}

$tags="phase=$phase";

Get-ChildItem $path | Where-Object {($_.Name.StartsWith("telegraf") -and ($_.Extension -eq ".out"))} | ForEach-Object {
	# add telegraf content
	$lines += Add-TelegrafContent -inputFile $_.FullName -output $output -tags $tags -newStartDateMs $newStartDateMs

}

Write-Host "Wrote $lines lines to $output" -ForegroundColor Yellow