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

        public void DeleteUser(IUIService uiService)
        {
            if (CurrentUser.Role != Role.Administrator) throw new Exception("You are not authorized for this action.");
            List<User> filteredUsers = FilterUsers(CurrentUser.Id);
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Delete a user :");
                    uiService.PrintEntites(filteredUsers);
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

        public void UnassignDriver(IDriverService driverService, IUIService uiService)
        {
            if (CurrentUser.Role != Role.Manager) throw new Exception("You are not authorized for this action.");
            while (true)
            {
                try
                {
                    Console.Clear();
                    List<Driver> assignedDrivers = driverService.GetAssignedDrivers();
                    uiService.PrintEntites(assignedDrivers);

                    string input = ConsoleHelper.GetInput("Enter index to assign driver or 'q' to exit : ");
                    if (input.ToLower() == "q")
                    {
                        break;
                    }
                    int index = ConsoleHelper.GetNumberInput(input, assignedDrivers.Count);
                    Driver unassinedDriver = assignedDrivers[index - 1];
                    unassinedDriver.Unassign();
                    driverService.Update(unassinedDriver);
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

        public void AssignDriver(IDriverService driverService, ICarService carService, IUIService uiService)
        {
            if (CurrentUser.Role != Role.Manager) throw new Exception("You are not authorized for this action.");
            while (true)
            {
                try
                {
                    Console.Clear();
                    List<Driver> unassingedDrivers = driverService.GetUnassignedDrivers();
                    uiService.PrintEntites(unassingedDrivers);
                    string input = ConsoleHelper.GetInput("Enter index to assign driver or 'q' to exit : ");
                    if (input.ToLower() == "q")
                    {
                        break;
                    }
                    int index = ConsoleHelper.GetNumberInput(input, unassingedDrivers.Count);
                    Driver assignDriver = unassingedDrivers[index - 1];
                    Shift assignShift = EnumHelper.SelectEnum<Shift>("shift");
                    Car assignCar = carService.SelectCarToAssign(assignShift, uiService);
                    ConsoleHelper.TextColor($"Successfully assigned {assignCar.Model} to {assignDriver.FullName}", ConsoleColor.Green);
                    Console.ReadLine();
                    assignDriver.AssignCar(assignCar);
                    assignDriver.AssignShift(assignShift);
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
    }
}
