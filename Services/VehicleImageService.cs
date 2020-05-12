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
        Task<IEnumerable<VehicleImage>> FindByVehicleId(int? vehicleId);
    }

    public class VehicleImageService : CosmosDbService<VehicleImage>, IVehicleImageService
    {
        public static async Task<IVehicleImageService> InitializeCosmosClientInstanceAsync(IConfigurationSection configSection)
        {
            var connectionString = configSection.GetSection("PrimaryConnectionString").Value;
            var databaseName = configSection.GetSection("DatabaseName").Value;
            var containerName = configSection.GetSection("ContainerName").Value;
            var partitionKey = configSection.GetSection("PartitionKey").Value;
            var rebuildContainer = (bool)configSection.GetValue(typeof(Boolean), "RebuildContainer", false);

            var clientBuilder = new CosmosClientBuilder(connectionString);
            CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();

            // Create database
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            // Recreate container
            if (rebuildContainer)
            {
                try
                {
                    var cn = client.GetContainer(databaseName, containerName);
                    await cn.DeleteContainerAsync();
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // NOP
                }
            }

            // Define the unique key as ImageId + VehicleId
            await database.Database.DefineContainer(name: containerName, partitionKeyPath: partitionKey)
                .WithUniqueKey()
                    .Path("/ImageId")
                    .Path("/VehicleId")
                .Attach()
                .CreateIfNotExistsAsync();

            var container = client.GetContainer(databaseName, containerName);

            return new VehicleImageService(container);
        }

        public VehicleImageService(Container container) 
            : base(container)
        {
            Query = $"SELECT TOP {MaxItemCount} * from c ";
        }

        public IQueryable<VehicleImage> GetJobIterator() => _container
            .GetItemLinqQueryable<VehicleImage>(requestOptions: new QueryRequestOptions { MaxItemCount = this.MaxItemCount });

        public async Task<IEnumerable<VehicleImage>> FindByVehicleId(int? vehicleId)
        {
            return await GetEntitiesAsync($"c.VehicleId = {vehicleId}");
        }
    }
}
