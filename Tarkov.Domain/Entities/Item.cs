using System;
using System.Collections.Generic;
using System.Text;

namespace Tarkov.Domain.Entities
{
    public class Item
    {
        public required string _id { get; set; }
        public required string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsOnFleaMarket = true;
        //prices always in RUB
        public int FlealastLowPrice {  get; set; }
        public int TraderHighPrice { get; set; }
        public int Gain => TraderHighPrice - FlealastLowPrice;

        //Gain = (TraderHighPrice - FlealastLowPrice);
    }
}
