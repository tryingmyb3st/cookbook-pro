using CookbookDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookbookDB;

public partial class CookbookDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public CookbookDbContext()
    {
    }

    public CookbookDbContext(DbContextOptions<CookbookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredient_pk");

            entity.ToTable("ingredient", "cookbook", tb => tb.HasComment("ингредиенты"));

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('cookbook.ingredients_id_seq'::regclass)")
                .HasComment("идентификатор ингредиента")
                .HasColumnName("id");
            entity.Property(e => e.Calories)
                .HasComment("ккал")
                .HasColumnName("calories");
            entity.Property(e => e.Carbs)
                .HasComment("углеводы")
                .HasColumnName("carbs");
            entity.Property(e => e.Fats)
                .HasComment("жиры")
                .HasColumnName("fats");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("nextval('cookbook.ingredients_name_seq'::regclass)")
                .HasComment("название ингредиента")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Protein)
                .HasComment("белки")
                .HasColumnName("protein");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recipe_pk");

            entity.ToTable("recipe", "cookbook", tb => tb.HasComment("рецепты"));

            entity.Property(e => e.Id)
                .HasComment("идентификатор рецепта")
                .HasColumnName("id");
            entity.Property(e => e.Instruction)
                .HasComment("инструкция приготовления")
                .HasColumnType("character varying")
                .HasColumnName("instruction");
            entity.Property(e => e.Name)
                .HasComment("название рецепта")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ServingsNumber)
                .HasComment("количество порций")
                .HasColumnName("servings_number");
            entity.Property(e => e.Weight)
                .HasComment("вес всего блюда")
                .HasColumnName("weight");
            entity.Property(e => e.FileName)
               .HasComment("имя файла")
               .HasColumnType("character varying")
               .HasColumnName("file_name");
            entity.Property(e => e.UserId)
                .HasComment("идентификатор пользователя")
                .HasColumnName("UserId");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId }).HasName("recipe_ingredients_pk");

            entity.ToTable("recipe_ingredients", "cookbook", tb => tb.HasComment("ингредиенты, использованные в рецептах"));

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_ingredients_ingredient_id_fk");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("recipe_ingredients_recipe_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
