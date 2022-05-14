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
        public int AssignedCarID { get; set; }
        public string Licence { get; set; }
        public DateTime LicenceExpiery { get; set; }

        public Driver(string firstName, string lastName, string licence, DateTime licenceExpiery)
        {
            FirstName = firstName;
            LastName = lastName;
            Licence = licence;
            LicenceExpiery = licenceExpiery;
            Shift = null;
            AssignedCarID = -1;
        }

        public override string Print()
        {
            return $"(ID-{Id}) {FullName}";
        }

        public void Assign(Car car, Shift shift)
        {
            AssignedCarID = car.Id;
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
            return Shift != null && AssignedCarID != -1;
        }
        public void Unassign()
        {
            AssignedCarID = -1;
            Shift = null;
        }
    }
}
