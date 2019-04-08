 # monolith
 az vm create --resource-group TestApp --name "monolith" --location "westeurope" --image "vmi-iis" --size Standard_B4ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.4 --tags "persistance=onetime" --public-ip-sku Standard
 
 
 # service.analysis
 az vm create --resource-group TestApp --name "analysis" --location "westeurope" --image "vmi-iis" --size Standard_B2ms --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.14 --tags "persistance=onetime" --public-ip-sku Standard
 
 
 # service.auth
 az vm create --resource-group TestApp --name "auth" --location "westeurope" --image "vmi-iis" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.24 --tags "persistance=onetime" --public-ip-sku Standard
 
  
 # service.reports
 az vm create --resource-group TestApp --name "reports" --location "westeurope" --image "vmi-iis" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.34 --tags "persistance=onetime" --public-ip-sku Standard
 

 
 # service.files
 az vm create --resource-group TestApp --name "files" --location "westeurope" --image "vmi-iis" --size Standard_B2s --admin-username "TestAppAdmin" --admin-password "Pa55word!Pa55word!" --storage-sku Standard_LRS --nsg "TestApp-ngs" --vnet-name "TestApp-vnet" --subnet "TestApp-subnet" --private-ip-address 10.0.0.44 --tags "persistance=onetime" --public-ip-sku Standard
 

 
  