using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Services.Services;
using Taxi_Manager.Services.Interfaces;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Enums;

namespace Taxi_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IUserService userService = new UserService();
            ICarService carService = new CarService();
            IDriverService driverService = new DriverService();
            IUIService uiService = new UIService();

            User admin1 = new User("admin1", "admin1", Role.Administrator);
            User admin2 = new User("admin2", "admin2", Role.Administrator);
            User manager1 = new User("manager1", "manager1", Role.Manager);
            User manager2 = new User("manage2", "manager2", Role.Manager);
            User maintenece1 = new User("maintenence1", "maintenence1", Role.Maintenence);
            User maintenece2 = new User("maintenence2", "maintenence2", Role.Maintenence);

            Driver driver1 = new Driver("Bob", "Bobsky", "AAA111", new DateTime(2024, 4, 16));
            Driver driver2 = new Driver("Tom", "Tomsky", "AAA222", new DateTime(2022, 7, 16));
            Driver driver3 = new Driver("Tim", "Timsky", "AAA333", new DateTime(2022, 2, 14));
            Driver driver4 = new Driver("John", "Doe", "AAA444", new DateTime(2023, 5, 24));
            Driver driver5 = new Driver("John", "Smith", "AAA555", new DateTime(2021, 3, 18));
            Driver driver6 = new Driver("Mark", "Marksjy", "AAA666", new DateTime(2020, 5, 7));
            Driver driver7 = new Driver("Toni", "Tonsky", "AAA777", new DateTime(2022, 6, 14));
            Driver driver8 = new Driver("Pero", "Perovski", "AAA888", new DateTime(2022, 9, 21));
            Driver driver9 = new Driver("Kire", "Kirevski", "AAA999", new DateTime(2022, 12, 12));

            Car car1 = new Car("Renault - Megane", "sk-111-aa", new DateTime(2022, 7, 12));
            Car car2 = new Car("Mercedes - E class", "sk-222-bb", new DateTime(2021, 8, 8));
            Car car3 = new Car("Dacia - Duster", "sk-333-cc", new DateTime(2024, 5, 6));
            Car car4 = new Car("Opel - Insignia", "sk-444-dd", new DateTime(2023, 7, 8));
            Car car5 = new Car("Toyota - Corola", "sk-555-ff", new DateTime(2023, 7, 8));

            List<User> users = new List<User>() { admin1, admin2, manager1, manager2, maintenece1, maintenece2 };
            List<Driver> drivers = new List<Driver>() { driver1, driver2, driver3, driver4, driver5, driver6, driver7, driver8, driver9 };
            List<Car> cars = new List<Car>() { car1, car2, car3, car4, car5 };

            userService.Seed(users);
            driverService.Seed(drivers);
            carService.Seed(cars);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login.");

                if (userService.CurrentUser == null)
                {
                    try
                    {
                        userService.Login();
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                        Console.ReadLine();
                        continue;
                    }
                }

                while (userService.CurrentUser != null)
                {
                    try
                    {
                        Console.Clear();
                        ConsoleHelper.TextColor($"Successful Login! Welcome {userService.CurrentUser.Print()}.", ConsoleColor.Green);
                        MenuOptions selected = uiService.GetUserChoice(userService.CurrentUser.Role);

                        switch (selected)
                        {
                            //Admin options
                            case MenuOptions.PrintAllUsers:
                                Console.Clear();
                                Console.WriteLine("Printing all users.");
                                uiService.PrintEntites(userService.GetAll());
                                Console.ReadLine();
                                break;

                            case MenuOptions.CreateNewUser:
                                try
                                {
                                    Console.Clear();
                                    userService.CreateNewUser();
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.DeleteUser:
                                try
                                {
                                    Console.Clear();
                                    userService.DeleteUser(uiService);
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            //Maintenence options
                            case MenuOptions.PrintAllCars:
                                try
                                {

                                    Console.Clear();
                                    uiService.PrintEntites(carService.GetAll());
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.CheckCarLicenceExpiration:
                                try
                                {

                                    Console.Clear();
                                    carService.CheckLicenceStatus();
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            //Manager options

                            case MenuOptions.PrintAllDrivers:
                                try
                                {
                                    Console.Clear();
                                    uiService.PrintEntites(driverService.GetAll());
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.PrintDriverLicenceStatus:
                                try
                                {
                                    Console.Clear();
                                    driverService.CheckLicenceStatus();
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.PrintUnassignedDrivers:
                                try
                                {
                                    Console.Clear();
                                    userService.HandleAssignDriver(driverService, carService, uiService);
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.UnassignDriver:
                                try
                                {
                                    Console.Clear();
                                    userService.UnassignDriver(driverService, uiService);
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            //Default options
                            case MenuOptions.Logout:
                                try
                                {
                                    userService.Logout();
                                    Console.WriteLine("logged out");
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            case MenuOptions.ChangePassword:
                                try
                                {
                                    Console.Clear();
                                    userService.ChangePassword();
                                    ConsoleHelper.TextColor("Password changed successfully.", ConsoleColor.Green);
                                    Console.ReadLine();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                                    Console.ReadLine();
                                    break;
                                }

                            default:
                                Console.WriteLine("invalid option");
                                Console.ReadLine();
                                break;
                        }
                        continue;
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
}
