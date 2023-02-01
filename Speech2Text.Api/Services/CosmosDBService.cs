﻿using Microsoft.Azure.Cosmos;
using Speech2Text.Api.Models;

namespace Speech2Text.Api.Services
{
    public class CosmosDbService : ICosmosDbService<Transcript>
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddAsync(Transcript item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Transcript>(id, new PartitionKey(id));
        }

        public async Task<Transcript> GetAsync(string id)
        {
			Transcript result;
			try
			{
				var response = await _container.ReadItemAsync<Transcript>(id, new PartitionKey(id));
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

        public async Task<IEnumerable<Transcript>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Transcript>(new QueryDefinition(queryString));

            var results = new List<Transcript>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync(string id, Transcript item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }
    }
}
