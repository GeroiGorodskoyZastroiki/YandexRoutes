using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace YandexRoutes.Server.Models
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public AppDbContext GetDbContext()
        {
            // Настроим HttpClient для получения JSON
            using var client = new HttpClient();
            var response = client.GetStringAsync("https://courier.yandex.ru/vrs/api/v1/result/mvrp/60a75a07-b48dc458-6470b004-a286d0c").Result;

            // Десериализуем JSON
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(response);

            // Получаем ID источника
            string sourceId = jsonObject.result.id;

            // Проверяем, существует ли запись в Sources с данным ResponseId
            var source = _context.Sources.FirstOrDefault(s => s.ResponseId == sourceId);
            if (source == null)
            {
                // Если записи нет, создаём новую
                source = new Source
                {
                    ResponseId = sourceId // Сохраняем ResponseId
                };
                _context.Sources.Add(source);

                // Добавляем новые машины
                var vehicles = jsonObject.result.vehicles;
                foreach (var vehicle in vehicles)
                {
                    var vehicleName = (string)vehicle.@ref;

                    // Добавляем машину в базу только если такой машины ещё нет
                    if (!_context.Vehicles.Any(v => v.Name == vehicleName))
                    {
                        _context.Vehicles.Add(new Vehicle
                        {
                            Name = vehicleName // Только имя, Id будет сгенерирован автоматически
                        });
                    }
                }

                // Добавляем маршруты
                var routes = jsonObject.result.routes;
                foreach (var route in routes)
                {
                    var vehicleId = (uint)route.vehicle_id;
                    var points = ((IEnumerable<dynamic>)route.route)
                        .Select(point => (string)point.node.value.@ref)
                        .ToList();

                    // Проверяем наличие маршрута
                    var existingRoutes = _context.Routes
                        .Where(r => r.VehicleId == vehicleId)
                        .AsEnumerable(); // Переносим данные на клиент для сравнения

                    if (!existingRoutes.Any(r => r.Points.SequenceEqual(points)))
                    {
                        _context.Routes.Add(new Route
                        {
                            VehicleId = vehicleId,
                            Points = points // Точки маршрута
                        });
                    }
                }

                // Сохраняем все изменения в базе
                _context.SaveChanges();
            }

            return _context; // Возвращаем актуальный контекст
        }
    }
}
