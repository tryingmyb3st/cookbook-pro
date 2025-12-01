using CookbookDB.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookDB;

public partial class CookbookDbContext : DbContext
{
    public CookbookDbContext()
    {
    }

    public CookbookDbContext(DbContextOptions<CookbookDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Укажите вашу строку подключения к PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Database=cookbook;Username=cookbook_user;Password=cookbook_user");
        }
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<ListRecipe> ListRecipes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_pk");

            entity.ToTable("list", "cookbook", tb => tb.HasComment("списки рецептов пользователей"));

            entity.Property(e => e.Id)
                .HasComment("идентификатор списка")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasComment("описание")
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasComment("название списка")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UserId)
                .HasComment("идентификатор пользователя")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Lists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("list_user_id_fk");
        });

        modelBuilder.Entity<ListRecipe>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("list_recipes", "cookbook", tb => tb.HasComment("рецепты в списках"));

            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_ingredients_recipe_id_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user", "cookbook", tb => tb.HasComment("пользователи"));

            entity.Property(e => e.Id)
                .HasComment("идентификатор пользователя")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasComment("имя пользователя")
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
