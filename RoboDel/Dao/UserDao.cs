using RoboDel.Controllers;
using RoboDel.DaoInterface;
using RoboDel.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.Dao
{
    public class UserDao : IUser
    {
        public bool UserExists(string username)
        {
            int count = 0;
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT COUNT(Username) FROM User WHERE Username='{username}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        count = reader.GetInt32("Count(Username)");
                     }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Couldn't check if username already exists {e.Message}");
                }
            }
            return count != 0;
        }

        //public bool RegisterUser(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error)
        //{
        //    error = string.Empty;

        //    if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(nickname) || String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName) || String.IsNullOrEmpty(postalCode))
        //    {
        //        error = "Not all values supplied. Could not create user account.";
        //        return false;
        //    }
        //    using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            string insert = "INSERT INTO User(Email, Password, Nickname, FirstName, LastName, PostalCode)" +
        //                            $"VALUES ('{email}','{password}','{nickname}','{firstName}','{lastName}','{postalCode}');";
        //            MySqlCommand command = new MySqlCommand(insert, conn);
        //            command.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine($"Database Error (Users): Cannot enter a new user in database {e.Message}");
        //            error = "Cannot enter a new user in database";
        //            return false;
        //        }
        //    }
        //}

        public bool ValidateUser(string usernameInput, out string error)
        {
            error = string.Empty;
            string username = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT User.Username " +
                                           "FROM User " +
                                           $"WHERE User.Username ='{usernameInput}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            username = row["Username"].ToString();
                        }
                    }
                    else
                    {
                        error = "No user account exists with that username";
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to validate user, couldn't find username {e.Message}");
                    error = "No user account exists with that username";
                }
            }
            return !String.IsNullOrEmpty(username);
        }


        public User ValidatePassword(string username, string password, out string error)
        {
            error = string.Empty;
            User user = new User();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT User.Username, FirstName, LastName " +
                                           "FROM User " +
                                            $"WHERE User.Username ='{username}' AND Password ='{password}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            user.Username = row["Username"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                        }
                    }
                    else
                    {
                        user = null;
                        error = "Didn’t enter correct password for the user";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to validate user's password {e.Message}");
                    error = "Didn’t enter correct password";
                    user = null;
                }
            }

            return user;
        }
       

        //public User GetRegisteredInfoByEmail(string userEmail, out string error)
        //{
        //    error = string.Empty;
        //    User user = new User();
        //    PostalCode postalCode = new PostalCode();
        //    Phone phone = new Phone();

        //    using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            string query = "SELECT u.Email, Nickname, Password, FirstName, LastName, u.PostalCode, City, State, PhoneNumber, Type, CAST(IfShare as CHAR) as IfShare " +
        //                                   "FROM User u JOIN PostalCode p ON u.PostalCode = p.PostalCode LEFT JOIN Phone ph ON u.Email = ph.Email " +
        //                                    $"WHERE u.Email ='{userEmail}';";
        //            MySqlCommand command = new MySqlCommand(query, conn);
        //            MySqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                user.Email = reader.GetString("Email");
        //                user.Nickname = reader.GetString("Nickname");
        //                user.Password = reader.GetString("Password");
        //                user.FirstName = reader.GetString("FirstName");
        //                user.LastName = reader.GetString("LastName");
        //                postalCode.Code = reader.GetString("PostalCode");
        //                postalCode.City = reader.GetString("City");
        //                postalCode.State = reader.GetString("State");
        //                try
        //                {
        //                    phone.PhoneNumber = reader.GetString("PhoneNumber");
        //                }
        //                catch (Exception e)
        //                {
        //                    phone.PhoneNumber = "";
        //                }
        //                try
        //                {
        //                    phone.Type = reader.GetString("Type");
        //                }
        //                catch (Exception e)
        //                {
        //                    phone.Type = "";
        //                }
        //                try
        //                {
        //                    phone.IfShare = reader.GetString("IfShare");
        //                }
        //                catch (Exception e)
        //                {
        //                    phone.IfShare = "-1.0";
        //                }
        //                user.PostalCode = postalCode;

        //                user.Phone = phone;

        //            }
        //            reader.Close();
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine($"Failed to get the User Information {e.Message}");
        //            error = "No User with that email";
        //            user = null;
        //        }
        //    }
        //    return user;
        //}

        //public bool UpdateUserInfoByEmail(string email, string password, string nickname, string firstName, string lastName, string postalCode, out string error)
        //{
        //    error = string.Empty;

        //    using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            string query = "UPDATE User " +
        //                            $"SET Nickname = '{nickname}',Password = '{password}',FirstName = '{firstName}',LastName = '{lastName}',PostalCode = '{postalCode}' " +
        //                            $"WHERE Email = '{email}'; ";
        //            MySqlCommand command = new MySqlCommand(query, conn);
        //            command.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine($"Database Error (Users): Cannot update the user in the database {e.Message}");
        //            error = "Cannot update the user in database";
        //            return false;
        //        }
        //    }
        //}
    }
}
