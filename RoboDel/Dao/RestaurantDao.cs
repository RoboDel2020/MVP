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
    public class RestaurantDao : IRestaurant
    {
        public bool RestaurantExists(string email)
        {
            int count = 0;
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT COUNT(Email) FROM Restaurant WHERE Email='{email}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        count = reader.GetInt32("Count(Email)");
                     }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Couldn't check if email already exists {e.Message}");
                }
            }
            return count != 0;
        }

        public bool RegisterRestaurant(string name, string email, string password, string type, double price, string phoneNumber, string address, string city, string state, string postalCode, string country, double latitude, double longitude, out string error)
        {
            error = string.Empty;

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(name) || String.IsNullOrEmpty(type) || String.IsNullOrEmpty(phoneNumber) || String.IsNullOrEmpty(address) || String.IsNullOrEmpty(city)  || String.IsNullOrEmpty(country))
            {
                error = "Please add all the fields to create restaurant account!";
                return false;
            }
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string insert = "INSERT INTO Restaurant " +
                                    $"VALUES ('{email}','{password}','{name}','{type}',{price},'active','{phoneNumber}','{address}','{city}','{state}','{postalCode}','{country}',{latitude},{longitude});";
                    MySqlCommand command = new MySqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database Error (Users): Cannot enter a new restaurant in database {e.Message}");
                    error = "Cannot enter a new restaurant in database";
                    return false;
                }
            }
        }

        public bool ValidateRestaurant(string emailInput, out string error)
        {
            error = string.Empty;
            string email = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Email " +
                                           "FROM Restaurant " +
                                           $"WHERE Email ='{emailInput}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            email = row["Email"].ToString();
                        }
                    }
                    else
                    {
                        error = "No restaurant account exists with that email";
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to validate resaturant, couldn't find email {e.Message}");
                    error = "No restaurant account exists with that email";
                }
            }
            return !String.IsNullOrEmpty(email);
        }


        public Restaurant ValidatePassword(string email, string password, out string error)
        {
            error = string.Empty;
            Restaurant restaurant = new Restaurant();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Email, Name " +
                                           "FROM Restaurant " +
                                            $"WHERE Email ='{email}' AND Password ='{password}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            restaurant.Email = row["Email"].ToString();
                            restaurant.Name = row["Name"].ToString();
                        }
                    }
                    else
                    {
                        restaurant = null;
                        error = "Didn’t enter correct password for the restaurant";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to validate restaurant's password {e.Message}");
                    error = "Didn’t enter correct password";
                    restaurant = null;
                }
            }

            return restaurant;
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

        
        public Restaurant GetRestaurantDetails(string restaurantEmail, out string error)
        {
            error = string.Empty;
            Restaurant restaurant = new Restaurant();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Restaurant " +
                                            $"WHERE Email ='{restaurantEmail}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            restaurant.Email = row["Email"].ToString();
                            restaurant.Name = row["Name"].ToString();
                            restaurant.Address = row["Address"].ToString();
                            restaurant.City = row["City"].ToString();
                            restaurant.Country = row["Country"].ToString();
                            if (row["Type"] != DBNull.Value)
                            {
                                restaurant.Type = row["Type"].ToString();
                            }
                            if (row["Price"] != DBNull.Value)
                            {
                                restaurant.Price = Double.Parse(row["Price"].ToString());
                            }
                            if (row["Status"] != DBNull.Value)
                            {
                                restaurant.Status = row["Status"].ToString();
                            }
                            if (row["PhoneNumber"] != DBNull.Value)
                            {
                                restaurant.PhoneNumber = row["PhoneNumber"].ToString();
                            }
                            if (row["State"] != DBNull.Value)
                            {
                                restaurant.State = row["State"].ToString();
                            }
                            if (row["Zip"] != DBNull.Value)
                            {
                                restaurant.Zip = row["Zip"].ToString();
                            }
                            if (row["Latitude"] != DBNull.Value)
                            {
                                restaurant.Latitude = Double.Parse(row["Latitude"].ToString());
                            }
                            if (row["Longitude"] != DBNull.Value)
                            {
                                restaurant.Longitude = Double.Parse(row["Longitude"].ToString());
                            }

                        }
                    }
                    else
                    {
                        restaurant = null;
                        error = "Didn’t enter correct email for the restaurant";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get restaurantwith email address {e.Message}");
                    error = "Didn’t enter correct email address for restaurant";
                    restaurant = null;
                }
            }

            return restaurant;
        }


        public List<Restaurant> GetAllRestaurants(out string error)
        {
            error = string.Empty;
            List<Restaurant> restaurants = new List<Restaurant>();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT r.Email as RestaurantEmail, r.Name as RestaurantName, r.Type as RestaurantType, r.Status as RestaurantStatus,r.PhoneNumber as RestaurantPhoneNumber,r.Address as RestaurantAddress,r.City as RestaurantCity, r.State as RestaurantState, r.Zip as RestaurantZip, r.Country as RestaurantCountry, r.Email as RestaurantEmail, r.Longitude as RestaurantLongitude, r.Latitude as RestaurantLatitude  " +
                                   "FROM  Restaurant r  ORDER BY r.Name; ";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Restaurant restaurant = new Restaurant();
                        restaurant.Email = reader.GetString("RestaurantEmail");
                        restaurant.Name = reader.GetString("RestaurantName");
                        try
                        {
                            restaurant.Type = reader.GetString("RestaurantType");
                        }
                        catch (Exception)
                        { }
                        restaurant.Status = reader.GetString("RestaurantStatus");
                        try
                        {
                            restaurant.PhoneNumber = reader.GetString("RestaurantPhoneNumber");
                        }
                        catch (Exception)
                        { }
                        restaurant.Address = reader.GetString("RestaurantAddress");
                        restaurant.City = reader.GetString("RestaurantCity");
                        try
                        {
                            restaurant.State = reader.GetString("RestaurantState");
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            restaurant.Zip = reader.GetString("RestaurantZip");
                        }
                        catch (Exception)
                        { }
                        restaurant.Country = reader.GetString("RestaurantCountry");
                        restaurant.Email = reader.GetString("RestaurantEmail");
                        try
                        {
                            restaurant.Longitude = double.Parse(reader.GetString("RestaurantLongitude"));
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            restaurant.Latitude = double.Parse(reader.GetString("RestaurantLatitude"));
                        }
                        catch (Exception)
                        { }
                        restaurants.Add(restaurant);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get the all restaurants {e.Message}");
                    error = "No restaurants";
                }
            }
            return restaurants;
        }

    }
}
