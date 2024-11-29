using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YandexRoutes.Server.Models
{
    public class Vehicle
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)] public int Id { get; set; }
        [Required][MaxLength(255)] public string Name { get; set; }
    }

    public class Route
    {
        [Key] public int Id { get; set; }
        [ForeignKey("Vehicle")] public int VehicleId { get; set; }
        [Required] public List<string> Points { get; set; }
    }
}
