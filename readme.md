# Purpose
This repository contains a simple dotnet core app, so I can demonstrate using kubernetes to manage the same

Not much to see here, code wise, just using to illustrate

To view the [k8s.slide](slides) in a browser will need to use the golang [https://godoc.org/golang.org/x/tools/present](slide) tool, just run it from the same folder



# dotnet as of May 2016
- Get [here](https://www.microsoft.com/net/core#linuxubuntu)
- Possibly wiser to wait on .NET standard 2 at this time, see these links
	- [Sample dotnet core 2.0 early usage](https://jeremylindsayni.wordpress.com/2017/04/02/installing-ubuntu-16-04-on-a-raspberry-pi-3-installing-net-core-2-and-running-a-sample-net-core-2-app/)
	- [Core setup](https://github.com/dotnet/core-setup/)



# Kubernetes
- Will use minikube here, see the k8s directory content



# dotnet solution and project setup

This is not being maintained, just so I can quickly find

See [tools](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/)

```
# Repo root dir
mkdir netcore-k8s-demo
cd netcore-k8s-demo

# Create solution file
dotnet new sln --name webapi

# Create and add web api project to solution
dotnet new webapi --output src/webapi
dotnet sln add src/webapi/webapi.csproj

# Create and add test project to solution
dotnet new xunit --output test/webapi.test
dotnet sln add test/webapi.test/webapi.test.csproj

# Add project reference - test ref src
dotnet add test/webapi.test/webapi.test.csproj reference src/webapi/webapi.csproj

# Add nuget package reference
dotnet add src/webapi/webapi.csproj package StackExchange.Redis --version 1.2.1

# Show solution info
dotnet sln list

Project reference(s)
--------------------
src
src/webapi/webapi.csproj
test
test/webapi.test/webapi.test.csproj

# Show generated content
find

.
./webapi.sln
./src
./src/webapi
./src/webapi/appsettings.json
./src/webapi/webapi.csproj
./src/webapi/Program.cs
./src/webapi/appsettings.Development.json
./src/webapi/Controllers
./src/webapi/Controllers/ValuesController.cs
./src/webapi/Startup.cs
./src/webapi/wwwroot
./test
./test/webapi.test
./test/webapi.test/webapi.test.csproj
./test/webapi.test/UnitTest1.cs

# Restore so all referenced packages are available locally for building
dotnet restore
```



# Testing
Just using curl to demonstrate

```
# Create new contacts
curl -X POST -H 'Content-Type: application/json' -d '{ "name": "Ted", "MobileNumber": "11" }' http://localhost:5000/contacts
curl -X POST -H 'Content-Type: application/xml' -d '<NewContact><Name>Toe</Name><MobileNumber>22</MobileNumber></NewContact>' http://localhost:5000/contacts

# Get all
curl -H 'Accept: application/json' http://localhost:5000/contacts
curl -H 'Accept: application/xml' http://localhost:5000/contacts
```



# Pending
- Authentication middleware
	- OAuth, OpendID conenct - When I get a chance
- Metrics middleware
	- Prometheus, waiting on PRs for the [same](https://github.com/andrasm/prometheus-net)
- Content negotiation
	- XmlSerialization, If I get a chance, have included but does not seem to be respecting Accept header
