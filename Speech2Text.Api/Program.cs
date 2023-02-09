using Speech2Text.Core.Models;
using Speech2Text.Core.Services;

// Get services config
var builder = WebApplication.CreateBuilder(args);
var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);

// Add services to the container.
var cosmosDbService = new CosmosDbServiceBuilder<Transcript>(cosmosDBSettings).GetCosmosDbTaskService();
builder.Services.AddSingleton<ICosmosDbService<Transcript>>(cosmosDbService);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
