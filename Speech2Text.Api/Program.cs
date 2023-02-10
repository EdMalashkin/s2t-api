using Speech2Text.Core.Models;
using Speech2Text.Core.Services;

// Get services config
var builder = WebApplication.CreateBuilder(args);
var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);

// Inject services builder
var cosmosDbBuilder = new CosmosDbServiceBuilder<Transcript>(cosmosDBSettings);
builder.Services.AddSingleton(cosmosDbBuilder);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
