using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class addfraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "EntityItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Fraction",
                table: "EntityItems",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fraction",
                table: "EntityItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "EntityItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
