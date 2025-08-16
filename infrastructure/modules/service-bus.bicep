@description('Creates Service Bus Namespace')

param environmentName string
param location string
param projectName string

// Service Bus Namespace
resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: 'sb-${projectName}-${environmentName}'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
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

// Service Bus Topic for notifications
resource notificationsTopic 'Microsoft.ServiceBus/namespaces/topics@2022-10-01-preview' = {
  name: 'notifications'
  parent: serviceBusNamespace
  properties: {
    maxSizeInMegabytes: 1024
    defaultMessageTimeToLive: 'P14D'
  }
}

// Subscription for email notifications
resource emailSubscription 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-10-01-preview' = {
  name: 'email-notifications'
  parent: notificationsTopic
  properties: {
    maxDeliveryCount: 10
    deadLetteringOnMessageExpiration: true
    defaultMessageTimeToLive: 'P14D'
  }
}

// Outputs
output connectionString string = listKeys('${serviceBusNamespace.id}/AuthorizationRules/RootManageSharedAccessKey', serviceBusNamespace.apiVersion).primaryConnectionString
output namespaceName string = serviceBusNamespace.name
