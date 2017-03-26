

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
	# See https://github.com/dotnet/cli/issues/6154
	dotnet clean --configuration Release
	dotnet publish --configuration Release --output pub /property:Version=${VERSION}


docker-build: publish
	docker image build --build-arg VERSION=${VERSION} --tag ${IMAGE_NAME}:${VERSION} .


docker-push:
	docker push ${IMAGE_NAME}:${VERSION}


.PHONY: restore build run-local publish docker-build docker-push
