using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Production.EmmaCabCompany.Migrations
{
    /// <inheritdoc />
    public partial class MigrateCustomerTableForMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Menu_MenuId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MenuId",
                table: "Customers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MenuId",
                table: "Customers",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Menu_MenuId",
                table: "Customers",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }
    }
}
