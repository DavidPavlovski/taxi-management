using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;

namespace Taxi_Manager.Services.Interfaces
{
    public interface ICarService : IBaseService<Car>
    {
        void CheckLicenceStatus();
        Car SelectCarToAssign(Shift selectedShift, IUIService uiService , IDriverService driverService);
    }
}
