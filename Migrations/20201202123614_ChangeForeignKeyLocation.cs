using Microsoft.EntityFrameworkCore.Migrations;

namespace CsScore.Migrations
{
    public partial class ChangeForeignKeyLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Project_GroupProjectRef",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_GroupProjectRef",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "GroupProjectRef",
                table: "Group");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_GroupId",
                table: "Project",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Group_GroupId",
                table: "Project",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Group_GroupId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_GroupId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "GroupProjectRef",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupProjectRef",
                table: "Group",
                column: "GroupProjectRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Project_GroupProjectRef",
                table: "Group",
                column: "GroupProjectRef",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
