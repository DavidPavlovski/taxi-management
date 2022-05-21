using System.Collections.Generic;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.Services.Interfaces
{
    public interface IDriverService : IBaseService<Driver>
    {
        void CheckLicenceStatus();
        List<Driver> GetUnassignedDrivers();
        List<Driver> GetAssignedDrivers();
        Car GetAssignedCar(Driver driver, ICarService carService);
    }
}
