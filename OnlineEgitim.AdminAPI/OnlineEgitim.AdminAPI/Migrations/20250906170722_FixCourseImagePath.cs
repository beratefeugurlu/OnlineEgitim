using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEgitim.AdminAPI.Migrations
{
    public partial class FixCourseImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Courses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Courses");
        }
    }
}
