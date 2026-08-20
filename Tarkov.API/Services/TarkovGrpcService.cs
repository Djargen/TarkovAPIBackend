using Grpc.Core;
using Tarkov.Infrastructure.DTOResponses;
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
                    Id = item.Id,
                    Name = item.Name ?? "",
                    Image = item.Image,
                    FleaLowPrice = item.FleaLowPrice,
                    BestTraderName = item.BestTraderName ?? "",
                    BestTraderPrice = item.BestTraderPrice
                };

                response.Items.Add(protoItem);
            }

            return response;
        }
    }
}
