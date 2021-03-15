using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class ChangeEntityRelate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRelateSelf",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "NeedForeignKey",
                table: "EntityItem");

            migrationBuilder.RenameColumn(
                name: "RelateEntityName",
                table: "EntityItem",
                newName: "RelateEntity");

            migrationBuilder.AddColumn<string>(
                name: "CascadeType",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinName",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelateDirection",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CascadeType",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "JoinName",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "RelateDirection",
                table: "EntityItem");

            migrationBuilder.RenameColumn(
                name: "RelateEntity",
                table: "EntityItem",
                newName: "RelateEntityName");

            migrationBuilder.AddColumn<bool>(
                name: "IsRelateSelf",
                table: "EntityItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedForeignKey",
                table: "EntityItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
