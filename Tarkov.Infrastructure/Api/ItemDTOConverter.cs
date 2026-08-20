using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tarkov.Infrastructure.Api
{
    public class ItemDTOConverter : JsonConverter<ItemDTO>
    {
        public override ItemDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Return null or default if not at StartObject - handles malformed JSON gracefully
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return new ItemDTO();
            }

            var item = new ItemDTO();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read(); // Move to the property value

                    // Handle properties case-insensitively
                    switch (propertyName.ToLowerInvariant())
                    {
                        case "id":
                            item.Id = reader.GetString();
                            break;

                        case "normalizedname":
                        case "normalizedName":
                            item.Name = reader.GetString();
                            break;

                        case "baseimagelink":
                        case "baseImageLink":
                            item.Image = reader.GetString();
                            break;

                        case "lastlowprice":
                        case "lastLowPrice":
                            if (reader.TokenType == JsonTokenType.Number)
                            {
                                item.FleaLowPrice = reader.GetInt32();
                            }
                            break;

                        case "selltotrader":
                        case "sellToTrader":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                using (var doc = JsonDocument.ParseValue(ref reader))
                                {
                                    var traderPrices = DeserializeTraderPrices(doc.RootElement);
                                    // Find the trader with the highest price
                                    if (traderPrices.Count > 0)
                                    {
                                        var highestTrader = traderPrices.OrderByDescending(t => t.priceRUB).First();
                                        item.BestTraderName = highestTrader.trader;
                                        item.BestTraderPrice = highestTrader.priceRUB;
                                    }
                                }
                            }
                            break;

                        default:
                            // Skip unknown properties by consuming the entire value
                            // This handles both simple values and complex nested structures
                            using (JsonDocument.ParseValue(ref reader))
                            {
                                // ParseValue consumes the entire value structure automatically
                            }
                            break;
                    }
                }
            }

            return item;
        }

        private Dictionary<string, object> ParseJsonElementToDictionary(JsonElement element)
        {
            var result = new Dictionary<string, object>();

            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in element.EnumerateObject())
                {
                    result[property.Name] = ConvertJsonElement(property.Value);
                }
            }

            return result;
        }

        private object ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt32(out int intValue) ? intValue : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => ConvertJsonArray(element),
                JsonValueKind.Object => ParseJsonElementToDictionary(element),
                _ => element.ToString()
            };
        }

        private List<object> ConvertJsonArray(JsonElement element)
        {
            var list = new List<object>();
            foreach (var item in element.EnumerateArray())
            {
                list.Add(ConvertJsonElement(item));
            }
            return list;
        }

        private List<TraderPriceDTO> DeserializeTraderPrices(JsonElement element)
        {
            var traders = new List<TraderPriceDTO>();

            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        var traderPrice = new TraderPriceDTO();
                        foreach (var prop in item.EnumerateObject())
                        {
                            switch (prop.Name.ToLowerInvariant())
                            {
                                case "trader":
                                    traderPrice.trader = prop.Value.GetString();
                                    break;
                                case "pricerub":
                                    if (prop.Value.TryGetInt32(out int price))
                                        traderPrice.priceRUB = price;
                                    break;
                            }
                        }
                        traders.Add(traderPrice);
                    }
                }
            }

            return traders;
        }

        public override void Write(Utf8JsonWriter writer, ItemDTO value, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Writing ItemDTO is not implemented");
        }
    }
}
