using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_marketplace_api_3tier.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeededUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PhoneNumber" },
                values: new object[] { "456", "newowner" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PhoneNumber" },
                values: new object[] { "789", "newcustomer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PhoneNumber" },
                values: new object[] { "123", "owner" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PhoneNumber" },
                values: new object[] { "123", "customer" });
        }
    }
}
