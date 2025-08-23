param environmentName string
param location string
param projectName string

// Cosmos DB Account with Free Tier
resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: 'cosmos-${projectName}-${environmentName}'
  location: location
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    databaseAccountOfferType: 'Standard'
    enableFreeTier: true
  }
}

// Database with shared throughput
resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  name: 'HelloContainerDB'
  parent: cosmosDbAccount
  properties: {
    resource: {
      id: 'HelloContainerDB'
    }
    options: {
      throughput: 1000
    }
  }
}

// Containers
resource alertsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'alerts'
  parent: cosmosDbDatabase
  properties: {
    resource: {
      id: 'alerts'
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
}

resource containersContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'containers'
  parent: cosmosDbDatabase
  properties: {
    resource: {
      id: 'containers'
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
}

resource ledgersContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'ledgers'
  parent: cosmosDbDatabase
  properties: {
    resource: {
      id: 'ledgers'
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
}

resource outboxsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'outboxs'
  parent: cosmosDbDatabase
  properties: {
    resource: {
      id: 'outboxs'
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
