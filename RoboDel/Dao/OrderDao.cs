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
    public class OrderDao : IOrder
    {
        public List<Order> GetAllOrders(out string error)
        {
            error = string.Empty;
            List<Order> orders = new List<Order>();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT o.ID as OrderID,o.PickupTime as OrderPickupTime, o.ReadyForPickup as OrderReadyForPickup, o.DateTime as OrderDateTime, o.Status as OrderStatus, o.Longitude as OrderLongitude, o.Latitude as OrderLatitude, r.Name as RestaurantName, r.Type as RestaurantType, r.Status as RestaurantStatus,r.PhoneNumber as RestaurantPhoneNumber,r.Address as RestaurantAddress,r.City as RestaurantCity, r.State as RestaurantState, r.Zip as RestaurantZip, r.Country as RestaurantCountry, r.Email as RestaurantEmail, r.Longitude as RestaurantLongitude, r.Latitude as RestaurantLatitude, c.ID as CustomerID, c.FirstName as CustomerFirstName, c.LastName as CustomerLastName, c.PhoneNumber as CustomerPhoneNumber, c.Email as CustomerEmail,  c.Address as CustomerAddress,c.City as CustomerCity, c.State as CustomerState, c.Zip as CustomerZip, c.Country as CustomerCountry " +
                                   "FROM `Order` o JOIN Restaurant r ON o.RestaurantEmail = r.Email JOIN Customer c ON c.ID = o.CustomerID ORDER BY o.DateTime; ";
                    
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order order = new Order();
                        Customer customer = new Customer();
                        Restaurant restaurant = new Restaurant();

                        order.ID = Int16.Parse(reader.GetString("OrderID"));
                        order.DateTime = DateTime.Parse(reader.GetString("OrderDateTime"));
                        order.PickupTime = DateTime.Parse(reader.GetString("OrderPickupTime"));
                        order.ReadyForPickup = bool.Parse(reader.GetString("OrderReadyForPickup"));
                        order.Status = reader.GetString("OrderStatus");
                        try
                        {
                            order.Longitude = double.Parse(reader.GetString("OrderLongitude"));
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            order.Latitude = double.Parse(reader.GetString("OrderLatitude"));
                        }
                        catch (Exception)
                        { }
                        customer.ID = Int16.Parse(reader.GetString("CustomerID"));
                        customer.FirstName = reader.GetString("CustomerFirstName");
                        try
                        {
                            customer.LastName = reader.GetString("CustomerLastName");
                        }
                        catch (Exception)
                        { }
                        
                        customer.PhoneNumber = reader.GetString("CustomerPhoneNumber");
                        try
                        {
                            customer.Email = reader.GetString("CustomerEmail");
                        }
                        catch (Exception)
                        { }
                        
                        customer.Address = reader.GetString("CustomerAddress");
                        customer.City = reader.GetString("CustomerCity");
                        try
                        {
                            customer.State = reader.GetString("CustomerState");
                        }
                        catch (Exception)
                        { }
                        
                        try
                        {
                            customer.Zip = reader.GetString("CustomerZip");
                        }
                        catch (Exception)
                        { }
                        
                        customer.Country = reader.GetString("CustomerCountry");
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
                        order.Restaurant = restaurant;
                        order.Customer = customer;
                        orders.Add(order);

                    }


                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get the alls orders {e.Message}");
                    error = "No Orders";
                }
            }
            return orders;
        }

        public Order GetOrderDetails(int orderID, out string error)
        {
            error = string.Empty;
            Order order = new Order();
            Customer customer = new Customer();
            Restaurant restaurant = new Restaurant();
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * " +
                                   $"FROM `Order` o WHERE o.ID = '{orderID}'; ";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            order.ID = Int16.Parse(row["ID"].ToString());
                            order.DateTime = DateTime.Parse(row["DateTime"].ToString());
                            order.PickupTime = DateTime.Parse(row["PickupTime"].ToString());
                            order.ReadyForPickup = bool.Parse(row["ReadyForPickup"].ToString());
                            order.Status = row["Status"].ToString();
                            restaurant.Email = row["RestaurantEmail"].ToString();
                            customer.ID = Int16.Parse(row["CustomerID"].ToString());
                            order.Restaurant = restaurant;
                            order.Customer = customer;
                            if (row["Latitude"] != DBNull.Value)
                            {
                                order.Latitude = Double.Parse(row["Latitude"].ToString());
                            }
                            if (row["Longitude"] != DBNull.Value)
                            {
                                order.Longitude = Double.Parse(row["Longitude"].ToString());
                            }
                        }
                    }
                    else
                    {
                        order = null;
                        error = "Didn’t enter correct ID for the order";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get order with ID {e.Message}");
                    error = "Didn’t enter correct ID for order";
                    order = null;
                }
            }

            return order;
        }

        public bool AddOrder(String pickupTime, bool readyForPickup, string restaurantEmail, int customerID, out string error)
        {
            error = string.Empty;

            if (String.IsNullOrEmpty(pickupTime) || String.IsNullOrEmpty(restaurantEmail) || String.IsNullOrEmpty(customerID.ToString()))
            {
                error = "Not all values supplied. Could not add customer.";
                return false;
            }
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string insert = "INSERT INTO `Order`(DateTime, PickupTime, ReadyForPickup,  Status, RestaurantEmail, CustomerID) " +
                                    $"VALUES(NOW(),'{pickupTime}', {readyForPickup}, 'pending', '{restaurantEmail}', '{customerID}'); ";
                    MySqlCommand command = new MySqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database Error (Order): Cannot enter a new order in database {e.Message}");
                    error = "Cannot enter a new order in database";
                    return false;
                }
            }
        }

    }
}
