
# Set VM IP
Set-Item WSMan:\localhost\Client\TrustedHosts -Value 12.34.567.890

# Setup local port to talk to VM
Get-AzureRmNetworkSecurityGroup -Name [TestAppVm1-nsg] -ResourceGroupName [TestApp] | Add-AzureRmNet
workSecurityRuleConfig -Name AllowingWinRMHTTPS -Description "To Enable PowerShell Remote Access" -Access Allow -Protoco
l Tcp -Direction Inbound -Priority 102 -SourceAddressPrefix Internet -SourcePortRange * -DestinationAddressPrefix * -Des
tinationPortRange 5986 | Set-AzureRmNetworkSecurityGroup

Get-AzureRmNetworkSecurityGroup -Name [TestAppVm1-nsg] -ResourceGroupName [TestApp] | Add-AzureRmNetworkSecurityRuleConfig -Name AllowingWinRMHTTP -Description "To Enable PowerShell Remote Access" -Access Allow -Protocol Tcp -Direction Inbound -Priority 103 -SourceAddressPrefix Internet -SourcePortRange * -DestinationAddressPrefix * -DestinationPortRange 5985 | Set-AzureRmNetworkSecurityGroup
