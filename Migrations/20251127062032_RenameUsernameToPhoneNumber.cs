using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_marketplace_api_3tier.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsernameToPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
          name: "Username",
          table: "Users",
          newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
           name: "PhoneNumber",
           table: "Users",
           newName: "Username");
        }
    }
}
