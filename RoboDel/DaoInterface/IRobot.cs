using RoboDel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboDel.DaoInterface
{
    public interface IRobot
    {
        public List<Robot> GetAllRobots(out string error);
        public Robot GetRobotDetails(int courierID, out string error);
    }
}
