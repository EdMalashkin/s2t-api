﻿using Speech2Text.Api.Models;

namespace Speech2Text.Api.Services
{
    public class CosmosDbServiceBuilder
    {
        CosmosDBOptions _cosmosDBOptions;
        public CosmosDbServiceBuilder(WebApplicationBuilder builder)
        {
            _cosmosDBOptions = new CosmosDBOptions();
            builder.Configuration.GetSection("CosmosDBSettings").Bind(_cosmosDBOptions);
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
