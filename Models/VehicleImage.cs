﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UQ.Demo.Models
{
    public class VehicleImage
    {
        [DisplayName("Image ID")]
        public int id { get; set; }
        
        [DisplayName("Vehicle ID")]
        public int VehicleId { get; set; }
        public float Time { get; set; }
        
        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Speed { get; set; }
        public decimal Acceleration { get; set; }
        
        [DisplayName("X Coordinate (image)")]
        public decimal X { get; set; }

        [DisplayName("Y Coordinate (image)")]
        public decimal Y { get; set; }
    }
}
