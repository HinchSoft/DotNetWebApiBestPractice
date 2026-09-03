var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("pgsql")
    .WithPgWeb()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("demodb");

var migration = builder.AddProject<Projects.AspireMigration>("aspiremigration")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.DemoApi>("demoapi")
    .WithReference(db)
    .WaitFor(db)
    .WaitFor(migration);


await builder.Build().RunAsync();