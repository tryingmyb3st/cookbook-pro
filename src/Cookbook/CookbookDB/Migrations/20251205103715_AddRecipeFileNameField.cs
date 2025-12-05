using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeFileNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_name",
                schema: "cookbook",
                table: "recipe",
                type: "character varying",
                nullable: true,
                comment: "имя файла");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                schema: "cookbook",
                table: "recipe");
        }
    }
}
