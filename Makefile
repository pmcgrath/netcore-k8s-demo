

IMAGE_NAME?=pmcgrath/webapi
VERSION?=1.1


restore:
	dotnet restore


build:
	dotnet build


run-local-redis:
	docker container run --detach --name redis --publish 6379:6379 redis:latest


run-local:
	dotnet run 8000


publish:
	sed -i "s|<Version>.*</Version>|<Version>${VERSION}</Version>|" webapi.csproj
	dotnet publish --configuration Release --output pub


docker-build: publish
	docker image build --build-arg VERSION=${VERSION} --tag ${IMAGE_NAME}:${VERSION} .


docker-push:
	docker push ${IMAGE_NAME}:${VERSION}


.PHONY: restore build run-local publish docker-build docker-push
