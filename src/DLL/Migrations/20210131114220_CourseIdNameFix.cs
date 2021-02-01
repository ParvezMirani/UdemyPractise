using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class CourseIdNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Course",
                newName: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Course",
                newName: "CourseID");
        }
    }
}
