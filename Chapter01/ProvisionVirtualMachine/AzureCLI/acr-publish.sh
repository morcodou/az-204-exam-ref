az login

az acr login –--name <acr_name>

docker tag foobar <acr_name>.azurecr.io/<repository_name>/<image_name>

docker push <acr_name>.azurecr.io/<repository_name>/<image_name>