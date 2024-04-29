using System;
using System.ComponentModel.DataAnnotations;
using static BackendTest1.Services.RobotService;

namespace BackendTest1.Models
{
    public class CheckPageModel
    {
        [Required]
        public Robot Robot { get; set; }
    }
}
