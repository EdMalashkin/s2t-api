using Microsoft.Azure.Cosmos;

namespace Speech2Text.Core.Services
{
    public class CosmosDbServiceBuilder<T>
    {
        private readonly string endPoint;
        private readonly string endPointKey;
        private readonly string databaseName;
        private string containerName;
        public CosmosDbServiceBuilder(CosmosDBSettings cosmosDBOptions)
        {
            databaseName = cosmosDBOptions.DatabaseName;
            containerName = cosmosDBOptions.ContainerName;
            endPoint = cosmosDBOptions.EndPoint;
            endPointKey = cosmosDBOptions.EndPointKey;
        }
        public CosmosDbServiceBuilder(string endPoint, string endPointKey, string dbName, string containerName)
        {
            this.endPoint = endPoint;
            this.endPointKey = endPointKey;
            this.databaseName = dbName;
            this.containerName = containerName;
        }
        public async Task<CosmosDbService<T>> GetCosmosDbServiceAsync()
        {
            var client = new CosmosClient(endPoint, endPointKey, new CosmosClientOptions()
			{
				SerializerOptions = new CosmosSerializationOptions()
				{
					PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
					IgnoreNullValues = true
				}
			});
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new CosmosDbService<T>(client, databaseName, containerName);
            return cosmosDbService;
        }
		public async Task<CosmosDbService<T>> GetCosmosDbServiceAsync(string containerName)
		{
			this.containerName = containerName; // override the one from cosmosDBOptions
			return await GetCosmosDbServiceAsync();
		}
		public CosmosDbService<T> GetCosmosDbService()
        {
            return GetCosmosDbServiceAsync().GetAwaiter().GetResult();
        }
		public CosmosDbService<T> GetCosmosDbService(string containerName)
		{
			return GetCosmosDbServiceAsync(containerName).GetAwaiter().GetResult();
		}


	}
}
