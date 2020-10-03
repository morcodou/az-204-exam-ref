#!/bin/bash

#Some variable definition useful for the script
ACR_NAME=az204demo
SP_NAME=az204demo_sp
IMAGE_TAG=az204demo.azurecr.io/develop/foobar:latest

RESOURCE_GROUP=AKSdemo-RG
APP_NAME=foobar
APP_DNS_NAME=prueba

#Get the registry ID. You will need this ID for creating the authorization to the
#service principal
ACR_ID=$(az acr show --name $ACR_NAME --query id --output tsv)

#Get the ACR login server
ACR_SERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)

#Get the service principal password. We will grant pull only privileges to the service
#principal
echo "Generating Service Principal password"
SP_PASS=$(az ad sp create-for-rbac --name http://$SP_NAME --scopes $ACR_ID
--role acrpull --query password --output tsv)

#Get the App ID associated to the service principal
SP_ID=$(az ad sp show --id http://$SP_NAME --query appId --output tsv)

echo "Service principal ID: $SP_ID"

echo "Service principal password: $SP_PASS"

#Create the container in the Container Instance service
az container create --resource-group $RESOURCE_GROUP --name $APP_NAME --image
$IMAGE_TAG --cpu 1 --memory 1 --registry-login-server $ACR_SERVER --registry-username
$SP_ID --registry-password $SP_PASS --dns-name-label $APP_DNS_NAME --ports 80
