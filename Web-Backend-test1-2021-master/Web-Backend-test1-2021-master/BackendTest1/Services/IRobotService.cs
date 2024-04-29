
using BackendTest1.Models;
using System;
using static BackendTest1.Services.RobotService;

namespace BackendTest1.Services
{
    public interface IRobotService
    {
        Boolean RobotCanBeCreated(String Name, Detail[] details);
        void AddRobot(String Name, Detail[] details);
        void DeleteRobot(String Name);
        Storage GetCurrentStorage();
        Accounting GetCurrentAccounting();
    }

}
