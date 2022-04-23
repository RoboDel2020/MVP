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
                        catch (Exception e)
                        { }
                        robots.Add(robot);
                        Console.WriteLine(robot.CourierID);
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
