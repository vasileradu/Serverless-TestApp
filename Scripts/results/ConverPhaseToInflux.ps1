param (
    [Parameter(Mandatory=$true)][int]$phase
)

$inputPath = "C:\Projects\Serverless-TestApp\Evaluation\Results\phase$phase";
$outputPath = "influx$phase.out"

Write-Host "input: $inputPath"
Write-Host "output: $outputPath"

& .\ConvertAllCsvToInflux.ps1 -path $inputPath -output $outputPath