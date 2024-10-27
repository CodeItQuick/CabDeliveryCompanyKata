using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.EmmaCabCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddFleetId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FleetId",
                table: "Cabs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FleetId",
                table: "Cabs");
        }
    }
}
