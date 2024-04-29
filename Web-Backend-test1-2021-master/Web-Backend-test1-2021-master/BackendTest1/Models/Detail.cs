using System;

namespace BackendTest1.Models
{
    public sealed class Detail
    {
        private static Random random = new Random();
        public Detail(String name, DetailType type, int price, int amountInStorage, int amountOnRobots)
        {
            this.Name = name;
            this.Type = type;
            this.Price = price;
            this.AmountInStorage = amountInStorage;
            this.AmountOnRobots = amountOnRobots;
            this.MaxAmount = random.Next(11);
        }

        public String Name { get; set; }
        public int MaxAmount { get; set; }
        public DetailType Type { get; set; }
        public int Price { get; set; }
        public int AmountInStorage { get; set; }
        public int AmountOnRobots { get; set; }
    }

}