using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;

namespace Taxi_Manager.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public DateTime LicensePlateExpieryDate { get; set; }
        public List<Driver> AssasignedDrivers { get; set; }

        public Car(string model, string licencePlate, DateTime licencePlateExpieryDate)
        {
            Model = model;
            LicensePlate = licencePlate;
            LicensePlateExpieryDate = licencePlateExpieryDate;
            AssasignedDrivers = new List<Driver>();
        }

        public Car(string model, string licencePlate, DateTime licencePlateExpieryDate, List<Driver> asignedDrivers)
        {
            Model = model;
            LicensePlate = licencePlate;
            LicensePlateExpieryDate = licencePlateExpieryDate;
            AssasignedDrivers = asignedDrivers;
        }

        public override string Print()
        {
            decimal utilized = ((decimal)AssasignedDrivers.Count / (decimal)Enum.GetNames(typeof(Shift)).Length) * 100;
            return $"Car :(ID-{Id}) {Model} , Licence plate : {LicensePlate} and utilized : {Math.Ceiling(utilized)}% ";
        }

        public bool IsAvailableForShif(Shift shift)
        {
            return AssasignedDrivers.Any(x => x.Shift != shift) || AssasignedDrivers.Count == 0;
        }

        public bool HasValidLicence()
        {
            return LicensePlateExpieryDate > DateTime.Now;
        }
        public void CheckLicencePlateExpiration()
        {
            if (LicensePlateExpieryDate < DateTime.Now)
            {
                ConsoleHelper.TextColor($" Car Id [Id] - Plate [{LicensePlate}] expiered on {LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Red);
            }
            else if (LicensePlateExpieryDate <= DateTime.Now.AddMonths(3))
            {
                ConsoleHelper.TextColor($" Car Id [Id] - Plate [{LicensePlate}] expiering on {LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Yellow);
            }
            else
            {
                ConsoleHelper.TextColor($" Car Id [Id] - Plate [{LicensePlate}] expiering on {LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Green);
            }
        }
        public void UnassignDriver(Driver driver)
        {
            AssasignedDrivers.Remove(driver);
        }
    }
}
