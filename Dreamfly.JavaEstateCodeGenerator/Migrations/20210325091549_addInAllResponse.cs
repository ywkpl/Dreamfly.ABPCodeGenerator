using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class addInAllResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InAllResponse",
                table: "EntityItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"UPDATE EntityItem SET InAllResponse=InResponse;
                                UPDATE EntityItem SET InResponse=0;");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE EntityItem SET InResponse=InAllResponse;");
            migrationBuilder.DropColumn(
                name: "InAllResponse",
                table: "EntityItem");
        }
    }
}
