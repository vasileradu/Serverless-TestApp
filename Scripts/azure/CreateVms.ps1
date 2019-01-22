 # monolith
 az vm create --resource-group TestApp --name "monolith" --location "westeurope" --image win2016datacenter --size Standard_B4ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.4
 
 # service.analysis
 az vm create --resource-group TestApp --name "analysis" --location "westeurope" --image win2016datacenter --size Standard_B4ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.5
 
 # service.auth
 az vm create --resource-group TestApp --name "auth" --location "westeurope" --image win2016datacenter --size Standard_B1s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.6
  
 # service.reports
 az vm create --resource-group TestApp --name "reports" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.7
 
 # service.files
 az vm create --resource-group TestApp --name "files" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.8 
 
 # jmeter
 az vm create --resource-group TestApp --name "jmeter" --location "westeurope" --image win2016datacenter --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --private-ip-address 10.0.0.9