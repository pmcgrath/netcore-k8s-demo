#


# dotnet solution and project setup
# See https://docs.microsoft.com/en-us/dotnet/articles/core/tools/
```
# Repo root dir
mkdir k8s
cd k8s

# Create solution file
dotnet new sln

# Create and add web api project to solution
dotnet new webapi --output src/webapi
dotnet sln add src/webapi/webapi.csproj

# Create and add test project to solution
dotnet new xunit --output test/webapi.test
dotnet sln add test/webapi.test/webapi.test.csproj

# Add project reference - test ref src
dotnet add test/webapi.test/webapi.test.csproj reference src/webapi/webapi.csproj

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
./k8s.sln
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
```



# VS Code debugging
- Need to configure a launch task
- Needed to cater for running debug from the root - solution directory, in which case it cannot find the appSettings files, see http://stackoverflow.com/questions/38986736/config-json-not-being-found-on-asp-net-core-startup-in-debug



# Pending
- Authentication middleware
- Metrics middleware
	- Prometheus
- XmlSerialization
- Control logging
- Redis repository for kubernetes
- Dockerfile
- Dockerhub - 2 versions
