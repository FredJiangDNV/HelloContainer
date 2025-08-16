@description('Creates Container App for API')

param environmentName string
param location string
param projectName string
param imageTag string
param containerAppsEnvironmentId string
param acrLoginServer string
param cosmosDbConnectionString string
param serviceBusConnectionString string
param appInsightsConnectionString string

// Container App for API
resource apiContainerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: 'ca-${projectName}-api-${environmentName}'
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
      secrets: [
        {
          name: 'cosmos-connection-string'
          value: cosmosDbConnectionString
        }
        {
          name: 'servicebus-connection-string'
          value: serviceBusConnectionString
        }
        {
          name: 'appinsights-connection-string'
          value: appInsightsConnectionString
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: '${acrLoginServer}/hellocontainer-api:${imageTag}'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:80'
            }
            {
              name: 'ConnectionStrings__CosmosDB'
              secretRef: 'cosmos-connection-string'
            }
            {
              name: 'DatabaseSettings__DatabaseName'
              value: 'HelloContainerDB'
            }
            {
              name: 'ServiceBusConnection'
              secretRef: 'servicebus-connection-string'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              secretRef: 'appinsights-connection-string'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '30'
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
  name: guid(apiContainerApp.id, 'acrpull')
  scope: resourceGroup()
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull
    principalId: apiContainerApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// Outputs
output fqdn string = apiContainerApp.properties.configuration.ingress.fqdn
output name string = apiContainerApp.name
