using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tarkov.Infrastructure.Api
{
    public class TarkovAPIService(HttpClient httpClient)
    {
        public async Task<Dictionary<string, ItemDTO>?> GetTarkovItemsAsync()
        {
            string url = "https://json.tarkov.dev/pve/items";

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement
            };

            // Add the custom converter for ItemDTO
            options.Converters.Add(new ItemDTOConverter());

            // Deserialize the full response structure
            var response = await httpClient.GetFromJsonAsync<TarkovApiResponse>(url, options);

            // Extract and return just the items dictionary
            return response?.Data?.Items;
        }
    }
}