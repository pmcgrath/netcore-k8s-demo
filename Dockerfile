FROM		microsoft/dotnet:1.1.1-runtime

ARG 		VERSION=1

LABEL		app=webapi-demo
LABEL  		version=$VERSION

WORKDIR		/app
COPY 		pub .

EXPOSE 		5000

ENTRYPOINT 	["dotnet", "webapi.dll"]
