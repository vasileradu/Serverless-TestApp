param (
    [Parameter(Mandatory=$true)][string]$app,
	[Parameter(Mandatory=$true)][string]$connectionString
)

# Storage vars
$ctx = New-AzureStorageContext -ConnectionString $connectionString
$shareName = "config"
$buildsCloudPath=".\builds\"
$toolsCloudPath=".\tools\telegraf"

# VM vars
$buildsLocalPath="C:\app"
$toolsLocalPath="C:\telegraf"


# Copy build step
Write-Host "-- Copy-Build Step --" -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path $buildsLocalPath
Get-AzureStorageFileContent -ShareName $shareName -Path "$buildsCloudPath\$app.zip" -Context $ctx -Destination $buildsLocalPath
Get-AzureStorageFileContent -ShareName $shareName -Path "$buildsCloudPath\StartApp.bat" -Context $ctx -Destination $buildsLocalPath
Write-Host "------------------" -ForegroundColor Yellow
###

# Unzip step
Write-Host "-- Unzip-Build Step --" -ForegroundColor Yellow
Add-Type -assembly "system.io.compression.filesystem"	
[System.IO.Compression.ZipFile]::ExtractToDirectory("$buildsLocalPath\$app.zip", $buildsLocalPath)
Write-Host "------------------" -ForegroundColor Yellow
###


# Copy telegraf step
Write-Host "-- Copy Monitoring Tools Step --" -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path $toolsLocalPath
Get-AzureStorageFileContent -ShareName $shareName -Path "$toolsCloudPath\telegraf.exe" -Context $ctx -Destination $toolsLocalPath
Get-AzureStorageFileContent -ShareName $shareName -Path "$toolsCloudPath\StartMonitoring.bat" -Context $ctx -Destination $toolsLocalPath
Get-AzureStorageFileContent -ShareName $shareName -Path "$toolsCloudPath\telegraf.azure.vm.$app.conf" -Context $ctx -Destination $toolsLocalPath
Rename-Item -Path "$toolsLocalPath\telegraf.azure.vm.$app.conf" -NewName "$toolsLocalPath\telegraf.conf"
Write-Host "------------------" -ForegroundColor Yellow
###

# Start Monitoring
Write-Host "-- Start Monitoring Step --" -ForegroundColor Yellow
invoke-expression "cmd /c start powershell -Command { Set-Location $toolsLocalPath; & $toolsLocalPath\StartMonitoring.bat; }"
Write-Host "------------------" -ForegroundColor Yellow
###

# Start App
Write-Host "-- Start App Step --" -ForegroundColor Yellow
invoke-expression "cmd /c start powershell -Command { Set-Location $buildsLocalPath; & $buildsLocalPath\StartApp.bat; }"
Write-Host "------------------" -ForegroundColor Yellow
###


