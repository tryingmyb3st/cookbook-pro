using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class FixNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "cookbook",
                table: "recipe",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                comment: "название рецепта",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true,
                oldComment: "название рецепта");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "cookbook",
                table: "recipe",
                type: "character varying",
                nullable: true,
                comment: "название рецепта",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldComment: "название рецепта");
        }
    }
}
