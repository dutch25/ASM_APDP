using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM_APDP.Migrations
{
    /// <inheritdoc />
    public partial class classidfr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassID",
                table: "Marks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_ClassID",
                table: "Marks",
                column: "ClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Classes_ClassID",
                table: "Marks",
                column: "ClassID",
                principalTable: "Classes",
                principalColumn: "ClassID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Classes_ClassID",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_ClassID",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "ClassID",
                table: "Marks");
        }
    }
}
