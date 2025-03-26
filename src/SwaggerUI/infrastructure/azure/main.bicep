targetScope = 'subscription'

@description('Environment for the deployment (e.g., dev, test, prod)')
param env string

@description('Subscription ID')
param subscription string

@description('Application instances common name')
param appName string

@description('Operating system (linux/windows)')
param os string

@description('Location for resources')
param location string = 'swedencentral'

@description('Name of the resource group to be created')
param resourceGroupName string = 'rg-app-${appName}-${env}'

@description('Set to false if only the resource group shall be created')
param configureApplication bool = true

module createResourceGroup './rg.bicep' = {
  name: 'create-rg-${env}'
  scope: az.subscription(subscription)
  params: {
    resourceGroupName: resourceGroupName
    location: location
  }
}

module deployApplication './webapp.bicep' = if (configureApplication) {
  name: 'deploy-app-${env}'
  scope: az.resourceGroup(subscription, resourceGroupName)
  params: {
    env: env
    appName: appName
    os: os
  }
  dependsOn: [
    createResourceGroup
  ]
}

output rgName string = createResourceGroup.outputs.rgName
output webAppName string = deployApplication.outputs.webAppName
