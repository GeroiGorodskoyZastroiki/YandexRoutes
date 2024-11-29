using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexRoutes.Server.Models;

namespace YandexRoutes.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var vehicles = await _dbService.Context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("routes/{vehicleId}")]
        public async Task<IActionResult> GetRoutes(int vehicleId)
        {
            var routes = await _dbService.Context.Routes
                .Where(r => r.VehicleId == vehicleId)
                .Select(r => r.Points )
                .ToListAsync();

            return Ok(routes);
        }
    }
}
