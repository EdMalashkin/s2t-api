using Microsoft.Azure.Cosmos;

namespace Speech2Text.Core.Services
{
    public class CosmosDbService<T> : ICosmosDbService<T>
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddAsync(string id, T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(id));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T> GetAsync(string id)
        {
			T result;
			try
			{
				var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
				result = response.Resource;
			}
			catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound) 
			{
				throw new KeyNotFoundException(id);
			}
			catch (Exception)
			{
				throw;
			}
			return result;
        }

        public async Task<IEnumerable<T>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
