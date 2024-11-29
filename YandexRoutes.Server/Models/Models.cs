using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YandexRoutes.Server.Models
{
    public class Vehicle
    {
        [Key] // Устанавливаем Id как первичный ключ
        public long Id { get; set; }

        [Required] // Указываем, что поле Name обязательно
        [MaxLength(255)] // Ограничиваем длину строки
        public string Name { get; set; }
    }

    public class Route
    {
        [Key] // Устанавливаем Id как первичный ключ
        public long Id { get; set; }

        [ForeignKey("Vehicle")] // Устанавливаем внешний ключ на таблицу Vehicle
        public long VehicleId { get; set; }

        [Required] // Поле Points обязательно
        public List<string> Points { get; set; }
    }

    public class Source
    {
        [Key] // Устанавливаем Id как первичный ключ
        public long Id { get; set; }

        [Required] // Поле ResponseId обязательно
        [MaxLength(255)] // Ограничиваем длину строки
        public string ResponseId { get; set; }
    }
}
