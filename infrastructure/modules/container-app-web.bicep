@description('Creates Container App for Web Frontend')

param environmentName string
param location string
param projectName string
param imageTag string
param containerAppsEnvironmentId string
param acrLoginServer string
param apiAppUrl string

// Container App for Web Frontend
resource webContainerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: 'ca-${projectName}-web-${environmentName}'
  location: location
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 80
        allowInsecure: false
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
      }
      registries: [
        {
          server: acrLoginServer
          identity: 'system'
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'web'
          image: '${acrLoginServer}/hellocontainer-web:${imageTag}'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'API_BASE_URL'
              value: 'https://${apiAppUrl}'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 5
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

// Grant ACR pull permissions to the container app
resource acrPullRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(webContainerApp.id, 'acrpull')
  scope: resourceGroup()
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull
    principalId: webContainerApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// Outputs
output fqdn string = webContainerApp.properties.configuration.ingress.fqdn
output name string = webContainerApp.name
