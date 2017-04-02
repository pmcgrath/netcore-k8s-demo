

# Switch to bash
SHELL=/bin/bash


IMAGE_NAME?=pmcgrath/webapi
VERSION?=1.1


restore:
	dotnet restore


build:
	dotnet build


test:
	@# dotnet test does not work for a solution file at this time
	for project in $(shell find test -name *.csproj); do dotnet test $${project}; done


run-local:
	dotnet run 8000


run-local-redis:
	docker container run --detach --name redis --publish 6379:6379 redis:latest


publish:
	@# See https://github.com/dotnet/cli/issues/6154
	dotnet clean --configuration Release
	@# Remove the pub directory as it does not clear any existing content from previous runs
	[[ -d pub ]] && rm -r pub || true
	@# We do so for the project, if we do so for the solution it will include the test assemblies which we do not want in the docker image at this time
	dotnet publish src/webapi/webapi.csproj --configuration Release --output ../../pub /property:Version=${VERSION}


docker-build: publish
	docker image build --build-arg VERSION=${VERSION} --tag ${IMAGE_NAME}:${VERSION} .


docker-push:
	@# Assumes you have logged into dockerhub
	docker push ${IMAGE_NAME}:${VERSION}


.PHONY: restore build test run-local run-local-redis publish docker-build docker-push
