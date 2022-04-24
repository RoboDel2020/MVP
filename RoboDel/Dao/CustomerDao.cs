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
    public class CustomerDao : ICustomer
    {

        public Customer GetCustomerDetails(int customerID, out string error)
        {
            error = string.Empty;
            Customer customer = new Customer();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Customer " +
                                            $"WHERE ID ='{customerID}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            customer.ID = Int16.Parse(row["ID"].ToString());
                            customer.FirstName = row["FirstName"].ToString();
                            customer.PhoneNumber = row["PhoneNumber"].ToString();
                            customer.Address = row["Address"].ToString();
                            customer.City = row["City"].ToString();
                            customer.Country = row["Country"].ToString();
                            if (row["LastName"] != DBNull.Value)
                            {
                                customer.LastName = row["LastName"].ToString();
                            }
                            if (row["Email"] != DBNull.Value)
                            {
                                customer.Email = row["Email"].ToString();
                            }
                            if (row["State"] != DBNull.Value)
                            {
                                customer.State = row["State"].ToString();
                            }
                            if (row["Zip"] != DBNull.Value)
                            {
                                customer.Zip = row["Zip"].ToString();
                            }
                        }
                    }
                    else
                    {
                        customer = null;
                        error = "Didn’t enter correct ID for the customer";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get customer with ID {e.Message}");
                    error = "Didn’t enter correct ID for customer";
                    customer = null;
                }
            }

            return customer;
        }

        public int CustomerExists(string firstName, string lastName, string email, string phoneNumber, string address, string city, string zip, string state, string country, out string error)
        {
            error = string.Empty;
            int customerID = -1;
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT ID FROM Customer WHERE FirstName='{firstName}' AND LastName='{lastName}' AND Email='{email}' AND PhoneNumber='{phoneNumber}' AND Address='{address}' AND City='{city}' AND Zip='{zip}' AND State='{state}' AND Country='{country}';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customerID = reader.GetInt16("ID");
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Couldn't check if there is a customer with that information {e.Message}");
                    error = "Failed to validate the customer!";

                }
            }
            return customerID;
        }


        public bool AddCustomer(string firstName, string lastName, string email, string phoneNumber, string address, string city, string zip, string state, string country, out string error)
        {
            error = string.Empty;

            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(phoneNumber) || String.IsNullOrEmpty(address) || String.IsNullOrEmpty(city) || String.IsNullOrEmpty(country))
            {
                error = "Not all values supplied. Could not add customer.";
                return false;
            }
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string insert = "INSERT INTO Customer(FirstName,LastName,PhoneNumber,Email,Address,City,State,Zip,Country) " +
                                    $"VALUES('{firstName}', '{lastName}', '{phoneNumber}', '{email}', '{address}', '{city}', '{state}', '{zip}', '{country}'); ";
                    MySqlCommand command = new MySqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database Error (Customer): Cannot enter a new customer in database {e.Message}");
                    error = "Cannot enter a new customer in database";
                    return false;
                }
            }
        }

        public int GetLastInsertedCustomerID()
        {
            int customerID = -1;
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT LAST_INSERT_ID() as lastCustomerID from Customer Limit 1;";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customerID = reader.GetInt16("lastCustomerID");
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Couldn't find the last added customer ID {e.Message}");
                }
            }
            return customerID;
        }

    }
}
