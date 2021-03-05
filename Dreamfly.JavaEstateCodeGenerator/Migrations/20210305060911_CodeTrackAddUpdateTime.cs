using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class CodeTrackAddUpdateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "CodeTrack",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "CodeTrack");
        }
    }
}
