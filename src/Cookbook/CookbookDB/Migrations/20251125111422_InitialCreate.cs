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
        CREATE SCHEMA IF NOT EXISTS cookbook;

        create sequence if not exists cookbook.ingredients_id_seq as integer;
        create sequence if not exists cookbook.ingredients_name_seq as integer;

        alter sequence cookbook.ingredients_id_seq owner to cookbook_user;
        alter sequence cookbook.ingredients_name_seq owner to cookbook_user;

        create table if not exists cookbook.recipe
        (
            id              serial
                constraint recipe_pk
                    primary key,
            name            varchar,
            weight          numeric,
            servings_number integer,
            instruction     varchar
        );

        comment on table cookbook.recipe is 'рецепты';
        comment on column cookbook.recipe.id is 'идентификатор рецепта';
        comment on column cookbook.recipe.name is 'название рецепта';
        comment on column cookbook.recipe.weight is 'вес всего блюда';
        comment on column cookbook.recipe.servings_number is 'количество порций';
        comment on column cookbook.recipe.instruction is 'инструкция приготовления';

        alter table cookbook.recipe owner to cookbook_user;

        create table if not exists cookbook.ingredient
        (
            id       integer default nextval('cookbook.ingredients_id_seq'::regclass) not null
                constraint ingredient_pk
                    primary key,
            name     varchar default nextval('cookbook.ingredients_name_seq'::regclass) not null,
            protein  numeric,
            fats     numeric,
            carbs    numeric,
            calories numeric
        );

        comment on table cookbook.ingredient is 'ингредиенты';
        comment on column cookbook.ingredient.id is 'идентификатор ингредиента';
        comment on column cookbook.ingredient.name is 'название ингредиента';
        comment on column cookbook.ingredient.protein is 'белки';
        comment on column cookbook.ingredient.fats is 'жиры';
        comment on column cookbook.ingredient.carbs is 'углеводы';
        comment on column cookbook.ingredient.calories is 'ккал';

        alter table cookbook.ingredient owner to cookbook_user;

        alter sequence cookbook.ingredients_id_seq owned by cookbook.ingredient.id;
        alter sequence cookbook.ingredients_name_seq owned by cookbook.ingredient.name;

        create table if not exists cookbook.recipe_ingredients
        (
            recipe_id     integer not null
                constraint recipe_ingredients_recipe_id_fk
                    references cookbook.recipe
                    on delete cascade,
            ingredient_id integer not null
                constraint recipe_ingredients_ingredient_id_fk
                    references cookbook.ingredient,
            weight        numeric,
            constraint recipe_ingredients_pk
                primary key (recipe_id, ingredient_id)
        );

        comment on table cookbook.recipe_ingredients is 'ингредиенты, использованные в рецептах';
        alter table cookbook.recipe_ingredients owner to cookbook_user;

        create table if not exists cookbook.user
        (
            id   serial
                constraint user_pk
                    primary key,
            name varchar not null
        );

        comment on table cookbook.user is 'пользователи';
        comment on column cookbook.user.id is 'идентификатор пользователя';
        comment on column cookbook.user.name is 'имя пользователя';
        alter table cookbook.user owner to cookbook_user;

        create table if not exists cookbook.list
        (
            id          serial
                constraint list_pk
                    primary key,
            name        varchar,
            user_id     integer
                constraint list_user_id_fk
                    references cookbook.user,
            description varchar
        );

        comment on table cookbook.list is 'списки рецептов пользователей';
        comment on column cookbook.list.id is 'идентификатор списка';
        comment on column cookbook.list.name is 'название списка';
        comment on column cookbook.list.user_id is 'идентификатор пользователя';
        comment on column cookbook.list.description is 'описание';
        alter table cookbook.list owner to cookbook_user;

        create table if not exists cookbook.list_recipes
        (
            list_id   integer not null,
            recipe_id integer not null
        );

        comment on table cookbook.list_recipes is 'рецепты в списках';
        alter table cookbook.list_recipes owner to cookbook_user;
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
