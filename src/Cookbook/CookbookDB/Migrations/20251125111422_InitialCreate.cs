using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CookbookDB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create sequence if not exists ingredients_id_seq
                    as integer;

                alter sequence ingredients_id_seq owner to cookbook_user;

                create sequence if not exists ingredients_name_seq
                    as integer;

                alter sequence ingredients_name_seq owner to cookbook_user;

                create table if not exists recipe
                (
                    id              serial
                        constraint recipe_pk
                            primary key,
                    name            varchar,
                    weight          numeric,
                    servings_number integer,
                    instruction     varchar
                );

                comment on table recipe is 'рецепты';
                comment on column recipe.id is 'идентификатор рецепта';
                comment on column recipe.name is 'название рецепта';
                comment on column recipe.weight is 'вес всего блюда';
                comment on column recipe.servings_number is 'количество порций';
                comment on column recipe.instruction is 'инструкция приготовления';

                alter table recipe
                    owner to cookbook_user;

                create table if not exists ingredient
                (
                    id       integer default nextval('cookbook.ingredients_id_seq'::regclass)   not null
                        constraint ingredient_pk
                            primary key,
                    name     varchar default nextval('cookbook.ingredients_name_seq'::regclass) not null,
                    protein  numeric,
                    fats     numeric,
                    carbs    numeric,
                    calories numeric
                );

                comment on table ingredient is 'ингредиенты';
                comment on column ingredient.id is 'идентификатор ингредиента';
                comment on column ingredient.name is 'название ингредиента';
                comment on column ingredient.protein is 'белки';
                comment on column ingredient.fats is 'жиры';
                comment on column ingredient.carbs is 'углеводы';
                comment on column ingredient.calories is 'ккал';

                alter table ingredient
                    owner to cookbook_user;

                alter sequence ingredients_id_seq owned by ingredient.id;
                alter sequence ingredients_name_seq owned by ingredient.name;

                create table if not exists recipe_ingredients
                (
                    recipe_id     integer not null
                        constraint recipe_ingredients_recipe_id_fk
                            references recipe
                            on delete cascade,
                    ingredient_id integer not null
                        constraint recipe_ingredients_ingredient_id_fk
                            references ingredient,
                    weight        numeric,
                    constraint recipe_ingredients_pk
                        primary key (recipe_id, ingredient_id)
                );

                comment on table recipe_ingredients is 'ингредиенты, использованные в рецептах';

                alter table recipe_ingredients
                    owner to cookbook_user;

                create table if not exists ""user""
                (
                    id   serial
                        constraint user_pk
                            primary key,
                    name varchar not null
                );

                comment on table ""user"" is 'пользователи';
                comment on column ""user"".id is 'идентификатор пользователя';
                comment on column ""user"".name is 'имя пользователя';

                alter table ""user""
                    owner to cookbook_user;

                create table if not exists list
                (
                    id          serial
                        constraint list_pk
                            primary key,
                    name        varchar,
                    user_id     integer
                        constraint list_user_id_fk
                            references ""user"",
                    description varchar
                );

                comment on table list is 'списки рецептов пользователей';
                comment on column list.id is 'идентификатор списка';
                comment on column list.name is 'название списка';
                comment on column list.user_id is 'идентификатор пользователя';
                comment on column list.description is 'описание';

                alter table list
                    owner to cookbook_user;

                create table if not exists list_recipes
                (
                    list_id   integer not null,
                    recipe_id integer not null
                );

                comment on table list_recipes is 'рецепты в списках';

                alter table list_recipes
                    owner to cookbook_user;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "list",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "list_recipes",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "recipe_ingredients",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "user",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "ingredient",
                schema: "cookbook");

            migrationBuilder.DropTable(
                name: "recipe",
                schema: "cookbook");
        }
    }
}
