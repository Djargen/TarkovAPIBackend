using Grpc.Core;
using Tarkov.Infrastructure.Api;
using Tarkov.API.Protos;

namespace Tarkov.API.Services
{
    public class TarkovGrpcService : TarkovService.TarkovServiceBase
    {
        private readonly TarkovAPIService _tarkovApiService;

        public TarkovGrpcService(TarkovAPIService tarkovApiService)
        {
            _tarkovApiService = tarkovApiService;
        }

        public override async Task<GetItemsResponse> GetItems(GetItemsRequest request, ServerCallContext context)
        {
            var items = await _tarkovApiService.GetTarkovItemsAsync();

            if (items == null)
            {
                return new GetItemsResponse();
            }

            var response = new GetItemsResponse();

            foreach (var item in items.Values)
            {
                var protoItem = new Item
                {
                    Id = item.id,
                    NormalizedName = item.normalizedName ?? "",
                    BaseImageLink = item.baseImageLink,
                    LastLowPrice = item.LastLowPrice,
                    HighestTraderName = item.HighestTraderName ?? "",
                    HighestTraderPrice = item.HighestTraderPrice
                };

                response.Items.Add(protoItem);
            }

            return response;
        }
    }
}
