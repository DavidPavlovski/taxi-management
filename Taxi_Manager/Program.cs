using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Services.Services;

namespace Taxi_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserService userService = new UserService();
            User admin1 = new User("admin1", "admin1", Role.Administrator);
            User admin2 = new User("admin2", "admin2", Role.Administrator);

            User manager1 = new User("manager1", "manager1", Role.Manager);
            User manager2 = new User("manage2", "manager2", Role.Manager);

            User maintenece1 = new User("maintenece1", "maintenece1", Role.Maintenence);
            User maintenece2 = new User("maintenece2", "maintenece2", Role.Maintenence);

            userService.Add(admin1);
            userService.Add(admin2);
            userService.Add(manager1);
            userService.Add(manager2);
            userService.Add(maintenece1);
            userService.Add(maintenece2);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login.");
                User user = HandleLogin(userService);
                if (user == null)
                {
                    Console.WriteLine("username or password incorrect");
                    Console.ReadLine();
                    continue;
                }
                switch (user.Role)
                {
                    case Role.Administrator:
                        AdminOptions(user, userService);
                        break;
                    case Role.Maintenence:
                        Console.WriteLine("Maintenence");
                        Console.ReadLine();
                        break;
                    case Role.Manager:
                        Console.WriteLine("Manager");
                        Console.ReadLine();
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
                    Console.WriteLine("Invalid input role choice please try again.");
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
            Console.WriteLine($"Successfully added : {newUser.Print()}");
            Console.ReadLine();
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
                    Console.WriteLine("Invalid input please try again.");
                    Console.ReadLine();
                    continue;
                }
                User userToDelete = filteredUsers[num - 1];
                userService.Remove(userToDelete.Id);
                Console.WriteLine($"Successfully deleted : {userToDelete.Print()}");
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

        public static void AdminOptions(User admin, UserService userService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome {admin.Username}");
                Console.WriteLine("Select option : \n1.)Print all Users \n2.)Create new user \n3.)Delete User\n4.)Change password\n5.)Logout");
                string option = Console.ReadLine();
                if (option == "5")
                {
                    Console.Clear();
                    Console.WriteLine("Goodbye.");
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
                            Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
                                Console.WriteLine("Password changed successfully.");
                                Console.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }
    }
}
