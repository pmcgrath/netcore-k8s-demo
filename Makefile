

# Switch to bash
SHELL=/bin/bash


IMAGE_NAME ?= webapi
K8S_VERSION ?= 1.6.0
FULL_IMAGE_NAME ?= pmcgrath/${IMAGE_NAME}
VERSION ?= 1.0


restore:
	dotnet restore


build:
	dotnet build


test:
	@# dotnet test does not work for a solution file at this time
	for project in $(shell find test -name *.csproj); do dotnet test $${project}; done


run-local:
	@# We could have used --project arg rather than cding into the dir, but would have problems with the appSettings files not being found
	@# Have alos overridden the port to illustrate we can control the port
	cd src/webapi && dotnet run 8000


publish:
	@# See https://github.com/dotnet/cli/issues/6154
	dotnet clean --configuration Release
	@# Remove the pub directory as it does not clear any existing content from previous runs
	[[ -d pub ]] && rm -r pub || true
	@# We do so for the project, if we do so for the solution it will include the test assemblies which we do not want in the docker image at this time
	dotnet publish src/webapi/webapi.csproj --configuration Release --output ../../pub /property:Version=${VERSION}


docker-build: publish
	docker image build --build-arg VERSION=${VERSION} --tag ${FULL_IMAGE_NAME}:${VERSION} .


docker-run-local:
	docker container run --detach --name ${IMAGE_NAME} --publish 5000:5000 ${FULL_IMAGE_NAME}:${VERSION}


docker-stop-local:
	docker container rm --force ${IMAGE_NAME}


docker-run-local-redis:
	docker container run --detach --name redis --publish 6379:6379 redis:latest


docker-stop-local-redis:
	docker container rm --force redis:latest


docker-push:
	@# Assumes you have logged into dockerhub
	docker push ${FULL_IMAGE_NAME}:${VERSION}


start-minikube:
	minikube start --kubernetes-version ${K8S_VERSION} -v 10 | tee minikube-start.log


.PHONY: restore build test run-local publish docker-build docker-run-local docker-stop-local docker-run-local-redis docker-stop-local-redis docker-push start-minikube
