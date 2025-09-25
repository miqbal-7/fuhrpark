using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuhrpark.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string VehicleClass { get; set; }
        public int? Mileage { get; set; }
        public double? Ton { get; set; }
        public int? YearOfManufacture { get; set; }
        public string State { get; set; }
    }
}
