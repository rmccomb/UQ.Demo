using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UQ.Demo.Models
{
    /// <summary>
    /// Represents a row in the table of Vehicle Image records
    /// </summary>
    public class VehicleImage : Entity
    {
        [DisplayName("Image ID")]
        public int ImageId { get; set; } // Primary Key
        
        [DisplayName("Vehicle ID")]
        public int VehicleId { get; set; } // Primary Key and Partition Key

        public double Time { get; set; }
        
        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Speed { get; set; }
        public decimal Acceleration { get; set; }
        
        [DisplayName("X Coordinate (image)")]
        public double X { get; set; }

        [DisplayName("Y Coordinate (image)")]
        public double Y { get; set; }
    }
}
