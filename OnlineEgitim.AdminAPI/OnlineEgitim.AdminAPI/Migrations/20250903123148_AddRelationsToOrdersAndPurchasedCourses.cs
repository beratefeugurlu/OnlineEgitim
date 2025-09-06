using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEgitim.AdminAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsToOrdersAndPurchasedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ Sadece Orders tablosuna TotalPrice ekliyoruz
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");
        }
    }
}
