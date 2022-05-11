using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class DriverService : BaseService<Driver>, IDriverService
    {
        public void CheckLicenceStatus()
        {
            Db.GetAll().ForEach(x => x.CheckDriversLicenceExpiration());
        }

        public List<Driver> GetAssignedDrivers()
        {
            return Db.GetAll().Where(d => d.IsAssigned()).ToList();
        }

        public List<Driver> GetUnassignedDrivers()
        {
            return Db.GetAll().Where(x => x.HasValidLicence() && !x.IsAssigned()).ToList();
        }
    }
}
