using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEgitim.AdminAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "PurchasedCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasedCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasedCourses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCourses_CourseId",
                table: "PurchasedCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedCourses_UserId",
                table: "PurchasedCourses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedCourses");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Courses");
        }
    }
}
