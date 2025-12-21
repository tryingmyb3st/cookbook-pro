using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class DropTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "list",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "list_recipes",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "user",
                schema: "cookbook");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "list_recipes",
                schema: "cookbook",
                columns: table => new
                {
                    list_id = table.Column<int>(type: "integer", nullable: false),
                    recipe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                },
                comment: "рецепты в списках");

            migrationBuilder.CreateTable(
                name: "user",
                schema: "cookbook",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "идентификатор пользователя")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false, comment: "имя пользователя")
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.id);
                },
                comment: "пользователи");

            migrationBuilder.CreateTable(
                name: "list",
                schema: "cookbook",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "идентификатор списка")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true, comment: "идентификатор пользователя"),
                    description = table.Column<string>(type: "character varying", nullable: true, comment: "описание"),
                    name = table.Column<string>(type: "character varying", nullable: true, comment: "название списка")
                },
                constraints: table =>
                {
                    table.PrimaryKey("list_pk", x => x.id);
                    table.ForeignKey(
                        name: "list_user_id_fk",
                        column: x => x.user_id,
                        principalSchema: "cookbook",
                        principalTable: "user",
                        principalColumn: "id");
                },
                comment: "списки рецептов пользователей");

            migrationBuilder.CreateIndex(
                name: "IX_list_user_id",
                schema: "cookbook",
                table: "list",
                column: "user_id");
        }
    }
}
