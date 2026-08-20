namespace TarkovCalculator.Records
{
    public class ItemRecord
    {    
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int FleaLowPrice { get; set; }
        public string? BestTraderName { get; set; }
        public int BestTraderPrice { get; set; }
        public int Profit => BestTraderPrice - FleaLowPrice;
    }
}

