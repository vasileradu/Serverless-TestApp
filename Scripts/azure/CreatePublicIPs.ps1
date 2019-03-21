# monolith 
az network public-ip create --name "IP_Monolith" --resource-group TestApp --dns-name "testapp-monolith" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

# service.analysis
az network public-ip create --name "IP_Analysis" --resource-group TestApp --dns-name "testapp-service-analysis" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

# service.auth
az network public-ip create --name "IP_Auth" --resource-group TestApp --dns-name "testapp-service-auth" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

# service.reports
az network public-ip create --name "IP_Reports" --resource-group TestApp --dns-name "testapp-service-reports" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

# service.files
az network public-ip create --name "IP_Files" --resource-group TestApp --dns-name "testapp-service-files" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"

# jmeter
az network public-ip create --name "IP_Jmeter" --resource-group TestApp --dns-name "testapp-jmeter" --location "westeurope" --allocation-method Static --sku Standard --version IPv4 --zone 1 --tags "persistance=onetime"