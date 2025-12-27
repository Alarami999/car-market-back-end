using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace car_marketplace_api_3tier.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Description", "ImageUrl", "Location", "Make", "Model", "Price", "Year" },
                values: new object[,]
                {
                    { 1, "A reliable and fuel-efficient sedan.", "https://example.com/toyota-corolla.jpg", "Riyadh", "Toyota", "Corolla", 15000m, 2020 },
                    { 2, "A sporty compact car with great handling.", "https://example.com/honda-civic.jpg", "Jeddah", "Honda", "Civic", 14000m, 2019 },
                    { 3, "A classic American muscle car.", "https://example.com/ford-mustang.jpg", "Dammam", "Ford", "Mustang", 30000m, 2021 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
