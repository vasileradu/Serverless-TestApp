param (
    [Parameter(Mandatory=$true)][string]$dbName,
    [Parameter(Mandatory=$true)][string]$metricName,
    [Parameter(Mandatory=$true)][string]$inputFile,
	[Parameter(Mandatory=$true)][string]$output,
	[Parameter(Mandatory=$true)][string]$tags
	
)

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

$lines = 0;

New-Item -ItemType File -Force -Path $output
Add-Content $output "# DML"
Add-Content $output "# CONTEXT-DATABASE: $dbName"
Add-Content $output ""

Get-Content $inputFile | Select-Object -Skip 1 | ForEach-Object {
    
		if($_ -match 'Cleanup Storage Windows') {
			return
		}
		
		$values = $_.Split($delimiter)		
		$lastValue = ""
		
		$starTags = ""; # filtreable
		$endTags = ""; # non-filtreable
		$usersPerScenario = @{};
				
		for ($index = 0; $index -lt $values.count; $index++) {
			
			if($index -eq $timestampIndex) {
				## convert timestamp from ms to ns
				$lastValue = $values[$index] + "000000";
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
					
					# number of threards (users) added now;
					$threads = $scenarioValues[1].Split("-")[1];
					$allThreads = 0;
					
					$usersPerScenario.Set_Item($value, [int]$threads)
										
					foreach ($users in $usersPerScenario.GetEnumerator()) {
						$allThreads += $($users.Value)
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

Write-Host "$lines lines written to $output" -ForegroundColor Yellow