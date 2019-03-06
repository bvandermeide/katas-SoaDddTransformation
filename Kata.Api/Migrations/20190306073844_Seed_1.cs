using Microsoft.EntityFrameworkCore.Migrations;

namespace Kata.Api.Migrations
{
  public partial class Seed_1 : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 1, 20d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 2, 30d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 3, 40d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 4, 50d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 5, 60d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 6, 70d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 7, 80d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 8, 90d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 9, 100d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 10, 110d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 11, 120d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 12, 130d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 13, 140d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 14, 150d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 15, 160d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 16, 170d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 17, 180d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 18, 190d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 19, 200d, 1 });
      migrationBuilder.InsertData("Products", new string[] { "ProductId", "SalePrice", "UoM" }, new object[] { 20, 210d, 1 });

    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}