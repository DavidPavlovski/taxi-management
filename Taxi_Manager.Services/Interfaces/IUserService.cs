using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.Services.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        User CurrentUser { get; }
        void Login();
        void Logout();
        void CreateNewUser();
        void DeleteUser(IUIService uiService);
        void ChangePassword();
        void UnassignDriver(IDriverService driverService, IUIService uiService, ICarService carService);
        void AssignDriver(IDriverService driverService, ICarService carService, IUIService uiService);
        List<User> FilterUsers(int id);
        void PrintAllDrivers(IDriverService driverService, ICarService carService);
        void PrintAllCars(IUIService uiService, ICarService carService);
        void PrintAllUsers(IUIService uiService);
    }
}
