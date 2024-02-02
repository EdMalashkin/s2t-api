using Speech2Text.Core.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);
builder.Services.AddSingleton(cosmosDBSettings);

var quickChartSettings = builder.Configuration.GetSection("QuickChartSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value);
builder.Services.AddSingleton(quickChartSettings);

var origins = builder.Configuration.GetSection("AllowedCorsOrigins").Value?.Split(";") ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => {policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();});
});

builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // doesn't work for some reason
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
