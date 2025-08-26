### Method 1: Using PowerShell Script (Recommended)

```powershell
# Run in infrastructure directory
.\deploy-cosmos.ps1 -EnvironmentName "dev" -Location "westus3" -ProjectName "hellocontainer"
```

### Using Azure CLI

```bash
# Deploy to subscription level
az deployment sub create -l westus3 -f main.bicep --name cosmos-deployment --verbose --parameters environmentName=dev location=westus3 projectName=hellocontainer

```

