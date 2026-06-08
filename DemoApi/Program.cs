using Api.Common;
using Demo.Application.Handlers;
using Demo.Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.AddGlobalProblemDetails();
builder.Services.AddValidation();

builder.AddNpgsqlDbContext<ApplicationDbContext>("demodb");


builder.Services.AddRepositories();
builder.Services.AddCQRS(typeof(AddUserCommand).Assembly);
builder.Services.AddValidators();

builder.AddEndpoints();
builder.AddApiResponses();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Mars)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

// used to enable Problem Detail returns
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEndpoints();

app.Run();

