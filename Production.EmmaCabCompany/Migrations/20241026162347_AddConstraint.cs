using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.EmmaCabCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cabs_Fleet_Id",
                table: "Cabs");

            migrationBuilder.DropColumn(
                name: "FleetId",
                table: "Cabs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cabs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Cab",
                table: "Cabs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cabs_Cab",
                table: "Cabs",
                column: "Cab");

            migrationBuilder.AddForeignKey(
                name: "FK_Cabs_Fleet_Cab",
                table: "Cabs",
                column: "Cab",
                principalTable: "Fleet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cabs_Fleet_Cab",
                table: "Cabs");

            migrationBuilder.DropIndex(
                name: "IX_Cabs_Cab",
                table: "Cabs");

            migrationBuilder.DropColumn(
                name: "Cab",
                table: "Cabs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cabs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "FleetId",
                table: "Cabs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Cabs_Fleet_Id",
                table: "Cabs",
                column: "Id",
                principalTable: "Fleet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
