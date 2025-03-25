targetScope = 'subscription'

@description('Name of the Resource Group')
param resourceGroupName string

@description('The location to host the resource group')
param location string

resource appResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
  name: resourceGroupName
  location: location
}

output rgName string = appResourceGroup.name
