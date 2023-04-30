using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Drink.Database.Contexts;

public class DishDrinkContext : DbContext
{
    public DbSet<Cuisine> Cuisines => Set<Cuisine>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishCuisine> DishCuisines => Set<DishCuisine>();
    public DbSet<DishDishType> DishDishTypes => Set<DishDishType>();
    public DbSet<DishDrink> DishDrinks => Set<DishDrink>();
    public DbSet<DishType> DishTypes => Set<DishType>();
    public DbSet<Entities.Drink> Drinks => Set<Entities.Drink>();


    public DishDrinkContext(DbContextOptions<DishDrinkContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* Composite Keys */
        modelBuilder.Entity<DishDrink>().HasKey(ci => new { ci.DishId, ci.DrinkId });
        modelBuilder.Entity<DishDishType>().HasKey(dd => new { dd.DishId, dd.DishTypeId });
        modelBuilder.Entity<DishCuisine>().HasKey(dc => new { dc.DishId, dc.CuisineId });

        /* Configuring related tables for the Dish table*/
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasMany(d => d.Cuisines)
                  .WithMany(p => p.Dishes)
                  .UsingEntity<DishCuisine>()
                  .ToTable("DishCuisine");

            entity.HasMany(d => d.Drinks)
                  .WithMany(p => p.Dishes)
                  .UsingEntity<DishDrink>()
                  .ToTable("DishDrink");

            entity.HasMany(d => d.DishTypes)
                  .WithMany(p => p.Dishes)
                  .UsingEntity<DishDishType>()
                  .ToTable("DishDishType");
        });
    }
}
