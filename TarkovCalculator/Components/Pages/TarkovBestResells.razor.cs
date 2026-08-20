using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TarkovCalculator.Protos;
using System.Linq;
using TarkovCalculator.Records;

namespace TarkovCalculator.Components.Pages
{
    public partial class TarkovBestResells
    {
        [Inject]
        public TarkovService.TarkovServiceClient TarkovClient { get; set; } = default!;
        private IEnumerable<ItemRecord> items = Enumerable.Empty<ItemRecord>();
        private bool isLoading = true;

        private readonly Dictionary<string, string> TraderNames = new()
        {
            { "54cb50c76803fa8b248b4571", "Prapor" },
            { "54cb57776803fa99248b456e", "Therapist" },
            { "579dc571d53a0658a154fbec", "Fence" },
            { "58330581ace78e27b8b10cee", "Skier" },
            { "5935c25fb3acc3127c3d8cd9", "Peacekeeper" },
            { "5a7c2eca46aef81a7ca2145d", "Mechanic" },
            { "5ac3b934156ae10c4430e83c", "Ragman" },
            { "5c0647fdd443bc2504c2d371", "Jaeger" },
            { "6617beeaa9cfa777ca915b7c", "Ref" },
            { "656f0f98d80a697f855d34b1", "BTR" }
        };

        private string GetTraderName(string id) =>
        TraderNames.TryGetValue(id, out var name) ? name : "Unknown";

        //get data over gRPC
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await TarkovClient.GetItemsAsync(new GetItemsRequest());
                items = response.Items
                    .Where(i => i.FleaLowPrice > 0)
                    .Select(i => new ItemRecord
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Image = i.Image,
                        FleaLowPrice = i.FleaLowPrice,
                        BestTraderName = i.BestTraderName,
                        BestTraderPrice = i.BestTraderPrice
                    })
                    .OrderByDescending(i => i.Profit)
                    .ToList();

            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
