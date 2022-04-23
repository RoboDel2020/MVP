using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface IUser
    {
        public bool UserExists(string username);
        //public bool RegisterUser(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error);
        public bool ValidateUser(string usernameInput, out string error);
        public User ValidatePassword(string username, string password, out string error);
        //public User GetRegisteredInfoByEmail(string userEmail, out string error);
        //public bool UpdateUserInfoByEmail(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error);
    }
}
