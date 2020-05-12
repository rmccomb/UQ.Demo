using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using UQ.Demo.Services;

namespace UQ.Demo.Controllers
{
    public class ApiController : Controller
    {
        private readonly IVehicleImageService _vehicleImageService;

        public ApiController(IVehicleImageService vehicleImageService)
        {
            _vehicleImageService = vehicleImageService;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var images = await _vehicleImageService.GetEntitiesAsync(null);
                return Json(images);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [ActionName("Search")]
        public async Task<IActionResult> SearchAsync(int id)
        {
            try
            {
                var images = await _vehicleImageService.FindByVehicleId(id);
                return Json(images);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Images([FromBody] object json)
        {
            Debug.WriteLine(json);
            var images = JsonSerializer.Deserialize<Models.VehicleImage[]>(
                json.ToString(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            try
            {
                var added = 0;
                var parsed = 0;
                foreach (var image in images)
                {
                    var existing = await _vehicleImageService.GetEntitiesAsync($"c.ImageId = {image.ImageId} AND c.VehicleId = {image.VehicleId}");
                    if (!existing.Any())
                    {
                        await _vehicleImageService.AddEntityAsync(image);
                        added++;
                    }
                    parsed++;
                }

                return Json(new { RecordsParsed = parsed, RecordsAdded = added });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}