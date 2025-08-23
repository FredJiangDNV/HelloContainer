param(
    [string]$EnvironmentName = "dev",
    [string]$Location = "eastus",
    [string]$ProjectName = "hellocontainer"
)

Write-Host "Deploying Cosmos DB..." -ForegroundColor Green

az deployment sub create `
    --name "cosmos-$(Get-Date -Format 'yyyyMMdd-HHmmss')" `
    --location $Location `
    --template-file "main.bicep" `
    --parameters environmentName=$EnvironmentName location=$Location projectName=$ProjectName

if ($LASTEXITCODE -eq 0) {
    Write-Host "Success!" -ForegroundColor Green
} else {
    Write-Host "Failed!" -ForegroundColor Red
    exit 1
}
