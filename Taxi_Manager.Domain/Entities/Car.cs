using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;

namespace Taxi_Manager.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public DateTime LicensePlateExpieryDate { get; set; }
        public List<int> AssignedDriversIDs { get; set; }
        public Car(string model, string licencePlate, DateTime licencePlateExpieryDate)
        {
            Model = model;
            LicensePlate = licencePlate;
            LicensePlateExpieryDate = licencePlateExpieryDate;
            AssignedDriversIDs = new List<int>();
        }

        public override string Print()
        {
            decimal utilized = ((decimal)AssignedDriversIDs.Count / 3) * 100;
            return $"Car :(ID-{Id}) {Model} , Licence plate : {LicensePlate} and utilized : {Math.Ceiling(utilized)}% ";
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

        public void AssignDriver(int driverID)
        {
            AssignedDriversIDs.Add(driverID);
        }

        public void UnassignDriver(int driverID)
        {
            AssignedDriversIDs.Remove(driverID);
        }
    }
}
