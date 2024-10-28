using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination_System.Migrations
{
    /// <inheritdoc />
    public partial class intial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_Question_Id1",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_Question_Id1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Question_Id1",
                table: "Answers");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Question_Id",
                table: "Answers",
                column: "Question_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_Question_Id",
                table: "Answers",
                column: "Question_Id",
                principalTable: "Questions",
                principalColumn: "Question_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_Question_Id",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_Question_Id",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "Question_Id1",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Question_Id1",
                table: "Answers",
                column: "Question_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_Question_Id1",
                table: "Answers",
                column: "Question_Id1",
                principalTable: "Questions",
                principalColumn: "Question_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
