using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class add_hasAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAudit",
                table: "Entity",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"UPDATE Entity SET HasAudit=1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAudit",
                table: "Entity");
        }
    }
}
