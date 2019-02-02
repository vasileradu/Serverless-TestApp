param (
    [Parameter(Mandatory=$true)][string]$dbName,
    [Parameter(Mandatory=$true)][string]$metricName,
    [Parameter(Mandatory=$true)][string]$inputFile,
	[Parameter(Mandatory=$true)][string]$output,
	[Parameter(Mandatory=$true)][string]$tags
	
)

# Copies column-header-name onto each row, before the corresponding:
# note: assumes timestamp is last column
#metricName,col1-name=row1-val-name,col2-name=row1-val-name timestamp
#metricName,col1-name=row2-val-name,col2-name=row2-val-name timestamp

$delimiter = ",";
$timestampIndex = 0;
$scenarioIndex = 4;
$headers = (Get-Content -Path $inputFile -TotalCount 1).Split($delimiter);

$lines = 0;

New-Item -ItemType File -Force -Path $output
Add-Content $output "# DML"
Add-Content $output "# CONTEXT-DATABASE: $dbName"
Add-Content $output ""

Get-Content $inputFile | Select-Object -Skip 1 | ForEach-Object {
    
	$values = $_.Split($delimiter)
	$newRow = "$metricName," + $tags;
	$lastValue = ""
		
	for ($index = 0; $index -lt $values.count; $index++) {
		
		if($index -eq $timestampIndex) {
			$lastValue = $values[$index] + "000000000";
			continue # go to next line;
		}
		
		$key=$headers[$index];
		$value=$values[$index];
		
		if($index -eq $scenarioIndex) {
			## cleanup, remove everything after space;
			$value = $value.Split(" ")[0] 
		}
	
		$newRow += $delimiter + $key + "=" + $value;
	}
	
	$newRow += " " + $lastValue;
	$lines++;
	
	return $newRow
	
} | Add-Content($output)

Write-Host "$lines lines written to $output" -ForegroundColor Yellow