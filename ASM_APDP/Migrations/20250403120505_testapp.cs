using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM_APDP.Migrations
{
    /// <inheritdoc />
    public partial class testapp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_CourseID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Users_UserID",
                table: "Classes");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseID",
                table: "Classes",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Users_UserID",
                table: "Classes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_CourseID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Users_UserID",
                table: "Classes");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseID",
                table: "Classes",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Users_UserID",
                table: "Classes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
