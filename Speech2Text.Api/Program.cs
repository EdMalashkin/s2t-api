using Speech2Text.Api.Models;
using Speech2Text.Api.Services;

// Get services config
var builder = WebApplication.CreateBuilder(args);
var cosmosDBOptions = new CosmosDBOptions();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBOptions);

// Add services to the container.
var cosmosDbService = new CosmosDbServiceBuilder(cosmosDBOptions).GetCosmosDbTaskService();
builder.Services.AddSingleton<ICosmosDbService<Transcript>>(cosmosDbService);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
