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
    public class RobotDao : IRobot
    {
        public List<Robot> GetAllRobotsWithCurrentStatistics(out string error)
        {
            error = string.Empty;
            List<Robot> robots = new List<Robot>();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT r.CourierID, r.StateOfRobot, r.City, r.State, r.Country, rs.DateTime, rs.Speed, rs.Battery, rs.Longitude, rs.Latitude  FROM Robot r LEFT JOIN (SELECT CourierID, DateTime, Speed, Battery, Longitude, Latitude FROM RobotStatistics WHERE (CourierID, DateTime) IN (SELECT CourierID, max(DateTime) as DateTime FROM RobotStatistics GROUP BY CourierID)) as rs ON r.CourierID=rs.CourierID ORDER BY r.StateOfRobot, r.Country, r.State, r.City, r.CourierID;";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Robot robot = new Robot();
                        RobotStatistics robotStatistics = new RobotStatistics();
                        robot.CourierID = Int16.Parse(reader.GetString("CourierID"));
                        robot.StateOfRobot = reader.GetString("StateOfRobot");
                        robot.City = reader.GetString("City");
                        robot.Country = reader.GetString("Country");
                        try
                        {
                            robot.State = reader.GetString("State"); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.DateTime = DateTime.Parse(reader.GetString("DateTime")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Speed = double.Parse(reader.GetString("Speed")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Battery = double.Parse(reader.GetString("Battery")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Longitude = double.Parse(reader.GetString("Longitude")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Latitude = double.Parse(reader.GetString("Latitude")); ;
                        }
                        catch (Exception)
                        { }
                        robot.CurrentRobotStatistics = robotStatistics;
                        robots.Add(robot);
                    }


                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get the all robots with current statistics {e.Message}");
                    error = "No robots";
                }
            }
            return robots;
        }

        public List<CourierForDelivery> GetAllActiveRobotsWithCurrentStatisticsAndOrders(out string error)
        {
            error = string.Empty;
            List<CourierForDelivery> couriersForDelivery = new List<CourierForDelivery>();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query =  "SELECT r.CourierID, r.StateOfRobot, r.City, r.State, r.Country, rs.DateTime, rs.Speed, rs.Battery, rs.Longitude, rs.Latitude, crd.DeliveryID, crd.StartTime, d.OrderID, o.Longitude as OrderLongitude, o.Latitude as OrderLatitude " +
                                    "FROM Robot r LEFT JOIN " +
                                    "(SELECT CourierID, DateTime, Speed, Battery, Longitude, Latitude FROM RobotStatistics WHERE(CourierID, DateTime) IN(SELECT CourierID, max(DateTime) as DateTime FROM RobotStatistics GROUP BY CourierID)) as rs ON r.CourierID = rs.CourierID " +
                                    "LEFT JOIN CourierForDelivery crd on r.CourierID = crd.CourierID LEFT JOIN Delivery d ON crd.DeliveryID = d.ID LEFT JOIN `Order` o ON d.OrderID = o.ID " +
                                    "WHERE r.StateOfRobot = 'active' AND crd.EndTime is NULL " +
                                    "ORDER BY r.CourierID; ";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Robot robot = new Robot();
                        CourierForDelivery courierForDelivery = new CourierForDelivery();
                        RobotStatistics robotStatistics = new RobotStatistics();
                        Delivery delivery = new Delivery();
                        Order order = new Order();
                        Restaurant restaurant = new Restaurant();
                        robot.CourierID = Int16.Parse(reader.GetString("CourierID"));
                        robot.StateOfRobot = reader.GetString("StateOfRobot");
                        robot.City = reader.GetString("City");
                        robot.Country = reader.GetString("Country");
                        try
                        {
                            robot.State = reader.GetString("State"); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.DateTime = DateTime.Parse(reader.GetString("DateTime")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Speed = double.Parse(reader.GetString("Speed")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Battery = double.Parse(reader.GetString("Battery")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Longitude = double.Parse(reader.GetString("Longitude")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            robotStatistics.Latitude = double.Parse(reader.GetString("Latitude")); ;
                        }
                        catch (Exception)
                        { }


                        try
                        {
                            order.ID = Int16.Parse(reader.GetString("OrderID")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            order.Longitude = double.Parse(reader.GetString("OrderLongitude")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            order.Latitude = double.Parse(reader.GetString("OrderLatitude")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            delivery.ID = Int16.Parse(reader.GetString("DeliveryID")); ;
                        }
                        catch (Exception)
                        { }
                        try
                        {
                            courierForDelivery.StartTime = DateTime.Parse(reader.GetString("StartTime")); ;
                        }
                        catch (Exception)
                        { }
                        delivery.Order = order;
                        robot.CurrentRobotStatistics = robotStatistics;
                        courierForDelivery.Robot = robot;
                        courierForDelivery.Delivery = delivery;
                        couriersForDelivery.Add(courierForDelivery);
                    }


                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get the all active robots with current statistics and orders {e.Message}");
                    error = "No robots";
                }
            }
            return couriersForDelivery;
        }


        public List<Robot> GetAllRobots(out string error)
        {
            error = string.Empty;
            List<Robot> robots = new List<Robot>();

            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT *  FROM Robot ORDER BY StateOfRobot; ";
                    
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Robot robot = new Robot();
                        robot.CourierID = Int16.Parse(reader.GetString("CourierID"));
                        robot.StateOfRobot = reader.GetString("StateOfRobot");
                        robot.City = reader.GetString("City");
                        robot.Country = reader.GetString("Country");
                        try
                        {
                            robot.State = reader.GetString("State"); ;
                        }
                        catch (Exception)
                        { }
                        robots.Add(robot);
                    }


                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get the all robots {e.Message}");
                    error = "No robots";
                }
            }
            return robots;
        }

        public Robot GetRobotDetails(int courierID, out string error)
        {
            error = string.Empty;
            Robot robot = new Robot();
            using (MySqlConnection conn = new MySqlConnection(Database.ConnectionStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * " +
                                   $"FROM Robot WHERE CourierID = '{courierID}'; ";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numberOfResults = dt.Rows.Count;
                    if (numberOfResults == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            robot.CourierID = Int16.Parse(row["ID"].ToString());
                            robot.StateOfRobot = row["StateOfRobot"].ToString(); 
                            robot.City = row["City"].ToString();
                            robot.Country = row["Country"].ToString();
                            if (row["State"] != DBNull.Value)
                            {
                                robot.State = row["State"].ToString();
                            }
                        }
                    }
                    else
                    {
                        robot = null;
                        error = "Didn’t enter correct courier ID for the robot";

                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to get robot with ID {e.Message}");
                    error = "Didn’t enter correct ID for robot";
                    robot = null;
                }
            }
            return robot;
        }

    }
}
