@description('Main Bicep template for HelloContainer Azure Container Apps deployment')

targetScope = 'subscription'

@description('Environment name (dev, staging, prod)')
param environmentName string = 'dev'

@description('Location for all resources')
param location string = 'eastus'

@description('Project name prefix')
param projectName string = 'hellocontainer'

@description('Container image tags')
param imageTag string = 'latest'

// Create resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${projectName}-${environmentName}'
  location: location
}

// Deploy Container Apps Environment
module containerAppsEnvironment 'modules/container-apps-environment.bicep' = {
  name: 'containerAppsEnvironment'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy Azure Container Registry
module acr 'modules/container-registry.bicep' = {
  name: 'containerRegistry'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy Cosmos DB
module cosmosDb 'modules/cosmos-db.bicep' = {
  name: 'cosmosDb'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy Service Bus
module serviceBus 'modules/service-bus.bicep' = {
  name: 'serviceBus'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy Storage Account for Functions
module storage 'modules/storage.bicep' = {
  name: 'storage'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy Application Insights
module appInsights 'modules/app-insights.bicep' = {
  name: 'appInsights'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
  }
}

// Deploy API Container App
module apiApp 'modules/container-app-api.bicep' = {
  name: 'apiApp'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
    imageTag: imageTag
    containerAppsEnvironmentId: containerAppsEnvironment.outputs.environmentId
    acrLoginServer: acr.outputs.loginServer
    cosmosDbConnectionString: cosmosDb.outputs.connectionString
    serviceBusConnectionString: serviceBus.outputs.connectionString
    appInsightsConnectionString: appInsights.outputs.connectionString
  }
  dependsOn: [
    containerAppsEnvironment
    acr
    cosmosDb
    serviceBus
    appInsights
  ]
}

// Deploy Web Container App
module webApp 'modules/container-app-web.bicep' = {
  name: 'webApp'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
    imageTag: imageTag
    containerAppsEnvironmentId: containerAppsEnvironment.outputs.environmentId
    acrLoginServer: acr.outputs.loginServer
    apiAppUrl: apiApp.outputs.fqdn
  }
  dependsOn: [
    containerAppsEnvironment
    acr
    apiApp
  ]
}

// Deploy Function Container App
module functionApp 'modules/container-app-function.bicep' = {
  name: 'functionApp'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    projectName: projectName
    imageTag: imageTag
    containerAppsEnvironmentId: containerAppsEnvironment.outputs.environmentId
    acrLoginServer: acr.outputs.loginServer
    cosmosDbConnectionString: cosmosDb.outputs.connectionString
    serviceBusConnectionString: serviceBus.outputs.connectionString
    storageConnectionString: storage.outputs.connectionString
    appInsightsConnectionString: appInsights.outputs.connectionString
  }
  dependsOn: [
    containerAppsEnvironment
    acr
    cosmosDb
    serviceBus
    storage
    appInsights
  ]
}

// Outputs
output resourceGroupName string = rg.name
output containerRegistryLoginServer string = acr.outputs.loginServer
output apiAppUrl string = apiApp.outputs.fqdn
output webAppUrl string = webApp.outputs.fqdn
output functionAppUrl string = functionApp.outputs.fqdn
