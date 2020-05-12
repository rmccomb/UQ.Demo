using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UQ.Demo.Services;

namespace UQ.IntegrationTests
{
    public class ServicesFixture : IDisposable
    {
        public IConfiguration Configuration { get; set; }
        public IServiceCollection Services { get; set; }

        public ServiceProvider ServiceProvider { get; set; }

        public ServicesFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Azure.json")
                .Build();

            this.Services = new ServiceCollection();

            // Register the services       
            this.Services.AddSingleton(VehicleImageService.InitializeCosmosClientInstanceAsync(
                Configuration.GetSection("CosmosDb:VehicleImages")).GetAwaiter().GetResult());

            // Build the Service Provider
            this.ServiceProvider = this.Services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // TODO any cleanup
        }
    }
}
