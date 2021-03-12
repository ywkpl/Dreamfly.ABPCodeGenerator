using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class AddStringTypeLengthAndEntityItemAddRelate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyName",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "RelateEntityName",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelateType",
                table: "EntityItem",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyName",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "IsRelateSelf",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "NeedForeignKey",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "RelateEntityName",
                table: "EntityItem");

            migrationBuilder.DropColumn(
                name: "RelateType",
                table: "EntityItem");
        }
    }
}
