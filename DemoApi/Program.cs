using Api.Common;
using CQRSServices.ApiResults;
using DemoApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DataService>();

builder.Services.AddScoped<IHttpResultService, HttpResultService>();
builder.Services.AddHttpContextAccessor();

builder.AddEndpoints();
builder.AddCQRS();
builder.AddApiResponses();

var app = builder.Build();

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

app.MapEndpoints();

app.Run();

