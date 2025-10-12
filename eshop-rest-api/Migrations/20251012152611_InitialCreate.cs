using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eshop_rest_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImgUri = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { 101, "D0", "https://placeholder.com/0", "P0", 58.15m },
                    { 102, "D1", "https://placeholder.com/1", "P1", 82.37m },
                    { 103, "D2", "https://placeholder.com/2", "P2", 95.81m },
                    { 104, "D3", "https://placeholder.com/3", "P3", 82.40m },
                    { 105, "D4", "https://placeholder.com/4", "P4", 87.63m },
                    { 106, "D5", "https://placeholder.com/5", "P5", 23.55m },
                    { 107, "D6", "https://placeholder.com/6", "P6", 6.42m },
                    { 108, "D7", "https://placeholder.com/7", "P7", 6.92m },
                    { 109, "D8", "https://placeholder.com/8", "P8", 79.28m },
                    { 110, "D9", "https://placeholder.com/9", "P9", 91.53m },
                    { 111, "D10", "https://placeholder.com/10", "P10", 87.65m },
                    { 112, "D11", "https://placeholder.com/11", "P11", 27.14m },
                    { 113, "D12", "https://placeholder.com/12", "P12", 45.33m },
                    { 114, "D13", "https://placeholder.com/13", "P13", 38.63m },
                    { 115, "D14", "https://placeholder.com/14", "P14", 96.95m },
                    { 116, "D15", "https://placeholder.com/15", "P15", 92.34m },
                    { 117, "D16", "https://placeholder.com/16", "P16", 13.02m },
                    { 118, "D17", "https://placeholder.com/17", "P17", 92.24m },
                    { 119, "D18", "https://placeholder.com/18", "P18", 11.17m },
                    { 120, "D19", "https://placeholder.com/19", "P19", 64.34m },
                    { 121, "D20", "https://placeholder.com/20", "P20", 28.44m },
                    { 122, "D21", "https://placeholder.com/21", "P21", 76.32m },
                    { 123, "D22", "https://placeholder.com/22", "P22", 86.46m },
                    { 124, "D23", "https://placeholder.com/23", "P23", 35.84m },
                    { 125, "D24", "https://placeholder.com/24", "P24", 50.07m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
