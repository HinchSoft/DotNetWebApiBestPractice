dotnet tool update --global dotnet-ef

$description = Read-Host -Prompt "Migration description"

dotnet ef migrations add $description `
 --project ./Demo/src/DemoInfrastructure/DemoInfrastructure.csproj `
 --startup-project ./Demo/src/DemoApi/DemoApi.csproj
