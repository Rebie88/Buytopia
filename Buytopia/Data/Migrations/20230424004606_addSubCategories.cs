using Microsoft.EntityFrameworkCore.Migrations;

namespace Buytopia.Data.Migrations
{
    public partial class addSubCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Category_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories");

            migrationBuilder.RenameTable(
                name: "SubCategories",
                newName: "SubCategorie");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategorie",
                newName: "IX_SubCategorie_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategorie",
                table: "SubCategorie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorie_Category_CategoryId",
                table: "SubCategorie",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorie_Category_CategoryId",
                table: "SubCategorie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategorie",
                table: "SubCategorie");

            migrationBuilder.RenameTable(
                name: "SubCategorie",
                newName: "SubCategories");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategorie_CategoryId",
                table: "SubCategories",
                newName: "IX_SubCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Category_CategoryId",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
