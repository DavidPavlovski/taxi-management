using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class CarService : BaseService<Car>, ICarService
    {
        public List<Car> GetAvailableCarsForShift(Shift shift, IDriverService driverService)
        {
            return Db.GetAll().Where(x => x.HasValidLicence()).ToList().Where(y => y.AssignedDriversIDs.Any(id => driverService.GetById(id).Shift != shift) || y.AssignedDriversIDs.Count == 0).ToList();
        }

        public void CheckLicenceStatus()
        {
            List<Car> cars = Db.GetAll().OrderByDescending(x => x.LicensePlateExpieryDate).ToList();
            foreach (Car car in cars)
            {
                LicenceStatus licenceStatus = DateTimeHelper.GetLicenceStatus(car.LicensePlateExpieryDate);
                switch (licenceStatus)
                {
                    case LicenceStatus.Expiered:
                        ConsoleHelper.TextColor($" Car Id [Id] - Plate [{car.LicensePlate}] expiered on {car.LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Red);
                        break;
                    case LicenceStatus.NearExpiration:
                        ConsoleHelper.TextColor($" Car Id [Id] - Plate [{car.LicensePlate}] expiering on {car.LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Yellow);
                        break;
                    case LicenceStatus.Valid:
                        ConsoleHelper.TextColor($" Car Id [Id] - Plate [{car.LicensePlate}] expiering on {car.LicensePlateExpieryDate:dd/MM(MMM)/yyy}", ConsoleColor.Green);
                        break;
                }
            }
        }

        public Car SelectCarToAssign(Shift selectedShift, IDriverService driverService)
        {
            List<Car> filteredCars = GetAvailableCarsForShift(selectedShift, driverService);
            if (filteredCars.Count == 0)
            {
                throw new Exception($"No cars available for {selectedShift} shift.");
            }
            while (true)
            {
                try
                {
                    Console.Clear();
                    filteredCars.PrintEntities();
                    int carIndex = ConsoleHelper.GetNumberInput(ConsoleHelper.GetInput("Enter car index : "), filteredCars.Count);
                    return filteredCars[carIndex - 1];
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                }
            }
        }
    }
}
