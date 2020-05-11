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
        public void Test1()
        {
            Assert.True(true);
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
                X = 532.0123m,
                Y = 548.1238m
            };

            await _vehicleImageService.AddEntityAsync(image);
        }
    }
}