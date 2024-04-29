using System;
using System.ComponentModel.DataAnnotations;
using static BackendTest1.Services.RobotService;

namespace BackendTest1.Models
{
    public class DeletePageModel
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public int Mods { get; set; }
    }
}
