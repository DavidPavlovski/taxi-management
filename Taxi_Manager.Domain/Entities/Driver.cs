using System;
using Taxi_Manager.Domain.Enums;

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
