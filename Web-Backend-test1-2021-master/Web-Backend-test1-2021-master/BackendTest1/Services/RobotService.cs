using System;
using System.Collections.Generic;
using System.Linq;
using BackendTest1.Models;
using Microsoft.Extensions.Logging;

namespace BackendTest1.Services
{
    public sealed class RobotService : IRobotService
    {
        private readonly ILogger logger;

        private readonly Storage storage = new Storage();
        
        private readonly Accounting accounting = new Accounting();
        

        private static Random random = new Random();



        public RobotService(ILogger<IRobotService> logger)
        {
            this.logger = logger;
        }

        public Storage GetCurrentStorage()
        {
            lock (storage)
            {
                this.logger.LogInformation($"Making copy of current storage and sending it to Model.");
                var storageCopy = new Storage();
                storageCopy.Details = storage.Details;
                storageCopy.Robots = storage.Robots;
                return storageCopy;
            }
        }

        public Accounting GetCurrentAccounting()
        {
            lock (accounting)
            {
                this.logger.LogInformation($"Making copy of current accounting and sending it to Model.");
                var accountingCopy = new Accounting();
                accountingCopy.CreatedRobots = accounting.CreatedRobots;
                accountingCopy.DestroyedRobots = accounting.DestroyedRobots;
                accountingCopy.CurrMoney = accounting.CurrMoney;
                accountingCopy.SpentMoney = accounting.SpentMoney;
                accountingCopy.FreeModules = accounting.FreeModules;
                accountingCopy.UsedModules = accounting.UsedModules;
                return accountingCopy;
            }
        }

        public void AddRobot(String Name, Detail[] details)
        {

            int sum = 0;
            int mods = 0;
            for (int i = 0; i < details.Length; i++)
            {
                sum += details[i].Price;
                if (details[i].Type == DetailType.ModArms || details[i].Type == DetailType.ModHead || details[i].Type == DetailType.ModBody)
                {
                    mods += 1;
                }
            }

            lock (storage)
            {
                this.logger.LogInformation($"Creating the robot.");
                this.storage.Robots.Add(new Robot(Name, details, mods, sum));

                for (int i = 0; i < details.Length; i++)
                {
                    details[i].AmountInStorage--;
                    details[i].AmountOnRobots++;
                }

                lock (accounting)
                {
                    this.logger.LogInformation($"Making the accounting.");
                    accounting.CreatedRobots++;
                    accounting.SpentMoney += sum;
                    accounting.CurrMoney -= sum;
                }
            }
        }

