@description('Creates Service Bus Namespace')

param environmentName string
param location string
param projectName string

// Service Bus Namespace
resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: 'sb-${projectName}-${environmentName}'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
  properties: {
    minimumTlsVersion: '1.2'
  }
}

// Service Bus Queue for container events
resource containerEventsQueue 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  name: 'container-events'
  parent: serviceBusNamespace
  properties: {
    maxSizeInMegabytes: 1024
    defaultMessageTimeToLive: 'P14D'
    deadLetteringOnMessageExpiration: true
    maxDeliveryCount: 10
  }
}

// Outputs
output connectionString string = listKeys('${serviceBusNamespace.id}/AuthorizationRules/RootManageSharedAccessKey', serviceBusNamespace.apiVersion).primaryConnectionString
output namespaceName string = serviceBusNamespace.name
