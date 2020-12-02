using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsScore.Migrations
{
    public partial class SubmitDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SubmitDate",
                table: "Score",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SubmitDate",
                table: "Project",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Project",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Project");
        }
    }
}
