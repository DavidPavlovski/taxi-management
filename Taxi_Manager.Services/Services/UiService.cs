using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Helpers;
using Taxi_Manager.Services.Enums;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class UIService : IUIService
    {
        private List<MenuOptions> _defaultOptions = new List<MenuOptions>() { MenuOptions.ChangePassword, MenuOptions.Logout };
        private List<MenuOptions> _menuOptions;
        public List<MenuOptions> Options
        {
            get => _menuOptions;
            set
            {
                if (_menuOptions != null)
                {
                    _menuOptions.Clear();
                }
                _menuOptions = value;
            }
        }

        public void PrintEntites<T>(List<T> entites, string extension = null) where T : BaseEntity
        {
            for (int i = 0; i < entites.Count; i++)
            {
                Console.WriteLine($"{i + 1}.) {entites[i].Print()}" + extension);
            }
        }

        public void UserMenu(Role role)
        {
            switch (role)
            {
                case Role.Administrator:
                    List<MenuOptions> AdminOptions = new List<MenuOptions>() { MenuOptions.PrintAllUsers, MenuOptions.CreateNewUser };
                    AdminOptions.AddRange(_defaultOptions);
                    Options = AdminOptions;
                    break;

                case Role.Maintenence:
                    List<MenuOptions> MaintenenceOptions = new List<MenuOptions>() { MenuOptions.PrintAllCars, MenuOptions.CheckCarLicenceExpiration, MenuOptions.DeleteUser };
                    MaintenenceOptions.AddRange(_defaultOptions);
                    Options = MaintenenceOptions;
                    break;

                case Role.Manager:
                    List<MenuOptions> ManagerOptions = new List<MenuOptions>() { MenuOptions.PrintAllDrivers, MenuOptions.PrintDriverLicenceStatus, MenuOptions.PrintUnassignedDrivers, MenuOptions.UnassignDriver };
                    ManagerOptions.AddRange(_defaultOptions);
                    Options = ManagerOptions;
                    break;

                default:
                    Options = null;
                    break;
            }
        }
        public MenuOptions GetUserChoice(Role role)
        {
            UserMenu(role);
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"{i + 1}.) {Options[i].ToString().FromPascalCase()}");
            }
            string menuChoice = ConsoleHelper.GetInput("Select Option : ");
            int menuIndex = ConsoleHelper.GetNumberInput(menuChoice, Options.Count);
            MenuOptions selected = Options[menuIndex - 1];
            return selected;
        }

    }
}
