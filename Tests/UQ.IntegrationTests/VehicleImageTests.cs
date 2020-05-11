using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UQ.Demo.Models;
using UQ.Demo.Services;
using Xunit;

namespace UQ.IntegrationTests
{
    public class VehicleImageTests : IClassFixture<ServicesFixture>
    {
        ServicesFixture _services;
        IVehicleImageService _vehicleImageService;

        public VehicleImageTests(ServicesFixture servicesFixture)
        {
            _services = servicesFixture;
            _vehicleImageService = _services.ServiceProvider
                .GetRequiredService<IVehicleImageService>();
        }

        [Fact]
        public async Task AddVehicleImage()
        {
            var image = new VehicleImage
            {
                id = Guid.NewGuid().ToString(),
                VehicleId = 1,
                Acceleration = 0.5m,
                ImageId = 100,
                Latitude = 0.123m,
                Longitude = 0.456m,
                Speed = 59.5m,
                Time = 0.1,
                VehicleType = "Car",
                X = 532.0123,
                Y = 548.1238
            };

            await _vehicleImageService.AddEntityAsync(image);
        }

        [Fact]
        public async Task BulkAddVehicleImages()
        {
            var max = 100;
            var imageId = 1;
            while (imageId < max)
            {
                var image = GetRandomImage(imageId);
                await _vehicleImageService.AddEntityAsync(image);
                imageId++;
            }
        }

        private VehicleImage GetRandomImage(int imageId)
        {
            var r = new Random(DateTime.Now.Millisecond);

            return new VehicleImage
            {
                // Unique key constraint on ImageId + VehicleId
                ImageId = 35000 + imageId,
                VehicleId = 13403,

                Acceleration = new decimal(r.NextDouble() * 10),
                id = Guid.NewGuid().ToString(),
                Latitude = -27.5599m,
                Longitude = 153.0813m,
                Speed = new decimal(r.NextDouble() * 10),
                Time = imageId / 10.0,
                VehicleType = "Car",
                X = Math.Round(r.NextDouble() * 500, 4),
                Y = Math.Round(r.NextDouble() * 500, 4)
            };
        }
    }
}