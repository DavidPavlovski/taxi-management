using System;
using System.Linq;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public User Login(string username, string password)
        {
            User user = Db.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public bool ChangePassword(int id, string oldPassword, string newPassword)
        {
            User user = Db.GetById(id);
            if (user.Password == oldPassword)
            {
                user.Password = newPassword;
                return true;
            }
            return false;
        }
    }
}
