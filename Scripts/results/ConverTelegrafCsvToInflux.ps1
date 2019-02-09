param (
    [Parameter(Mandatory=$true)][string]$dbName,
    [Parameter(Mandatory=$true)][string]$inputFile,
	[Parameter(Mandatory=$true)][string]$output,
	[Parameter(Mandatory=$true)][string]$tags
	
)

# Adds new tags right after first comma-separated-value

$delimiter = ",";
$lines = 0;

New-Item -ItemType File -Force -Path $output
Add-Content $output "# DML"
Add-Content $output "# CONTEXT-DATABASE: $dbName"
Add-Content $output ""

Get-Content $inputFile | ForEach-Object {
    
	$values = $_.Split($delimiter);
		$metricName=$values[0];
		$newRow = $metricName + $delimiter + $tags;
			
		for ($index = 1; $index -lt $values.count; $index++) {
		
			$newRow += $delimiter + $values[$index];
		}
		
		$lines++;
		
		return "$newRow`n"
	
} | Add-Content -Path $output -NoNewline -Encoding "ascii"

Write-Host "$lines lines written to $output" -ForegroundColor Yellow