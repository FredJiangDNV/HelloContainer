@description('Creates Azure Container Registry')

param environmentName string
param location string
param projectName string

// Azure Container Registry
resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: 'acr${projectName}${environmentName}'
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    publicNetworkAccess: 'Enabled'
  }
}

// Outputs
output loginServer string = containerRegistry.properties.loginServer
output name string = containerRegistry.name
output resourceId string = containerRegistry.id
