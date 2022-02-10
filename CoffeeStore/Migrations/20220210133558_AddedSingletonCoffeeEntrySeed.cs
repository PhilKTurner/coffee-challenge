using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeStore.Migrations
{
    public partial class AddedSingletonCoffeeEntrySeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coffee",
                columns: new[] { "Id", "Inventory" },
                values: new object[] { 1, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coffee",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
