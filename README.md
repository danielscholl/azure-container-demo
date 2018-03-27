# Azure Container Demo

This is a code base used to help demo different scenarios for using containers in Azure.

__Requirements:__

- Azure Subscription
- Azure CLI (2.0)
- direnv

## Installation
### Clone the repo

```
git clone https://github.com/danielscholl/azure-container-demo
cd azure-container-demo
```

### Create the private ssh keys

Access to any servers is via a private ssh session and requires the user to create the SSH Keys in the .ssh directory.

```bash
mkdir .ssh && cd .ssh
ssh-keygen -t rsa -b 2048 -C "azureuser@email.com" -f id_rsa
```

### Create the Environment File

The solution uses the utility direnv to read environment variables and apply them retrieve required settings automatically.

Copy the .env_sample to .envrc and edit to set the required environment variables


```bash
export AZURE_SUBSCRIPTION=<your_subscription_id>
export AZURE_LOCATION=<region_location>
export REGISTRY=localhost:5000
```

## Create an Azure Container Registry

### Azure Service Price/Feature Summary

__Basic SKU__

- Cost Optimized Entry Point for Developers
- Integration to Azure AD for Authentication
- 10 GB Storage
- 30 MBps Download 10 MBps Upload
- 2 Web Hooks
- $5.00 /month

__Standard SKU__

- Most Production Scenarios
- 100 GB Storage
- 60 MBps Download 20 MBps Upload
- 10 Web Hooks
- $20.00 /month

__Premium SKU__

- Geo Replication
- 500 GB Storage
- 100 MBps Download 50 MBps Upload
- 100 Web Hooks
- $5.00 /month

### Configure a Registry

1. Create a Container Registry (portal/cli)

```bash
ResourceGroup="k8s-demo"
Location="eastus"
RegistryName="<your_registry_name>"

# Create Resource Group
az group create --name ${ResourceGroup} \
  --location ${Location}


# Create a Container Registry
az acr create --name ${RegistryName} \
  --resource-group ${ResourceGroup} \
  --sku Basic

```

1. Login to the Registry  (cli)

```bash
RegistryName="<your_registry_name>"

# Login to the Registry
az acr login --name ${RegistryName}


# Get the Registry FQDN and set it
AZURE_REGISTRY=$(az acr list --resource-group ${ResourceGroup} \
  --query "[].{acrLoginServer:loginServer}" \
  -otsv)
```

>Note: You can now set the Registry into the .envrc file and resource it.


## Build and push the Sample Hello-World Application

```bash
cd apps/hello-world
docker build -t ${AZURE_REGISTRY}/hello-world:latest .
docker push ${AZURE_REGISTRY}/hello-world:latest
```

### Setup a Webhook

Webhooks can be used to fire on Registry Events.

Note: New feature only supports Headers and not Request Body Changes.

Use an App Service Function that modifies the payload and puts it on the Request body.

1. Create an Incoming Web Hook in Slack

1. Create an App Service Function that posts to the Slack Web Hook

1. Create a WebHook on the Repository that posts to the App Service Function

Now whenever a new image is pushed it will post into a Slack Channel


## Repository Level Delete

Delete functionality is available at the Repository or Tag Level.

```bash
RegistryName="<your_registry_name>"

# List the Repositories
az acr repository list -n ${RegistryName}

# Delete a Repository
az acr repository delete -n ${RegistryName} --repository hello-world
```
