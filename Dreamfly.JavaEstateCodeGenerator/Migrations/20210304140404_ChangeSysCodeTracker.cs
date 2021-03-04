using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class ChangeSysCodeTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CodeTracks",
                newName: "Values");

            migrationBuilder.RenameColumn(
                name: "LastUpDateTime",
                table: "CodeTracks",
                newName: "Name");

            migrationBuilder.AlterColumn<long>(
                name: "SysCodeId",
                table: "CodeTracks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Values",
                table: "CodeTracks",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CodeTracks",
                newName: "LastUpDateTime");

            migrationBuilder.AlterColumn<long>(
                name: "SysCodeId",
                table: "CodeTracks",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }
    }
}
