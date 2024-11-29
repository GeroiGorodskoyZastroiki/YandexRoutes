using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace YandexRoutes.Server.Models
{
    public class DatabaseService
    {
        public AppDbContext Context { get; private set; }

        public DatabaseService(AppDbContext context)
        {
            Context = context;

            if (!Context.Database.CanConnect())
                Context.Database.EnsureCreated();

            UpdateDb();
        }

        private AppDbContext UpdateDb()
        {
            using var client = new HttpClient();

            //Какая-то логика, чтобы получить apiLink
            string apiLink = "https://courier.yandex.ru/vrs/api/v1/result/mvrp/60a75a07-b48dc458-6470b004-a286d0c";

            // Получаем и десериализуем JSON
            var result = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync(apiLink).Result).result;

            // Добавляем машины
            var vehicles = result.vehicles;
            foreach (var vehicle in vehicles)
            {
                int vehicleId = vehicle.id;
                var vehicleName = (string)vehicle.@ref;

                if (!Context.Vehicles.Any(v => v.Id == vehicleId))
                {
                    Context.Vehicles.Add(new Vehicle
                    {
                        Id = vehicleId,
                        Name = vehicleName
                    });
                }
            }

            // Добавляем маршруты
            var routes = result.routes;
            foreach (var route in routes)
            {
                int vehicleId = (int)route.vehicle_id;
                var points = ((IEnumerable<dynamic>)route.route)
                    .Select(point => (string)point.node.value.@ref)
                    .ToList();

                var existingRoutes = Context.Routes
                    .Where(r => r.VehicleId == vehicleId)
                    .AsEnumerable();

                if (!existingRoutes.Any(r => r.Points.SequenceEqual(points)))
                {
                    Context.Routes.Add(new Route
                    {
                        VehicleId = vehicleId,
                        Points = points
                    });
                }
            }

            // Сохраняем все изменения в базе
            Context.SaveChanges();

            return Context;
        }

    }
}
