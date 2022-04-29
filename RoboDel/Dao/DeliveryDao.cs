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
    public class DeliveryDao : IDelivery
    {
        public bool AddDelivery(int orderId, out string error)
        {
            error = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string insert = "INSERT INTO Delivery(OrderID,ToRestaurant, Status) " +
                                    $"Values({orderId},NOW(), 'active'); ";
                    MySqlCommand command = new MySqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database Error (Order): Cannot enter a new delivery into database {e.Message}");
                    error = "Cannot enter a new delivery into database";
                    return false;
                }
            }
        }

        public int GetActiveDeliveryByOrderID(int orderId)
        {
            int foundOrderID = -1;
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT ID FROM Delivery WHERE OrderID={orderId} AND Status='active';";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundOrderID = reader.GetInt16("ID");
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Couldn't find the Delivery ID by the Order ID {e.Message}");
                }
            }
            return foundOrderID;
        }

        public bool AddCourierForDelivery(int deliveryID, int courierID, out string error)
        {
            error = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string insert = "INSERT INTO CourierForDelivery(DeliveryID,CourierID, StartTime) " +
                                    $"Values({deliveryID},{courierID},NOW()); ";
                    MySqlCommand command = new MySqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database Error (Order): Cannot enter a new courier for delivery into database {e.Message}");
                    error = "Cannot enter a new courier for delivery into database";
                    return false;
                }
            }
        }
    }
}
