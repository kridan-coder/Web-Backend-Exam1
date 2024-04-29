using System;
using System.ComponentModel.DataAnnotations;
using static BackendTest1.Services.RobotService;

namespace BackendTest1.Models
{
    public class MainPageModel
    {
        [Required]
        public Storage Storage;
        [Required]
        public Accounting Accounting;

        public int ChosenRobot;
    }
}
