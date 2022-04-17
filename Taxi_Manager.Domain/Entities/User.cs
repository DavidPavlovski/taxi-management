using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taxi_Manager.Domain.Enums;

namespace Taxi_Manager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User(string username, string password, Role role)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username cannot be empty");
            }
            else if (username.Length < 5)
            {
                throw new ArgumentException("Username must contain at least 5 characters");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password cannot be empty");
            }
            else if (password.Length < 5)
            {
                throw new ArgumentException("Password must be contain 5 characters");
            }
            else if (password.Any(char.IsDigit) == false)
            {
                throw new ArgumentException("Password must contain at least 1 digit");
            }
            Username = username;
            Password = password;
            Role = role;
        }
        public override string Print()
        {
            return $"{Username} [{Role}]";
        }
    }
}
