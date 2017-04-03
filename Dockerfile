# Look into multi-stage builds so we can do the build from within a dockerfile rather than manually doing so or requiring dotnet on the host
# See http://blog.alexellis.io/mutli-stage-docker-builds/
FROM		microsoft/dotnet:1.1.1-runtime

ARG 		VERSION=1

LABEL		app=webapi-demo
LABEL  		version=$VERSION

WORKDIR		/app
COPY 		pub .

EXPOSE 		5000

ENTRYPOINT 	["dotnet", "webapi.dll"]
