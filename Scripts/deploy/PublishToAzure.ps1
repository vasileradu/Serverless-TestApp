param (
    [Parameter(Mandatory=$true)][string]$app,
    [Parameter(Mandatory=$false)][string]$selfContained="false"
 )

### helper functions
function Get-AppPath ($app) {

	$appRootPath = "C:\Projects\Serverless-TestApp"
	$appPath = ""
	
	switch -wildcard ($app) {
		"monolith" {
			$appPath = "$appRootPath\TestApp.Monolith\TestApp.Monolith\TestApp.Monolith.csproj"			
		}
		"service*" {
			$appPath = "$appRootPath\TestApp.Microservices\TestApp.$app\TestApp.$app\TestApp.$app.csproj"
		}
	}
	
	return $appPath;
}

function Get-BuildsPath ($app) {
	return "C:\Projects\Serverless-TestApp\builds\$app";
}

function Update-Config($app) {

	$buildsPath = Get-BuildsPath $app
	$devConfigPath = "$buildsPath\appsettings.Development.json"
	$prodOldConfigPath = "$buildsPath\appsettings.json"
	$prodNewConfigPath = "C:\Projects\Serverless-TestApp\SecretConfig\$app.appsettings.azure.json"

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

	$startText = "dotnet TestApp.$app.dll --hosturl http://0.0.0.0:5000";
	
	if($selfContained -eq "true") {
		Copy-Item -Path $prodNewConfigPath -Destination $prodOldConfigPath -Force
		Rename-Item -Path "$buildsPath\TestApp.$app.exe" -NewName "TestApp.exe"
		
		$startText = "TestApp.exe --hosturl http://0.0.0.0:5000";
	}
	
	$startText | Set-Content "$buildsPath\StartApp.bat"
	
	Write-Host "Updated config to production settings" -ForegroundColor Green	
}

function Archive-App($app) {
	$source = Get-BuildsPath $app
	$destination = Get-BuildsPath "$app.zip"
	
	if(Test-path $destination) {
		Remove-item $destination
		Write-Host "Removed $destination"
	}	
	
	Add-Type -assembly "system.io.compression.filesystem"	
	[io.compression.zipfile]::CreateFromDirectory($source, $destination);	
	
	Write-Host "Created archive for: $destination" -ForegroundColor Green
	
	return $destination;
}

######
 
$appPath = Get-AppPath $app;
$buildsPath = Get-BuildsPath $app;

Write-Host "Starting publish for application: $app ...self-contained:$selfContained"

Write-Host "-- Cleanup Step --" -ForegroundColor Yellow
if (Test-Path $buildsPath) {
	Remove-Item $buildsPath -Recurse
	Write-Host "Cleanup successfull: $buildsPath" -ForegroundColor Green
}
Write-Host "------------------" -ForegroundColor Yellow

Write-Host "-- Publish Step --" -ForegroundColor Yellow
Write-Host "Publishing $appPath to $buildsPath"
$publishResult = & dotnet publish $appPath -c Release -r win-x64 --self-contained $selfContained -o $buildsPath
Write-Host "$publishResult"
Write-Host "------------------" -ForegroundColor Yellow

## Update Config Step
Write-Host "-- Update Config Step --" -ForegroundColor Yellow
Update-Config $app
Write-Host "------------------" -ForegroundColor Yellow

## Zip Step
Write-Host "-- Archive Step --" -ForegroundColor Yellow
$archivePath = Archive-App $app
Write-Host "------------------" -ForegroundColor Yellow

## Copy to Cloud Step
Write-Host "-- Copy-to-Cloud Step --" -ForegroundColor Yellow

$cloudPath = "\\testappcloudstorage.file.core.windows.net\config\builds"
Write-Host "Copying $archivePath to $cloudPath ..."
Copy-Item -Path $archivePath -Destination $cloudPath -Force

Write-Host "------------------" -ForegroundColor Yellow

Write-Host "-- All done ! --" -ForegroundColor Green

