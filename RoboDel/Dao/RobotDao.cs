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
                    string query = "SELECT r.CourierID, r.StateOfRobot, r.City, r.State, r.Country, rs.DateTime, rs.Speed, rs.Battery, rs.Longitude, rs.Latitude  FROM Robot r LEFT JOIN (SELECT CourierID, DateTime, Speed, Battery, Longitude, Latitude FROM RobotStatistics WHERE CourierID IN (SELECT CourierID FROM RobotStatistics GROUP BY CourierID) AND DateTime IN (SELECT max(DateTime) FROM RobotStatistics GROUP BY CourierID)) as rs ON r.CourierID=rs.CourierID ORDER BY r.StateOfRobot, r.Country, r.State, r.City, r.CourierID;";

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
