using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexRoutes.Server.Models;

namespace YandexRoutes.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public TransportController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehicles()
        {
            var context = _dbService.GetDbContext();
            var vehicles = await context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("routes/{vehicleId}")]
        public async Task<IActionResult> GetRoutes(uint vehicleId)
        {
            var context = _dbService.GetDbContext();
            var routes = await context.Routes
                .Where(r => r.VehicleId == vehicleId)
                .Select(r => new { r.VehicleId, r.Points })
                .ToListAsync();

            return Ok(routes);
        }
    }
}
