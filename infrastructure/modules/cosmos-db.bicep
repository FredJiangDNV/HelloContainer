@description('Creates Cosmos DB Account')

param environmentName string
param location string
param projectName string

// Cosmos DB Account
resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: 'cosmos-${projectName}-${environmentName}'
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: false
    enableMultipleWriteLocations: false
    capabilities: []
  }
}

// Cosmos DB Database
resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  name: 'HelloContainerDB'
  parent: cosmosDbAccount
  properties: {
    resource: {
      id: 'HelloContainerDB'
    }
    options: {
      throughput: 400
    }
  }
}

// Container for main data
resource cosmosDbContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'Containers'
  parent: cosmosDbDatabase
  properties: {
    resource: {
      id: 'Containers'
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
}

// Outputs
output connectionString string = cosmosDbAccount.listConnectionStrings().connectionStrings[0].connectionString
output accountName string = cosmosDbAccount.name
output databaseName string = cosmosDbDatabase.name
