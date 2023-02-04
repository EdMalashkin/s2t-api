using Speech2Text.Api.Models;
using Speech2Text.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var cosmosDbService = new CosmosDbServiceBuilder(builder).GetCosmosDbTaskService();
builder.Services.AddSingleton<ICosmosDbService<Transcript>>(cosmosDbService);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
