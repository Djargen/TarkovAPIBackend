using Microsoft.AspNetCore.Mvc;
using Tarkov.API.Services;

namespace Tarkov.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarkovController(TarkovAPIService tarkovApiService) : ControllerBase
    {
        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var items = await tarkovApiService.GetTarkovItemsAsync();

            if (items == null)
            {
                return NotFound("Geen items gevonden");
            }

            // Return all items as a collection
            return Ok(items.Values);
        }
    }
}
