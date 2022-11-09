
using Microsoft.Extensions.DependencyInjection;
using speech2text.api.Services;
using Speech2Text.Api.Models;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("Speech2TextVaultUri"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddSingleton<ICosmosDbService<Transcript>>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync()
{
    var databaseName = "db1";
    var containerName = "tasks";
    var account = "https://speech2text-cosmosdb.documents.azure.com:443/";
    var key = "7egNq99fnAimJWSS2JHOVRizbzgQKglH51xJh4ZYnA62035a758XaPwyLcqZ8y29E1go8vnrvbILACDbQGOa7g==";

    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    return cosmosDbService;
}