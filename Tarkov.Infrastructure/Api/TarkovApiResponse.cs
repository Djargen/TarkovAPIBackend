using System.Text.Json;

namespace Tarkov.Infrastructure.Api
{
    /// <summary>
    /// Represents the root response structure from the Tarkov API
    /// </summary>
    public class TarkovApiResponse
    {
        public ApiData? Data { get; set; }
        public List<string>? Translations { get; set; }
    }

    /// <summary>
    /// Represents the data object within the API response
    /// </summary>
    public class ApiData
    {
        public Dictionary<string, ItemDTO>? Items { get; set; }
        public JsonElement? Skills { get; set; }
        public JsonElement? ItemCategories { get; set; }
        public JsonElement? HandbookCategories { get; set; }
        public JsonElement? FleaMarket { get; set; }
        public JsonElement? ArmorMaterial { get; set; }
        public JsonElement? SpecialItems { get; set; }
        public ApiSettings? Settings { get; set; }
    }

    /// <summary>
    /// Represents the settings object within the API data
    /// </summary>
    public class ApiSettings
    {
        public int ScavCooldownSeconds { get; set; }
        public bool GlobalMaxTraders { get; set; }
        public int GpCoinValue { get; set; }
    }
}
