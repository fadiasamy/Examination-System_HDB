using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination_System.Migrations
{
    /// <inheritdoc />
    public partial class ExamResultupdatedtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Exams_Exam_Id",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "Exam_Id",
                table: "ExamResults",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamResults_Exam_Id",
                table: "ExamResults",
                newName: "IX_ExamResults_ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Exams_ExamId",
                table: "ExamResults",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Exam_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Exams_ExamId",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "ExamResults",
                newName: "Exam_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                newName: "IX_ExamResults_Exam_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Exams_Exam_Id",
                table: "ExamResults",
                column: "Exam_Id",
                principalTable: "Exams",
                principalColumn: "Exam_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
