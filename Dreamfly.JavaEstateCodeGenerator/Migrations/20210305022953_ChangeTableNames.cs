using Microsoft.EntityFrameworkCore.Migrations;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    public partial class ChangeTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityItems_Entities_EntityId",
                table: "EntityItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityItems",
                table: "EntityItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entities",
                table: "Entities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeTracks",
                table: "CodeTracks");

            migrationBuilder.RenameTable(
                name: "EntityItems",
                newName: "EntityItem");

            migrationBuilder.RenameTable(
                name: "Entities",
                newName: "Entity");

            migrationBuilder.RenameTable(
                name: "CodeTracks",
                newName: "CodeTrack");

            migrationBuilder.RenameIndex(
                name: "IX_EntityItems_EntityId",
                table: "EntityItem",
                newName: "IX_EntityItem_EntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityItem",
                table: "EntityItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entity",
                table: "Entity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeTrack",
                table: "CodeTrack",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityItem_Entity_EntityId",
                table: "EntityItem",
                column: "EntityId",
                principalTable: "Entity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityItem_Entity_EntityId",
                table: "EntityItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityItem",
                table: "EntityItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entity",
                table: "Entity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeTrack",
                table: "CodeTrack");

            migrationBuilder.RenameTable(
                name: "EntityItem",
                newName: "EntityItems");

            migrationBuilder.RenameTable(
                name: "Entity",
                newName: "Entities");

            migrationBuilder.RenameTable(
                name: "CodeTrack",
                newName: "CodeTracks");

            migrationBuilder.RenameIndex(
                name: "IX_EntityItem_EntityId",
                table: "EntityItems",
                newName: "IX_EntityItems_EntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityItems",
                table: "EntityItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entities",
                table: "Entities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeTracks",
                table: "CodeTracks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityItems_Entities_EntityId",
                table: "EntityItems",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
