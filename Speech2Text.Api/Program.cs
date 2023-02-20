using Speech2Text.Core.Models;
using Speech2Text.Core.Services;

// Get services config
var builder = WebApplication.CreateBuilder(args);
var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);

builder.Services.AddSingleton(cosmosDBSettings);

builder.Services.AddControllers().AddNewtonsoftJson(); 

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
