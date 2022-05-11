using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Domain.Enums;
using Taxi_Manager.Services.Enums;

namespace Taxi_Manager.Services.Interfaces
{
    public interface IUIService
    {
        void PrintEntites<T>(List<T> entites) where T : BaseEntity;
        void UserMenu(Role role);
        MenuOptions GetUserChoice(Role role);
    }
}
