# How to create resources
Run the following command to create all resources, including the resource group:

`az deployment sub create --template-file main.bicep --location swedencentral --parameters env='<environment>' os='<operating-system>' appName='<app-name>' subscription='<subscription-id>' --confirm-with-what-if`

Example:

`az deployment sub create --template-file main.bicep --location swedencentral --parameters env='dev' os='linux' appName='gridtariff-swaggerui' subscription='<subscription-id>' --confirm-with-what-if`

If the resource group exists, run this command instead:

`az deployment group create --resource-group <rg-name> --template-file webapp.bicep --parameters env='<environment>' os='<operating-system>' appName='<app-name>' --confirm-with-what-if`

If you want to reconfigure the application, just run the `group create` az deployment command again. Note that, since all environment variables and other settings will be
overwritten at reconfigure, they should link to e.g. a keyvault where the real values are stored if they are to be changed after the bicep configuration.

Deleting the resource group is not preferred since it will wipe all logs, costs and other data for
the resources within the resource group. It's better to just overwrite the settings that have
changed which is done when running the bicep code again.

# Deploy Web App
Navigate to the SwaggerUI project folder and run (on Windows):

`.\infrastructure\azure\publish.ps1 -resourceGroup <rg-name> -webAppName <app-service-name> -os windows`

or on Linux for publishing to a Linux Web App:

`pwsh infrastructure/azure/publish.ps1 -resourceGroup <rg-name> -webAppName <app-service-name> -os linux`