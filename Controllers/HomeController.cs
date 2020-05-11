﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UQ.Demo.Models;
using UQ.Demo.Services;

namespace UQ.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleImageService _vehicleImageService;

        public HomeController(IVehicleImageService vehicleImageService)
        {
            _vehicleImageService = vehicleImageService;
        }

        public async Task<IActionResult> Index()
        {
            var images = await _vehicleImageService.GetEntitiesAsync(null);
            return View(images);
        }

        /// <summary>
        /// Display the API help
        /// </summary>
        public ActionResult ApiHelp()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
