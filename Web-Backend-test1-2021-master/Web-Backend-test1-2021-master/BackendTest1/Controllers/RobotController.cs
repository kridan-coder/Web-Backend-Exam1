using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackendTest1.Models;
using BackendTest1.Services;
using WebApplication2.Models;

namespace BackendTest1.Controllers
{
    public class RobotController : Controller
    {
        private static Random random = new Random();
        private readonly IRobotService robotService;

        public RobotController(IRobotService robotService)
        {
            this.robotService = robotService;
        }

        public IActionResult Index()
        {
            return View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Boolean CreatePressed, int? Delete, int? Edit, int? Check)
        {
            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError("", "Invalid Code");
                return this.View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
            }

            if (CreatePressed)
            {
                return this.View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
            }
            else if (Delete != null)
            {
                var storage = robotService.GetCurrentStorage();
                var newModel = new DeletePageModel { Cost = storage.Robots[(int)Delete].TotalCost, Mods = storage.Robots[(int)Delete].TotalMods, Name = storage.Robots[(int)Delete].Name };
                return View("DeleteRobot", newModel);
            }
            else if (Check != null)
            {
                var storage = robotService.GetCurrentStorage();
                var newModel = new CheckPageModel { Robot = storage.Robots[(int)Check] };
                return View("CheckRobot", newModel);
            }
            else if(Edit != null)
            {
                return this.View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
            }
            return this.View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRobot(Boolean Delete, DeletePageModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError("", "Invalid Code");
                return this.View(new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
            }

            if (Delete)
            {
                robotService.DeleteRobot(model.Name);
            }

            return View("Index", new MainPageModel { Storage = robotService.GetCurrentStorage(), Accounting = robotService.GetCurrentAccounting() });
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
