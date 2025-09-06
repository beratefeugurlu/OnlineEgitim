using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEgitim.AdminAPI.Migrations
{
    public partial class AddInstructorToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "Courses");
        }
    }
}
