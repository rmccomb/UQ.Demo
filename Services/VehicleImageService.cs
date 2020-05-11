using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UQ.Demo.Models;

namespace UQ.Demo.Services
{
    public interface IVehicleImageService : ICosmosDbService<VehicleImage>
    {
    }

    public class VehicleImageService : CosmosDbService<VehicleImage>, IVehicleImageService
    {
        public static async Task<IVehicleImageService> InitializeCosmosClientInstanceAsync(IConfigurationSection configSection)
        {
            string connectionString = configSection.GetSection("PrimaryConnectionString").Value;
            string databaseName = configSection.GetSection("DatabaseName").Value;
            string containerName = configSection.GetSection("ContainerName").Value;
            string partitionKey = configSection.GetSection("PartitionKey").Value;

            var clientBuilder = new CosmosClientBuilder(connectionString);
            CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();

            // Craete database
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            // Create containers
            await database.Database.CreateContainerIfNotExistsAsync(containerName, partitionKey);

            return new VehicleImageService(client, databaseName, containerName);
        }

        public VehicleImageService(
            CosmosClient dbClient, 
            string databaseName, 
            string containerName) 
            : base(dbClient, databaseName, containerName)
        {
            Query = $"SELECT TOP {MaxItemCount} * from c";
        }

        public IQueryable<VehicleImage> GetJobIterator() => _container
            .GetItemLinqQueryable<VehicleImage>(requestOptions: new QueryRequestOptions { MaxItemCount = this.MaxItemCount });


    }
}
