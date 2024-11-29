using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace YandexRoutes.Server.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Route> Routes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка конверсии для List<string> Points
            modelBuilder.Entity<Route>()
                .Property(r => r.Points)
                .HasConversion(
                    points => JsonConvert.SerializeObject(points),
                    points => JsonConvert.DeserializeObject<List<string>>(points));
        }
    }
}
