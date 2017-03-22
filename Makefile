

IMAGE_NAME?=pmcgrath/webapi
VERSION?=1


restore:
	dotnet restore


build:
	dotnet build


run-local:
	dotnet run 8000


publish:
	dotnet publish --configuration Release --output pub


build-docker-image: publish
	docker image build --build-arg VERSION=${VERSION} --tag ${IMAGE_NAME}:${VERSION} .


.PHONY: restore build run-local publish build-docker-image
