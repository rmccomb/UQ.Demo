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
            bool rebuildContainer = (bool)configSection.GetValue(typeof(Boolean), "RebuildContainer", false);

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
            Query = $"SELECT TOP {MaxItemCount} * from c";
        }

        public IQueryable<VehicleImage> GetJobIterator() => _container
            .GetItemLinqQueryable<VehicleImage>(requestOptions: new QueryRequestOptions { MaxItemCount = this.MaxItemCount });


    }
}
