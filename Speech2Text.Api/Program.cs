using Speech2Text.Core.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var cosmosDBSettings = new CosmosDBSettings();
builder.Configuration.GetSection("CosmosDBSettings").Bind(cosmosDBSettings);
builder.Services.AddSingleton(cosmosDBSettings);

var quickChartSettings = builder.Configuration.GetSection("QuickChartSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value);
builder.Services.AddSingleton(quickChartSettings);

//.AddNewtonsoftJson()
builder.Services.AddControllers().AddJsonOptions(option =>
{
	option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
