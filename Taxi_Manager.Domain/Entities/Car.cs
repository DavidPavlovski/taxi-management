using System;
using System.Collections.Generic;
using System.Text;

namespace Taxi_Manager.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public DateTime LicensePlateExpieryDate { get; set; }
        public List<Driver> AsignedDrivers { get; set; }

        public Car(string model, string licencePlate, DateTime licencePlateExpieryDate, List<Driver> asignedDrivers)

        {
            Model = model;
            LicensePlate = licencePlate;
            LicensePlateExpieryDate = licencePlateExpieryDate;
            AsignedDrivers = asignedDrivers;
        }

        public override string Print()
        {
            string drivers = string.Empty;
            foreach (Driver d in AsignedDrivers)
            {
                drivers += $"\n-{d.Print()}";
            }
            return $"Car {Model} with {LicensePlate} with expiery date on : {LicensePlateExpieryDate:dd/MM(MMM)/yyyy} driven by : {drivers}";
        }
    }
}
