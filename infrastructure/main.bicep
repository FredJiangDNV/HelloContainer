targetScope = 'subscription'

@description('Environment name (dev, staging, prod)')
param environmentName string = 'dev'

@description('Location for all resources')
param location string = 'westus3'

@description('Project name prefix')
param projectName string = 'hellocontainer'

// Create resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  name: 'Dev-resoruce'
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

// Outputs
output resourceGroupName string = rg.name
