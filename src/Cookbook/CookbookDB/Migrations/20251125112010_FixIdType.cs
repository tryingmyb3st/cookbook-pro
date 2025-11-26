using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class FixIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "cookbook",
                table: "user",
                type: "bigint",
                nullable: false,
                comment: "идентификатор пользователя",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "идентификатор пользователя")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ingredient_id",
                schema: "cookbook",
                table: "recipe_ingredients",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "recipe_id",
                schema: "cookbook",
                table: "recipe_ingredients",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "cookbook",
                table: "recipe",
                type: "bigint",
                nullable: false,
                comment: "идентификатор рецепта",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "идентификатор рецепта")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "cookbook",
                table: "list",
                type: "bigint",
                nullable: true,
                comment: "идентификатор пользователя",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "идентификатор пользователя");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "cookbook",
                table: "list",
                type: "bigint",
                nullable: false,
                comment: "идентификатор списка",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "идентификатор списка")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "cookbook",
                table: "ingredient",
                type: "bigint",
                nullable: false,
                defaultValueSql: "nextval('cookbook.ingredients_id_seq'::regclass)",
                comment: "идентификатор ингредиента",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('cookbook.ingredients_id_seq'::regclass)",
                oldComment: "идентификатор ингредиента");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "cookbook",
                table: "user",
                type: "integer",
                nullable: false,
                comment: "идентификатор пользователя",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "идентификатор пользователя")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ingredient_id",
                schema: "cookbook",
                table: "recipe_ingredients",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                schema: "cookbook",
                table: "recipe_ingredients",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "cookbook",
                table: "recipe",
                type: "integer",
                nullable: false,
                comment: "идентификатор рецепта",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "идентификатор рецепта")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                schema: "cookbook",
                table: "list",
                type: "integer",
                nullable: true,
                comment: "идентификатор пользователя",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "идентификатор пользователя");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "cookbook",
                table: "list",
                type: "integer",
                nullable: false,
                comment: "идентификатор списка",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "идентификатор списка")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "cookbook",
                table: "ingredient",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('cookbook.ingredients_id_seq'::regclass)",
                comment: "идентификатор ингредиента",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "nextval('cookbook.ingredients_id_seq'::regclass)",
                oldComment: "идентификатор ингредиента");
        }
    }
}
