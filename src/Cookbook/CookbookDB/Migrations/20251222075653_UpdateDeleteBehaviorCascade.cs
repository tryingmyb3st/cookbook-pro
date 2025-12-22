using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_AspNetUsers_UserId",
                schema: "cookbook",
                table: "ingredient");

            migrationBuilder.DropForeignKey(
                name: "recipe_ingredients_recipe_id_fk",
                schema: "cookbook",
                table: "recipe_ingredients");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_UserId",
                schema: "cookbook",
                table: "ingredient");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "cookbook",
                table: "recipe",
                type: "bigint",
                nullable: false,
                comment: "идентификатор пользователя",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "recipe_ingredients_recipe_id_fk",
                schema: "cookbook",
                table: "recipe_ingredients",
                column: "recipe_id",
                principalSchema: "cookbook",
                principalTable: "recipe",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "recipe_ingredients_recipe_id_fk",
                schema: "cookbook",
                table: "recipe_ingredients");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "cookbook",
                table: "recipe",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "идентификатор пользователя");

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
                name: "recipe_ingredients_recipe_id_fk",
                schema: "cookbook",
                table: "recipe_ingredients",
                column: "recipe_id",
                principalSchema: "cookbook",
                principalTable: "recipe",
                principalColumn: "id");
        }
    }
}
