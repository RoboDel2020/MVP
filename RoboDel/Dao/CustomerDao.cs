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
    }
}
