using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_marketplace_api_3tier
{
    [Table("Cars")] // Maps the class to the "Cars" table in the database
    public class Car
    {
        [Key] // Defines 'Id' as the primary key
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}