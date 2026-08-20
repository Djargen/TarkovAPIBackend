using System;
using System.Collections.Generic;
using System.Text;

namespace Tarkov.Infrastructure.Data.Entities
{
    public class ItemEntity
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int FleaLowPrice { get; set; }
        public string? BestTraderName { get; set; }
        public int BestTraderPrice { get; set; }
    }
}