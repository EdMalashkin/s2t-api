using Speech2Text.Core.Models;
using Speech2Text.Core.Services;
using System.Configuration;
using System.Dynamic;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);
builder.Services.AddSingleton(cosmosDBSettings);

var quickChartSettings = builder.Configuration.GetSection("QuickChartSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value);
builder.Services.AddSingleton(quickChartSettings);

builder.Services.AddControllers().AddNewtonsoftJson(); 

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