        public bool RobotCanBeCreated(string Name, Detail[] details)
        {
            lock(storage)
            {
                Robot robot = null;
                for (int i = 0; i < storage.Robots.Count; i++)
                {
                    if (Name == storage.Robots[i].Name && Enumerable.SequenceEqual(storage.Robots[i].Details, details))
                    {
                        robot = storage.Robots[i];
                    }
                }

                if (robot != null)
                {
                    return false;
                }
                else
                {
                    lock (accounting)
                    {
                        int sum = 0;
                        for (int i = 0; i < details.Length; i++)
                        {
                            sum += details[i].Price;
                        }
                        if (sum > this.accounting.CurrMoney)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
        }

        public void DeleteRobot(string Name)
        {
            lock (storage)
            {
                Robot robot = null;
                for (int i = 0; i < storage.Robots.Count; i++)
                {
                    if (Name == storage.Robots[i].Name)
                    {
                        robot = storage.Robots[i];
                    }
                }

                if (robot != null)
                {

                    this.logger.LogInformation($"Deleting the robot.");
                    Detail[] details = robot.Details;
                    int sum = robot.TotalCost;
                    int mods = robot.TotalMods;
                    this.storage.Robots.Remove(robot);
                    for (int i = 0; i < details.Length; i++)
                    {
                        details[i].AmountInStorage++;
                        details[i].AmountOnRobots--;
                    }
                    lock (accounting)
                    {
                        this.logger.LogInformation($"Making the accounting.");
                        accounting.DestroyedRobots++;
                        accounting.CurrMoney += (int)(sum * 0.8);
                        accounting.FreeModules += mods;
                        accounting.UsedModules -= mods;
                    }
                }
            }
        }

        public sealed class Accounting
        {

            public Accounting()
            {
                CurrMoney = random.Next(5001) + 10000;
                SpentMoney = 0;

                CreatedRobots = 1;
                DestroyedRobots = 0;

                // set them
                FreeModules = 42;
                UsedModules = 3;
            }

            public int CurrMoney { get; set; }
            public int SpentMoney { get; set; }

            public int FreeModules { get; set; }
            public int UsedModules { get; set; }

            public int CreatedRobots { get; set; }
            public int DestroyedRobots { get; set; }

        }

        public sealed class Storage
        {
            public Storage()
            {
                Robots = new List<Robot>();
                Details = new Dictionary<DetailType, Detail[]>();

                Details[DetailType.Arms] = new Detail[] { new Detail("Wooden Arm", DetailType.Arms, 500, 3, 0), new Detail("Iron Arm", DetailType.Arms, 1500, 2, 0), new Detail("Emerald Arm", DetailType.Arms, 5500, 1, 0) };
                Details[DetailType.Body] = new Detail[] { new Detail("Wooden Body", DetailType.Body, 1500, 4, 0), new Detail("Iron Body", DetailType.Body, 6500, 2, 0), new Detail("Emerald Body", DetailType.Body, 8500, 1, 0) };
                Details[DetailType.Head] = new Detail[] { new Detail("Wooden Head", DetailType.Head, 3500, 1, 0), new Detail("Iron Head", DetailType.Head, 4500, 3, 0), new Detail("Emerald Head", DetailType.Head, 6500, 5, 0) };

                Details[DetailType.ModArms] = new Detail[] { new Detail("Lighted Arm", DetailType.ModArms, 250, 1, 0), new Detail("Shooting Arm", DetailType.ModArms, 3500, 1, 0), new Detail("One Punch Arm", DetailType.ModArms, 9999, 2, 0) };
                Details[DetailType.ModBody] = new Detail[] { new Detail("Lighted Body", DetailType.ModBody, 500, 2, 0), new Detail("Spike Body", DetailType.ModBody, 2500, 4, 0), new Detail("Bullet Proof Body", DetailType.ModBody, 1500, 2, 0) };
                Details[DetailType.ModHead] = new Detail[] { new Detail("Lighted Head", DetailType.ModHead, 200, 1, 0), new Detail("Spike Head", DetailType.ModHead, 1200, 2, 0), new Detail("Bullet Proof Head", DetailType.ModHead, 1000, 7, 0) };

                var Name = "Mister Twister";
                var details = new Detail[] { Details[DetailType.Arms][0], Details[DetailType.Body][0], Details[DetailType.Head][0], Details[DetailType.ModArms][0], Details[DetailType.ModArms][1], Details[DetailType.ModBody][2] };

                int sum = 0;
                int mods = 0;
                for (int i = 0; i < details.Length; i++)
                {
                    sum += details[i].Price;
                    if (details[i].Type == DetailType.ModArms || details[i].Type == DetailType.ModHead || details[i].Type == DetailType.ModBody)
                    {
                        mods += 1;
                    }
                }

                Robots.Add(new Robot(Name, details, mods, sum));

                for (int i = 0; i < details.Length; i++)
                {
                    details[i].AmountInStorage--;
                    details[i].AmountOnRobots++;
                }
            }
            public List<Robot> Robots { get; set; }
            public Dictionary<DetailType, Detail[]> Details { get; set; }
        }


        public sealed class Robot
        {
            public Robot(String Name, Detail[] Details, int TotalMods, int TotalCost)
            {
                this.Name = Name;
                this.Details = Details;
                this.TotalMods = TotalMods;
                this.TotalCost = TotalCost;
            }

            public String Name { get; set; }
            public int TotalMods { get; set; }
            public int TotalCost { get; set; }
            public Detail[] Details { get; set; }
        }



    }
}