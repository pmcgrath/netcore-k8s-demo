FROM		microsoft/aspnetcore:1.1.1

ARG 		VERSION=1

LABEL		app=webapi-demo
LABEL  		version=$VERSION

WORKDIR		/app
COPY 		pub .

EXPOSE 		5000

ENTRYPOINT 	["dotnet", "webapi.dll"]
