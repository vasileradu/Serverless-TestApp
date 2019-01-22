# monolith
az vm extension set --publisher Microsoft.Compute --version 1.8 --name CustomScriptExtension --vm-name "monolith" --resource-group "TestApp" --settings ".\settings.monolith.json"

# service.analysis
az vm extension set --publisher Microsoft.Compute --version 1.8 --name CustomScriptExtension --vm-name "analysis" --resource-group "TestApp" --settings ".\settings.analysis.json"

# service.auth
az vm extension set --publisher Microsoft.Compute --version 1.8 --name CustomScriptExtension --vm-name "auth" --resource-group "TestApp" --settings ".\settings.auth.json"

# service.reports
az vm extension set --publisher Microsoft.Compute --version 1.8 --name DeployScript CustomScriptExtension --vm-name "reports" --resource-group "TestApp" --settings ".\settings.reports.json"

# service.files
az vm extension set --publisher Microsoft.Compute --version 1.8 --name CustomScriptExtension --vm-name "files" --resource-group "TestApp" --settings ".\settings.files.json"

# jmeter
az vm extension set --publisher Microsoft.Compute --version 1.8 --name CustomScriptExtension --vm-name "jmeter" --resource-group "TestApp" --settings ".\settings.jmeter.json"