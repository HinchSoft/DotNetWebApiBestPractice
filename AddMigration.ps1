dotnet tool update --global dotnet-ef

$description = Read-Host -Prompt "Migration description"

dotnet ef migrations add $description `
 --project ./Demo.Infrastructure/Demo.Infrastructure.csproj `
 --startup-project ./DemoApi/DemoApi.csproj
