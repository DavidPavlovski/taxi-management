using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;

namespace Taxi_Manager.Domain.Entities
{
    public class Driver : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Shift? Shift { get; set; }
        public Car Car { get; set; }
        public string Licence { get; set; }
        public DateTime LicenceExpiery { get; set; }

        public Driver() { }
        public Driver(string firstName, string lastName, string licence, DateTime licenceExpiery)
        {
            FirstName = firstName;
            LastName = lastName;
            Licence = licence;
            LicenceExpiery = licenceExpiery;
            Shift = null;
            Car = null;
        }


        public override string Print()
        {
            if (Car == null || Shift == null)
            {
                return $"(ID-{Id}) {FullName} - unassigned";
            }
            return $"(ID-{Id}) {FullName} driving in the {Shift} shift with a {Car.Model} car";
        }
        public void AssignCar(Car car)
        {
            if (car.AssignedDrivers.Any(d => d.Shift == this.Shift))
            {
                throw new Exception("Car already in use for that shift");
            }
            Car = car;
            car.AssignDriver(this);
        }
        public void AssignShift(Shift shift)
        {
            Shift = shift;
        }
        public void CheckDriversLicenceExpiration()
        {
            if (LicenceExpiery < DateTime.Now)
            {
                ConsoleHelper.TextColor($"Driver {FullName} with license [{Licence}] expiered on {LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Red);
            }
            else if (LicenceExpiery <= DateTime.Now.AddMonths(3))
            {
                ConsoleHelper.TextColor($"Driver {FullName} with license [{Licence}] expiering on {LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Yellow);
            }
            else
            {
                ConsoleHelper.TextColor($"Driver {FullName} with license [{Licence}] expiering on {LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Green);
            }
        }
        public bool HasValidLicence()
        {
            return LicenceExpiery > DateTime.Now;
        }
        public bool IsAssigned()
        {
            return Shift != null && Car != null;
        }
        public void Unassign()
        {
            Car.UnassignDriver(this);
            Car = null;
            Shift = null;
        }
    }
}
