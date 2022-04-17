using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Services;

namespace Taxi_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserService userService = new UserService();
            CarService carService = new CarService();
            DriverService driverService = new DriverService();

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

            userService.Add(admin1);
            userService.Add(admin2);
            userService.Add(manager1);
            userService.Add(manager2);
            userService.Add(maintenece1);
            userService.Add(maintenece2);

            driverService.Add(driver1);
            driverService.Add(driver2);
            driverService.Add(driver3);
            driverService.Add(driver4);
            driverService.Add(driver5);
            driverService.Add(driver6);
            driverService.Add(driver7);
            driverService.Add(driver8);
            driverService.Add(driver9);

            //driver1.Shift = Shift.Morning;
            //driver2.Shift = Shift.Afternoon;
            //driver3.Shift = Shift.Night;
            //driver1.AssignCar(car1);
            //driver2.AssignCar(car1);
            //driver3.AssignCar(car1);

            //driver4.Shift = Shift.Morning;
            //driver5.Shift = Shift.Night;
            //driver4.AssignCar(car2);
            //driver5.AssignCar(car2);

            //driver6.Shift = Shift.Morning;
            //driver7.Shift = Shift.Afternoon;
            //driver8.Shift = Shift.Night;
            //driver6.AssignCar(car3);
            //driver7.AssignCar(car3);

            //driver8.AssignCar(car4);


            carService.Add(car1);
            carService.Add(car2);
            carService.Add(car3);
            carService.Add(car4);
            carService.Add(car5);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login.");
                User user = HandleLogin(userService);
                if (user == null)
                {
                    ConsoleHelper.TextColor("username or password incorrect", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                switch (user.Role)
                {
                    case Role.Administrator:
                        AdminOptions(user, userService);
                        break;
                    case Role.Maintenence:
                        MaintenenceOptions(user, carService, userService);
                        break;
                    case Role.Manager:
                        ManagerOptions(user, driverService, userService, carService);
                        break;
                }
            }
        }

        public static User HandleLogin(UserService userService)
        {
            Console.Write("Enter your username : ");
            string username = Console.ReadLine();
            Console.Write("Enter your password : ");
            string password = Console.ReadLine();
            return userService.Login(username, password);
        }
        public static Role SelectRole()
        {
            Console.WriteLine("Select role : ");

            for (int i = 0; i < Enum.GetValues(typeof(Role)).Length; i++)
            {
                Console.WriteLine($"{i + 1}.{(Role)i + 1}");
            }
            while (true)
            {
                string roleChoice = Console.ReadLine();
                if (!int.TryParse(roleChoice, out int num) || num < 0 || num > Enum.GetValues(typeof(Role)).Length)
                {
                    ConsoleHelper.TextColor("Invalid input role choice please try again.", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                return (Role)num;
            }
        }

        public static User CreateNewUser(UserService userService)
        {
            Console.WriteLine("Create a new user");
            Console.Write("Enter username : ");
            string username = Console.ReadLine();
            if (ValidateUsername(username, userService))
            {
                throw new Exception($"User with the {username} username already exists.");
            }
            Console.Write("Enter password : ");
            string password = Console.ReadLine();
            Role role = SelectRole();
            User newUser = new User(username, password, role);
            return newUser;
        }
        public static void DeleteUser(User admin, UserService userService)
        {
            List<User> filteredUsers = userService.GetAll().Where(u => u.Id != admin.Id).ToList();
            Console.WriteLine("Delete a user :");
            for (int i = 0; i < filteredUsers.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{filteredUsers[i].Print()}");
            }
            Console.Write("Enter user index to delete : ");
            while (true)
            {
                string index = Console.ReadLine();
                if (!int.TryParse(index, out int num) || num < 0 || num > filteredUsers.Count)
                {
                    ConsoleHelper.TextColor("Invalid input please try again.", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                User userToDelete = filteredUsers[num - 1];
                userService.Remove(userToDelete.Id);
                ConsoleHelper.TextColor($"Successfully deleted : {userToDelete.Print()}", ConsoleColor.Green);
                break;
            }
        }
        public static bool ValidateUsername(string username, UserService userService)
        {
            return userService.GetAll().Any(u => u.Username == username);
        }
        public static bool HandleChangePassword(UserService users, User user)
        {
            Console.Clear();
            Console.WriteLine("Change password.");
            Console.Write("Enter old password : ");
            string oldPassword = Console.ReadLine();
            Console.Write("Enter new password : ");
            string newPassword = Console.ReadLine();
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new Exception("Password cannot be empty.");
            }
            else if (newPassword.Length < 5)
            {
                throw new Exception("Password must contain at least 5 charactrers.");
            }
            else if (!newPassword.Any(char.IsDigit))
            {
                throw new Exception("Password must contain at least 1 digit.");
            }
            return users.ChangePassword(user.Id, oldPassword, newPassword);
        }
        public static void HandleUnassignDriver(DriverService driverService)
        {
            while (true)
            {
                Console.Clear();
                List<Driver> assignedDrivers = driverService.GetAll().Where(d => d.Shift != null && d.Car != null).ToList();
                for (int i = 0; i < assignedDrivers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{assignedDrivers[i].Print()}");
                }
                Console.Write("Enter index to assign driver or 'q' to exit : ");
                string input = Console.ReadLine();
                if (input.ToLower() == "q")
                {
                    break;
                }
                if (!int.TryParse(input, out int index))
                {
                    ConsoleHelper.TextColor("Input must be a number", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                else if (index <= 0 || index > assignedDrivers.Count)
                {
                    ConsoleHelper.TextColor($"Number must be between 1 and {assignedDrivers.Count}", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                try
                {
                    Driver unassinedDriver = assignedDrivers[index - 1];
                    unassinedDriver.Unassign();
                    ConsoleHelper.TextColor($"Successfully unassigned {unassinedDriver.FullName}", ConsoleColor.Green);
                    Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.TextColor("Something went wrong", ConsoleColor.Red);
                    Console.ReadLine();
                    break;
                }
            }
        }
        public static Shift SelectShift()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select shift : ");
                for (int i = 0; i < Enum.GetValues(typeof(Shift)).Length; i++)
                {
                    Console.WriteLine($"{i + 1}.{(Shift)i + 1}");
                }
                string roleChoice = Console.ReadLine();
                if (!int.TryParse(roleChoice, out int num) || num < 0 || num > Enum.GetValues(typeof(Shift)).Length)
                {
                    ConsoleHelper.TextColor("Invalid input role choice please try again.", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                return (Shift)num;
            }
        }
        public static Car SelectCarToAssign(CarService carService, Shift selectedShift)
        {
            List<Car> filteredCars = carService.GetAll().Where(c => c.AssasignedDrivers.All(d => d.Shift != selectedShift) && c.HasValidLicence()).ToList();

            for (int i = 0; i < filteredCars.Count; i++)
            {
                Console.WriteLine($"{i + 1}.){filteredCars[i].Model}");
            }
            while (true)
            {
                Console.Write("Enter car index : ");
                if (!int.TryParse(Console.ReadLine(), out int index))
                {
                    ConsoleHelper.TextColor("Input must be a number", ConsoleColor.Red);
                    continue;
                }
                else if (index <= 0 || index > filteredCars.Count)
                {
                    ConsoleHelper.TextColor($"Number must be between 0 and {filteredCars.Count}", ConsoleColor.Red);
                    continue;
                }
                return filteredCars[index - 1];
            }
        }

        public static void HandleAssignDriver(DriverService driverService, CarService carService)
        {
            while (true)
            {
                Console.Clear();
                List<Driver> unassingedDrivers = driverService.GetAll().Where(d => d.IsAssigned() && d.HasValidLicence()).ToList();
                for (int i = 0; i < unassingedDrivers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{unassingedDrivers[i].Print()}");
                }
                Console.WriteLine("Enter index to assign driver or 'q' to exit");
                string input = Console.ReadLine();
                if (input.ToLower() == "q")
                {
                    break;
                }
                if (!int.TryParse(input, out int index))
                {
                    ConsoleHelper.TextColor("Input must be a number", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                else if (index <= 0 || index > unassingedDrivers.Count)
                {
                    ConsoleHelper.TextColor($"Number must be between 1 and {unassingedDrivers.Count}", ConsoleColor.Red);
                    Console.ReadLine();
                    continue;
                }
                Driver assignDriver = unassingedDrivers[index - 1];
                Shift assignShift = SelectShift();
                Car assignCar = SelectCarToAssign(carService, assignShift);
                ConsoleHelper.TextColor($"Successfully assigned {assignCar.Model} to {assignDriver.FullName}", ConsoleColor.Green);
                assignDriver.AssignCar(assignCar);
                assignDriver.AssignShift(assignShift);
                Console.ReadLine();
                break;
            }
        }
        public static void ManagerOptions(User manager, DriverService driverService, UserService userService, CarService carService)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.TextColor($"Successful Login! Welcome {manager.Print()}.", ConsoleColor.Green);
                Console.WriteLine("Select option : \n1.)Print all drivers \n2.)Print license status \n3.)Print unassigned drivers \n4.)Unassign driver \n5.)Change password \n6.)Logout");
                string option = Console.ReadLine();
                if (option == "6")
                {
                    ConsoleHelper.TextColor("Goobye.", ConsoleColor.Yellow);
                    Console.ReadLine();
                    break;
                }
                switch (option)
                {
                    case "1":
                        Console.Clear();
                        driverService.GetAll().ForEach(d => Console.WriteLine(d.Print()));
                        Console.ReadLine();
                        break;
                    case "2":
                        Console.Clear();
                        driverService.GetAll().ForEach(d => d.CheckDriversLicenceExpiration());
                        Console.ReadLine();
                        break;
                    case "3":
                        HandleAssignDriver(driverService, carService);
                        break;
                    case "4":
                        HandleUnassignDriver(driverService);
                        break;
                    case "5":
                        Console.Clear();
                        try
                        {
                            bool success = HandleChangePassword(userService, manager);
                            if (success)
                            {
                                ConsoleHelper.TextColor("Password changed successfully.", ConsoleColor.Green);
                                Console.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                            Console.ReadLine();
                        }
                        break;
                    default:
                        Console.Clear();
                        ConsoleHelper.TextColor("Invalid input", ConsoleColor.Red);
                        Console.ReadLine();
                        break;
                }
            }

        }
        public static void MaintenenceOptions(User maintenece, CarService carService, UserService userService)
        {
            while (true)
            {

                Console.Clear();
                ConsoleHelper.TextColor($"Successful Login! Welcome {maintenece.Print()}.", ConsoleColor.Green);
                Console.WriteLine("Select option : \n1.)Print all cars \n2.)Check Licence expiration \n3.)Change password \n4.)Logout");
                string option = Console.ReadLine();
                if (option == "4")
                {
                    Console.Clear();
                    ConsoleHelper.TextColor("Goodbye.", ConsoleColor.Yellow);
                    Console.ReadLine();
                    break;
                }
                switch (option)
                {
                    case "1":
                        carService.GetAll().ForEach(c => Console.WriteLine(c.Print()));
                        Console.ReadLine();
                        break;
                    case "2":
                        carService.GetAll().ForEach(c => c.CheckLicencePlateExpiration());
                        Console.ReadLine();
                        break;
                    case "3":
                        try
                        {
                            bool success = HandleChangePassword(userService, maintenece);
                            if (success)
                            {
                                ConsoleHelper.TextColor("Password changed successfully.", ConsoleColor.Green);
                                Console.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                            Console.ReadLine();
                        }
                        break;
                    default:
                        Console.Clear();
                        ConsoleHelper.TextColor("Invalid input", ConsoleColor.Red);
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void AdminOptions(User admin, UserService userService)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.TextColor($"Successful Login! Welcome {admin.Print()}.", ConsoleColor.Green);
                Console.WriteLine("Select option : \n1.)Print all Users \n2.)Create new user \n3.)Delete User\n4.)Change password\n5.)Logout");
                string option = Console.ReadLine();
                if (option == "5")
                {
                    Console.Clear();
                    ConsoleHelper.TextColor("Goodbye.", ConsoleColor.Yellow);
                    Console.ReadLine();
                    break;
                }
                switch (option)
                {
                    case "1":
                        userService.GetAll().ForEach(u => Console.WriteLine(u.Print()));
                        Console.ReadLine();
                        break;
                    case "2":
                        try
                        {
                            User newUser = CreateNewUser(userService);
                            userService.Add(newUser);
                            ConsoleHelper.TextColor($"Successfully added {newUser.Print()}", ConsoleColor.Green);
                            Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                            Console.ReadLine();
                        }
                        break;
                    case "3":
                        DeleteUser(admin, userService);
                        Console.ReadLine();
                        break;
                    case "4":
                        try
                        {
                            bool success = HandleChangePassword(userService, admin);
                            if (success)
                            {
                                ConsoleHelper.TextColor("Password changed successfully.", ConsoleColor.Green);
                                Console.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            ConsoleHelper.TextColor(ex.Message, ConsoleColor.Red);
                            Console.ReadLine();
                        }
                        break;
                    default:
                        ConsoleHelper.TextColor("Invalid input", ConsoleColor.Red);
                        break;
                }
            }
        }
    }
}
