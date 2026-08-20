using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Tarkov.Infrastructure.Api
{
    public class ItemDTO
    {
        public string id { get; set; }
        public string normalizedName { get; set; }
        public string baseImageLink { get; set; }
        public int LastLowPrice { get; set; }
        public string? HighestTraderName { get; set; }
        public int HighestTraderPrice { get; set; }
    }
}