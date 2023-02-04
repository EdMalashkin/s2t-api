using Speech2Text.Core.Models;

namespace Speech2Text.Core.Services
{
    public class CosmosDbServiceBuilder
    {
        CosmosDBOptions _cosmosDBOptions;
        public CosmosDbServiceBuilder(CosmosDBOptions cosmosDBOptions)
        {
            _cosmosDBOptions = cosmosDBOptions;
        }
        public async Task<CosmosDbService<Transcript>> GetCosmosDbTaskServiceAsync()
        {
            var databaseName = _cosmosDBOptions.DatabaseName;
            var containerName = _cosmosDBOptions.ContainerName;
            var account = _cosmosDBOptions.EndPoint;
            var key = _cosmosDBOptions.EndPointKey;

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            var cosmosDbService = new CosmosDbService<Transcript>(client, databaseName, containerName);
            return cosmosDbService;
        }
        public CosmosDbService<Transcript> GetCosmosDbTaskService()
        {
            return GetCosmosDbTaskServiceAsync().GetAwaiter().GetResult();
        }
    }
}
