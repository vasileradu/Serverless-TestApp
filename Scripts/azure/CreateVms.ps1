 # monolith
 az vm create --resource-group TestApp --name "monolith" --location "westeurope" --image "vm-image-monolith" --size Standard_B4ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.4 --public-ip-address "IP_Monolith" --tags "persistance=onetime"
 
 az vm create --resource-group TestApp --name "monolith" --location "westeurope" --image win2016datacenter --size Standard_B4ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.4 --public-ip-address "IP_Monolith" --tags "persistance=onetime"
 
 # service.analysis
 az vm create --resource-group TestApp --name "analysis" --location "westeurope" --image "vm-image-service-analysis" --size Standard_B2ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.5 --public-ip-address "IP_Analysis" --tags "persistance=onetime"
 
 az vm create --resource-group TestApp --name "analysis" --location "westeurope" --image win2016datacenter --size Standard_B2ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.5 --public-ip-address "IP_Analysis" --tags "persistance=onetime"
 
 # service.auth
 az vm create --resource-group TestApp --name "auth" --location "westeurope" --image "vm-image-service-auth" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.6 --public-ip-address "IP_Auth" --tags "persistance=onetime"
 
 az vm create --resource-group TestApp --name "auth" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.6 --public-ip-address "IP_Auth" --tags "persistance=onetime"
  
 # service.reports
 az vm create --resource-group TestApp --name "reports" --location "westeurope" --image "vm-image-service-reports" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.7 --public-ip-address "IP_Reports" --tags "persistance=onetime"
 
 az vm create --resource-group TestApp --name "reports" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.7 --public-ip-address "IP_Reports" --tags "persistance=onetime"
 
 # service.files
 az vm create --resource-group TestApp --name "files" --location "westeurope" --image "vm-image-service-files" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.8 --public-ip-address "IP_Files" --tags "persistance=onetime"
 
 az vm create --resource-group TestApp --name "files" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.8 --public-ip-address "IP_Files" --tags "persistance=onetime" 
 
 # jmeter
 az vm create --resource-group TestApp --name "jmeter" --location "westeurope" --image win2016datacenter --size Standard_D1_v2 --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.9 --tags "persistance=onetime"
  