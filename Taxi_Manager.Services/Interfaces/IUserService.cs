using System.Collections.Generic;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.Services.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        User CurrentUser { get; }
        void Login();
        void Logout();
        void CreateNewUser();
        void DeleteUser();
        void ChangePassword();
        void UnassignDriver(IDriverService driverService,ICarService carService);
        void AssignDriver(IDriverService driverService, ICarService carService);
        List<User> FilterUsers(int id);
        void PrintAllDrivers(IDriverService driverService, ICarService carService);
        void PrintAllCars(ICarService carService);
        void PrintAllUsers();
    }
}
