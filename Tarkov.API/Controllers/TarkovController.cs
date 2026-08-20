using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tarkov.API.Services;
using Tarkov.Infrastructure.Data;

namespace Tarkov.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarkovController(TarkovDbContext context) : ControllerBase
    {
        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var items = await context.Items.ToListAsync();

            if (items == null)
            {
                return NotFound("Geen items gevonden");
            }

            // Return all items as a collection
            return Ok(items);
        }
    }
}
