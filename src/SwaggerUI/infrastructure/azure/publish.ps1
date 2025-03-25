param (
    [string]$resourceGroup,
    [string]$webAppName,
    [string]$os
)

$version = & openapi --version 2>$null
if (-not $LASTEXITCODE -eq 0) {
    Write-Host "❌ OpenAPI CLI is not installed or not in PATH."
    exit 1
}

if (-not $resourceGroup -or -not $webAppName -or -not $os) {
    Write-Host "Usage: ./deploy.ps1 -resourceGroup <ResourceGroupName> -webAppName <WebAppName> -os <windows/linux>"
    exit 1
}

$publishFolder = "publish"
$zipFile = "publish/app.zip"
if (Test-Path $publishFolder) {
    Remove-Item -Recurse -Force $publishFolder
}
New-Item -ItemType Directory -Path $publishFolder | Out-Null

Write-Host "Building application..."
dotnet build --configuration Release --verbosity quiet

Write-Host "Publishing .NET application..."
if ($os -eq "linux") {
    Write-Host "dotnet publish SwaggerUI.csproj --configuration Release --runtime linux-x64 --self-contained false --output $publishFolder --verbosity quiet"
    dotnet publish SwaggerUI.csproj --configuration Release --runtime linux-x64 --self-contained false --output $publishFolder --verbosity quiet
} else {
    Write-Host "dotnet publish SwaggerUI.csproj --configuration Release --self-contained false --output $publishFolder --verbosity quiet"
    dotnet publish SwaggerUI.csproj --configuration Release --self-contained false --output $publishFolder --verbosity quiet
}

Write-Host "Creating ZIP package..."
Compress-Archive -Path "$publishFolder\*" -DestinationPath $zipFile

Write-Host "Deploying to Azure Web App..."
Write-Host "az webapp deploy --resource-group $resourceGroup --name $webAppName --src-path $zipFile --type zip"
az webapp deploy --resource-group $resourceGroup --name $webAppName --src-path $zipFile --type zip
