@description('Creates Container App for Azure Function')

param environmentName string
param location string
param projectName string
param imageTag string
param containerAppsEnvironmentId string
param acrLoginServer string
param cosmosDbConnectionString string
param serviceBusConnectionString string
param storageConnectionString string
param appInsightsConnectionString string

// Container App for Azure Function
resource functionContainerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: 'ca-${projectName}-func-${environmentName}'
  location: location
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: false
        targetPort: 80
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
          name: 'storage-connection-string'
          value: storageConnectionString
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
          name: 'function'
          image: '${acrLoginServer}/hellocontainer-function:${imageTag}'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'AzureWebJobsStorage'
              secretRef: 'storage-connection-string'
            }
            {
              name: 'FUNCTIONS_WORKER_RUNTIME'
              value: 'dotnet-isolated'
            }
            {
              name: 'ServiceBusConnection'
              secretRef: 'servicebus-connection-string'
            }
            {
              name: 'CosmosDB'
              secretRef: 'cosmos-connection-string'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              secretRef: 'appinsights-connection-string'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 10
        rules: [
          {
            name: 'servicebus-scaling'
            custom: {
              type: 'azure-servicebus'
              metadata: {
                queueName: 'container-events'
                messageCount: '5'
              }
              auth: [
                {
                  secretRef: 'servicebus-connection-string'
                  triggerParameter: 'connection'
                }
              ]
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
  name: guid(functionContainerApp.id, 'acrpull')
  scope: resourceGroup()
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull
    principalId: functionContainerApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// Outputs
output fqdn string = functionContainerApp.properties.configuration.ingress != null ? functionContainerApp.properties.configuration.ingress.fqdn : ''
output name string = functionContainerApp.name
