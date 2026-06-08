Check dotnet-f tool at latest version

`dotnet tool install --global dotnet-ef`

To Create migrations

+ in package manager set default project to DemoApi
+ change directory to DemoApi
+ Run: `dotnet ef migrations add InitialCreate --project ../Demo.Infrastructure/Demo.Infrastructure.csproj`


To update database
+ Run: `dotnet ef database update --project ../Demo.Infrastructure/Demo.Infrastructure.csproj`