using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class DriverService : BaseService<Driver>, IDriverService
    {
        public void CheckLicenceStatus()
        {
            List<Driver> drivers = Db.GetAll().OrderByDescending(x => x.LicenceExpiery).ToList();
            foreach (Driver driver in drivers)
            {
                LicenceStatus licenceStatus = DateTimeHelper.GetLicenceStatus(driver.LicenceExpiery);
                switch (licenceStatus)
                {
                    case LicenceStatus.Expiered:
                        ConsoleHelper.TextColor($"Driver {driver.FullName} with license [{driver.Licence}] expiered on {driver.LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Red);
                        break;
                    case LicenceStatus.NearExpiration:
                        ConsoleHelper.TextColor($"Driver {driver.FullName} with license [{driver.Licence}] expiering on {driver.LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Yellow);
                        break;
                    case LicenceStatus.Valid:
                        ConsoleHelper.TextColor($"Driver {driver.FullName} with license [{driver.Licence}] expiering on {driver.LicenceExpiery:dd/MM(MMM)/yyy}", ConsoleColor.Green);
                        break;
                }
            }
        }

        public List<Driver> GetAssignedDrivers()
        {
            return Db.GetAll().Where(d => d.IsAssigned()).ToList();
        }

        public List<Driver> GetUnassignedDrivers()
        {
            return Db.GetAll().Where(x => x.HasValidLicence() && !x.IsAssigned()).ToList();
        }

        public Car GetAssignedCar(Driver driver, ICarService carService)
        {
            return carService.GetById(driver.AssignedCarID);
        }

    }
}
