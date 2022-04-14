using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Enums;

namespace Taxi_Manager.Domain.Entities
{
    public class Driver : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Shift Shift { get; set; }
        public Car Car { get; set; }
        public string Licence { get; set; }
        public DateTime LicenceExpiery { get; set; }
        public Driver(string firstName, string lastName, Shift shift, Car car, string licence, DateTime licenceExpiery)

        {
            FirstName = firstName;
            LastName = lastName;
            Shift = shift;
            Car = car;
            Licence = licence;
            LicenceExpiery = licenceExpiery;
        }

        public override string Print()
        {
            return $"Driver : {FullName} , licence number : {Licence} - expiery date : {LicenceExpiery:dd/MM(MMM)/yyyy}";
        }
    }
}
