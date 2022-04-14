using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.Services.Interfaces
{
    internal interface IUserService
    {
        User Login(string username, string password);
        bool ChangePassword(int id, string oldPassword, string newPassword);
    }
}
