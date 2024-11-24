using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.EmmaCabCompany.Migrations
{
    /// <inheritdoc />
    public partial class AddCabsToMenuAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Menu_Customer",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CabId",
                table: "Cabs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cabs_CabId",
                table: "Cabs",
                column: "CabId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cabs_Menu_CabId",
                table: "Cabs",
                column: "CabId",
                principalTable: "Menu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Menu_CustomerId",
                table: "Customers",
                column: "CustomerId",
                principalTable: "Menu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cabs_Menu_CabId",
                table: "Cabs");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Menu_CustomerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Cabs_CabId",
                table: "Cabs");

            migrationBuilder.DropColumn(
                name: "CabId",
                table: "Cabs");

            migrationBuilder.AddColumn<int>(
                name: "Customer",
                table: "Customers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Customer",
                table: "Customers",
                column: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Menu_Customer",
                table: "Customers",
                column: "Customer",
                principalTable: "Menu",
                principalColumn: "Id");
        }
    }
}
