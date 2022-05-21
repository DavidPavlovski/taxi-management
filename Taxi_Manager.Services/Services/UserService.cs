using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{

    public class UserService : BaseService<User>, IUserService
    {
        public User CurrentUser { get; private set; }
        public void Login()
        {
            string username = ConsoleHelper.GetInput("Enter your username : ");
            string password = ConsoleHelper.GetInput("Enter your password : ");
            CurrentUser = Db.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (CurrentUser == null)
            {
                throw new Exception("Invalid username or password. Try again.");
            }
        }
        public void ChangePassword()
        {
            Console.WriteLine("Change password.");
            string oldPassword = ConsoleHelper.GetInput("Enter old password : ");
            string newPassword = StringHelper.ValidatePassword();
            if (oldPassword != CurrentUser.Password)
            {
                throw new Exception("Something went wrong");
            }
            CurrentUser.Password = newPassword;
            Db.Update(CurrentUser);
        }

        public bool UsernameExists(string username)
        {
            return Db.GetAll().Any(x => x.Username == username);
        }

        public List<User> FilterUsers(int id)
        {
            return Db.GetAll().Where(x => x.Id != id).ToList();
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public void CreateNewUser()
        {
            if (CurrentUser.Role != Role.Administrator) throw new Exception("You are not authorized for this action.");
            try
            {
                Console.WriteLine("Create a new user.");
                string username = ConsoleHelper.GetInput("Enter username : ");
                if (UsernameExists(username))
                {
                    throw new Exception($"User with the {username} username already exists.");
                }
                string password = ConsoleHelper.GetInput("Enter password : ");
                Role role = EnumHelper.SelectEnum<Role>("role");
                User newUser = new User(username, password, role);
                Db.Add(newUser);
                ConsoleHelper.TextColor($"Successfully added {newUser.Print()}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ConsoleHelper.TextColor("Something went wrong.", ConsoleColor.Red);
                Console.ReadLine();
            }
        }

        public void DeleteUser()
        {
            if (CurrentUser.Role != Role.Administrator) throw new Exception("You are not authorized for this action.");
            List<User> filteredUsers = FilterUsers(CurrentUser.Id);
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Delete a user :");
                    filteredUsers.PrintEntities();
                    string input = ConsoleHelper.GetInput("Enter user index to delete or \"q\" to exit: ");
                    if (input.ToLower() == "q")
                    {
                        break;
                    }
                    int index = ConsoleHelper.GetNumberInput(input, filteredUsers.Count);
                    User userToDelete = filteredUsers[index - 1];
                    bool success = Remove(userToDelete.Id);
                    if (success)
                    {
                        ConsoleHelper.TextColor($"Successfully deleted : {userToDelete.Print()}", ConsoleColor.Green);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
            }
        }

        public void UnassignDriver(IDriverService driverService, ICarService carService)
        {
            if (CurrentUser.Role != Role.Manager) throw new Exception("You are not authorized for this action.");
            while (true)
            {
                try
                {
                    Console.Clear();
                    List<Driver> assignedDrivers = driverService.GetAssignedDrivers();
                    assignedDrivers.PrintEntities();

                    string input = ConsoleHelper.GetInput("Enter index to assign driver or 'q' to exit : ");
                    if (input.ToLower() == "q")
                    {
                        break;
                    }
                    int driverIndex = ConsoleHelper.GetNumberInput(input, assignedDrivers.Count);
                    Driver unassinedDriver = assignedDrivers[driverIndex - 1];
                    Car unassignCar = carService.GetById(unassinedDriver.AssignedCarID);
                    unassinedDriver.Unassign();
                    unassignCar.UnassignDriver(unassinedDriver.Id);
                    driverService.Update(unassinedDriver);
                    carService.Update(unassignCar);
                    ConsoleHelper.TextColor($"Successfully unassigned {unassinedDriver.FullName}", ConsoleColor.Green);
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
            }
        }

        public void AssignDriver(IDriverService driverService, ICarService carService)
        {
            if (CurrentUser.Role != Role.Manager) throw new Exception("You are not authorized for this action.");
            while (true)
            {
                try
                {
                    Console.Clear();
                    List<Driver> unassingedDrivers = driverService.GetUnassignedDrivers();
                    unassingedDrivers.PrintEntities();
                    string input = ConsoleHelper.GetInput("Enter index to assign driver or 'q' to exit : ");
                    if (input.ToLower() == "q")
                    {
                        break;
                    }
                    int index = ConsoleHelper.GetNumberInput(input, unassingedDrivers.Count);
                    Driver assignDriver = unassingedDrivers[index - 1];
                    Shift assignShift = EnumHelper.SelectEnum<Shift>("shift");

                    Car assignCar = carService.SelectCarToAssign(assignShift,driverService);
                    ConsoleHelper.TextColor($"Successfully assigned {assignCar.Model} to {assignDriver.FullName}", ConsoleColor.Green);
                    assignDriver.Assign(assignCar, assignShift);
                    assignCar.AssignDriver(assignDriver.Id);
                    carService.Update(assignCar);
                    driverService.Update(assignDriver);
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
            }
        }

        public void PrintAllUsers()
        {
            if (CurrentUser.Role != Role.Administrator) throw new Exception("You are not authorized for this action.");
            GetAll().PrintEntities();
        }
        public void PrintAllCars(ICarService carService)
        {
            if (CurrentUser.Role != Role.Maintenence) throw new Exception("You are not authorized for this action.");
            carService.GetAll().PrintEntities();
        }
        public void PrintAllDrivers(IDriverService driverService, ICarService carService)
        {
            if (CurrentUser.Role != Role.Manager) throw new Exception("You are not authorized for this action.");
            List<Driver> drivers = driverService.GetAll();
            for (int i = 0; i < drivers.Count; i++)
            {
                Driver driver = drivers[i];
                if (driver.AssignedCarID == -1)
                {
                    Console.WriteLine($"{i + 1}.) {driver.Print()} - Unassigned");
                    continue;
                }
                Car assignedCar = driverService.GetAssignedCar(driver, carService);
                Console.WriteLine($"{i + 1}.){driver.Print()} driving in the {driver.Shift} shift with a {assignedCar.Model} car");
            }
        }

    }
}