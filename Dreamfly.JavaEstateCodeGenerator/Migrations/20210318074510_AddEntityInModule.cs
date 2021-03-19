using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class AddEntityInModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RelateEntityInModule",
                table: "EntityItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelateEntityInModule",
                table: "EntityItem");
        }
    }
}
