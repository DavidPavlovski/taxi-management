using System.Linq;
using System.Text.RegularExpressions;
using Taxi_Manager.Helpers;

namespace System
{
    public static class StringHelper
    {
        public static string FromPascalCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static string ValidatePassword()
        {
            string newPassword = ConsoleHelper.GetInput("Enter new password : ");
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
            return newPassword;
        }
    }
}
