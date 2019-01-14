param (
    [Parameter(Mandatory=$true)][string]$app    
 )


$root="\\testappcloudstorage.file.core.windows.net\config";

$devConfigPath = "$root\builds\$app\appsettings.Development.json";
$prodOldConfigPath = "$root\builds\$app\appsettings.json";
$prodNewConfigPath = "$root\web\$app.appsettings.azure.json";


if (Test-Path $devConfigPath) {
	Remove-Item $devConfigPath
	Write-Host "Removed $devConfigPath"
}

if (-not(Test-Path $prodOldConfigPath)) {
	Write-Host "FAILED '"$prodOldConfigPath"' missing !" -ForegroundColor Red
	break;
}

if (-not(Test-Path $prodNewConfigPath)) {
	Write-Host "FAILED '"$prodNewConfigPath"' missing !" -ForegroundColor Red
	break;
}

Copy-Item -Path $prodNewConfigPath -Destination $prodOldConfigPath -Force

Write-Host "Updated config to production settings" -ForegroundColor Green


