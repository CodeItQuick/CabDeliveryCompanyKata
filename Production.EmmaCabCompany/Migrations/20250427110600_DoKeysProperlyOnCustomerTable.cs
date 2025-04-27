using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.EmmaCabCompany.Migrations
{
    /// <inheritdoc />
    public partial class DoKeysProperlyOnCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CabDrivers");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CabDrivers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CabDrivers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CabDrivers",
                type: "INTEGER",
                nullable: true);
        }
    }
}
