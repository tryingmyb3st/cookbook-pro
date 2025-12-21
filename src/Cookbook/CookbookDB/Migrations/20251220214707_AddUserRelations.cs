using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "cookbook",
                table: "recipe",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "cookbook",
                table: "ingredient",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_recipe_UserId",
                schema: "cookbook",
                table: "recipe",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_UserId",
                schema: "cookbook",
                table: "ingredient",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_AspNetUsers_UserId",
                schema: "cookbook",
                table: "ingredient",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipe_AspNetUsers_UserId",
                schema: "cookbook",
                table: "recipe",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_AspNetUsers_UserId",
                schema: "cookbook",
                table: "ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_recipe_AspNetUsers_UserId",
                schema: "cookbook",
                table: "recipe");

            migrationBuilder.DropIndex(
                name: "IX_recipe_UserId",
                schema: "cookbook",
                table: "recipe");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_UserId",
                schema: "cookbook",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "cookbook",
                table: "recipe");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "cookbook",
                table: "ingredient");
        }
    }
}
