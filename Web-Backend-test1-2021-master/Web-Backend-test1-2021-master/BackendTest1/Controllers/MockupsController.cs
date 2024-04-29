using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest1.Controllers
{
    public class MockupsController : Controller
    {
        public IActionResult Index() => this.View();

        public IActionResult MainPage() => this.View();

        public IActionResult RobotCreate() => this.View();

        public IActionResult RobotCreateTwo() => this.View();

        public IActionResult RobotCreateThree() => this.View();

        public IActionResult RobotCreateFour() => this.View();

        public IActionResult RobotEdit() => this.View();

        public IActionResult RobotDetails() => this.View();

        public IActionResult RobotDelete() => this.View();

    }
}
