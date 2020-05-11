using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UQ.Demo.Models;

namespace UQ.Demo.Services
{
    public abstract class CosmosDbService<T> 
        : ICosmosDbService<T> where T : IEntity, new()
    {
        protected readonly Container _container;
        public string Query { get; set; }
        protected int MaxItemCount = 64;

        public CosmosDbService(Container container)
        {
            this._container = container;
        }

        public async Task AddEntityAsync(T item)
        {
            await this._container.CreateItemAsync<T>(item);
        }

        public async Task DeleteEntityAsync(string id)
        {
            await this._container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T> GetEntityAsync(string id)
        {
            return await this._container.ReadItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task UpdateEntityAsync(T item)
        {
            await this._container.UpsertItemAsync<T>(item);
        }

        public async Task UpdateEntitiesAsync(IEnumerable<T> items)
        {
            foreach (var item in items)
                await UpdateEntityAsync(item);

        }

        public async Task<IEnumerable<T>> GetEntitiesAsync(string whereClause)
        {
            string queryString = Query; // Query provided in derived class
            if (whereClause != null)
            {
                queryString += $"WHERE ({whereClause})"; 
            }

            var iterator = this._container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
