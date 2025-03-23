targetScope = 'resourceGroup'

@description('Environment for the deployment (e.g., dev, test, prod)')
param env string

@description('Application instances common name')
param appName string

@description('Operating system (linux/windows)')
param os string

@description('Default access to the webapp (deny/allow)')
param defaultAccessPolicy string = 'allow'

@description('Web App Time Zone')
param timeZone string = 'W. Europe Standard Time'

@description('Allow access from IP addresses')
param allowIpAddresses string[] = []

@description('App Service Plan Name')
param appServicePlanName string = 'asp-${appName}-${env}'

@description('Name of the Web App excluding the unique identifier string')
param baseWebAppName string = 'webapp-${appName}-${env}'

@description('Application Insights Name')
param appInsightsName string = 'appi-${appName}-${env}'

@description('Name of the Log Analytics Workspace')
param logAnalyticsWorkspaceName string = 'law-${appName}-${env}'

@description('The geographical location to host all application instances')
param location string = resourceGroup().location

var aspSkuFree = {
  name: 'F1'
  tier: 'Free'
  size: 'F1'
  family: 'F'
}

var aspSkuPremiumP0V3 = {
  name: 'P0v3'
  tier: 'Premium0V3'
  size: 'P0v3'
  family: 'Pv3'
}

var uniqueSuffix = substring(uniqueString(resourceGroup().id), 0, 5)
var webAppName = '${baseWebAppName}-${uniqueSuffix}'

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: (env == 'prod' ? 90 : 30)
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: (os == 'linux' ? 'linux' : 'app')
  sku: (env == 'prod' ? aspSkuPremiumP0V3 : aspSkuFree)
  properties: {
    reserved: (os == 'linux' ? true : false)
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    IngestionMode: 'LogAnalytics'
    Request_Source: 'rest'
    RetentionInDays: (env == 'prod' ? 90 : 30)
  }
}

var ipSecurityRestrictions = [
  for i in range(0, length(allowIpAddresses)): {
    ipAddress: allowIpAddresses[i]
    action: 'Allow'
    priority: 200 + i
    name: 'AllowIp'
  }
]

resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: webAppName
  location: location
  kind: (os == 'linux' ? 'app,linux' : 'app')
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: union(
      os == 'linux'
        ? {
            linuxFxVersion: 'DOTNETCORE|8.0'
            appCommandLine: 'dotnet SwaggerUI.dll'
          }
        : {
            windowsFxVersion: 'dotnet:8'
          },
      {
        appSettings: [
          {
            name: 'WEBSITE_TIME_ZONE'
            value: timeZone
          }
          {
            name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
            value: applicationInsights.properties.InstrumentationKey
          }
          {
            name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
            value: applicationInsights.properties.ConnectionString
          }
        ]
        publicNetworkAccess: 'Enabled'
        ipSecurityRestrictionsDefaultAction: (defaultAccessPolicy == 'allow' ? 'Allow' : 'Deny')
        ipSecurityRestrictions: ipSecurityRestrictions
      }
    )
  }
}

output webAppName string = webApp.name
