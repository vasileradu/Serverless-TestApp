<# .SYNOPSIS #>
param (
    [Parameter(Mandatory=$true, HelpMessage = '(1) monolith, (2) service-auth, (3) service-analysis, (4) service-files, (5)service-reports')][int]$app,
	[Parameter(Mandatory=$false, HelpMessage = 'default is Standard_B2s')][string]$size="Standard_B2s"
)

$apps = @("monolith", "service-auth", "service-analysis", "service-files", "service-reports");
$subnets = @("GatewayMonolith", "GatewayAuth", "GatewayAnalysis", "GatewayFiles", "GatewayReports");

if($app -lt 1 -or $app -gt $apps.length) {
	
	Write-Host "-- Invalid app index --" -ForegroundColor Red
	
	return -1;
}

$StartTime = $(get-date)

$app = $app - 1; # index starts from 0;
$appName = $apps[$app];

Write-Host "-- Creating Scale Set for: $appName --" -ForegroundColor Yellow


$ruleName = "AllowAccessGatewayPorts";

$rules = az network nsg rule list -g TestApp --nsg-name TestApp-ngs;
$hasRule = $false;

Foreach ($rule in $rules) {
	if($rule.name -eq $ruleName) {
		$hasRule = $true;
		break;
	}
}

# Create network rule to allow traffic to gateway
if(-not($hasRule)) {
	
	Write-Host "-- Creating NGS Rule for Gateway --" -ForegroundColor Yellow

	az network nsg rule create --name "AllowAccessGatewayPorts" --nsg-name "TestApp-ngs" --priority 120 --resource-group "TestApp" --destination-address-prefixes * --destination-port-ranges * --protocol * --source-address-prefixes "Internet" --source-port-ranges "65200-65535"
}

Write-Host "-- Creating IP for Gateway --" -ForegroundColor Yellow

az network public-ip create --name "IP_$appName" --resource-group TestApp --dns-name "testapp-$appName" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

Write-Host "-- Creating Application Gateway --" -ForegroundColor Yellow

$subnetName = $subnets[$app];
az network application-gateway create --name "AG_$appName" --resource-group "TestApp" --min-capacity 2 --capacity 20 --frontend-port 5000 --http-settings-port 5000 --sku Standard_v2 --subnet "$subnetName" --vnet-name "TestApp-vnet" --location "westeurope" --zone 1 --public-ip-address "IP_$appName" --tags "persistance=onetime"

Write-Host "-- Creating ScaleSet --" -ForegroundColor Yellow

az vmss create --image "vm-image-$appName" --name "vmss$appName" --resource-group TestApp --vm-sku "$size" --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --nsg TestApp-ngs --instance-count 1 --app-gateway "AG_$appName" --subnet "TestApp-subnet" --vnet-name "TestApp-vnet" --upgrade-policy-mode "Automatic" --tags "persistance=onetime"

Write-Host "-- Creating Autoscale rule --" -ForegroundColor Red
az monitor autoscale create --resource-group TestApp --resource "vmss$appName" --resource-type Microsoft.Compute/virtualMachineScaleSets --name "autoscale$appName" --min-count 1 -max-count 5 --count 1

az monitor autoscale rule create -g TestApp --autoscale-name "autoscale$appName" --condition "Percentage CPU > 70 avg 5m" --scale out 1

az monitor autoscale rule create -g TestApp --autoscale-name "autoscale$appName" --condition "Percentage CPU < 30 avg 5m" --scale in 1


$elapsedTime = $(get-date) - $StartTime
$totalTime = "{0:HH:mm:ss}" -f ([datetime]$elapsedTime.Ticks)
Write-Host "-- Finished script: $totalTime --" -ForegroundColor Green

