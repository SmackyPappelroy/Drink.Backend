namespace Drink.Database.Contexts;

public class DishDrinkContext : DbContext
{
    public virtual DbSet<Dish> Dish { get; set; } = null!;
    public virtual DbSet<DishDrink> DishDrink { get; set; } = null!;
    public virtual DbSet<Entities.Drink> Drink { get; set; } = null!;

    public DishDrinkContext(DbContextOptions<DishDrinkContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* Composit Keys */
        modelBuilder.Entity<DishDrink>().HasKey(ci => new { ci.DishId, ci.DrinkId });

        /* Configuring related tables for the Dish table*/
        modelBuilder.Entity<Dish>(entity =>
        {


            // Configure a many-to-many realtionship between genres
            // and films using the DishDrink connection entity.
            entity.HasMany(d => d.Drinks)
                  .WithMany(p => p.Dishes)
                  .UsingEntity<DishDrink>()
                  // Specify the table name for the connection table
                  // to avoid duplicate tables (DishDrink  and DishDrinks)
                  // in the database.
                  .ToTable("DishDrink");
        });

        modelBuilder.Entity<Dish>().HasData(
            new
            {
                Id = 1,
                Name = "Roasted Chicken",
                Ingredients = "1 (3 pound) whole chicken, giblets removed\r\n\r\nsalt and black pepper to taste\r\n\r\n1 tablespoon onion powder, or to taste\r\n\r\n½ cup butter or margarine\r\n\r\n1 stalk celery, leaves removed",
                WarmDish = true,
                ColdDish = false,
                ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fthestayathomechef.com%2Froast-chicken%2F&psig=AOvVaw3svEy3FrAyhs9j9blN6_r-&ust=1679223194025000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCMiN9YKo5f0CFQAAAAAdAAAAABBp"
            },
               new
               {
                   Id = 2,
                   Name = "Salmon Sushi Nigri",
                   Ingredients = "1 1/2 cups (320 g) Calrose rice (sushi rice)\r\n1 3/4 cups (430 ml) water\r\n1 tsp salt\r\n3 tbsp (45 ml) rice vinegar\r\n1 tbsp sugar\r\n1 sushi-grade skinless salmon steak, about 1 lb (450 g) (see note)\r\n1 tsp (5 ml) wasabi\r\nSoy sauce for sushi and sashimi, to taste\r\nPickled ginger, to taste",
                   WarmDish = false,
                   ColdDish = true,
                   ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.manusmenu.com%2Fsalmon-nigiri&psig=AOvVaw26KB-QIsXLl1S5JUMCh03w&ust=1679223165863000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCPjEsfWn5f0CFQAAAAAdAAAAABAD"
               },
               new
               {
                   Id = 3,
                   Name = "Rib Eye Steak",
                   Ingredients = "2 rib-eye steaks, each about 200g and 2cm thick\r\n1tbsp sunflower oil\r\n1 tbsp/25g butter\r\n1 garlic clove, left whole but bashed once\r\nthyme, optional",
                   WarmDish = true,
                   ColdDish = false,
                   ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.wholesomeyum.com%2Fribeye-steak%2F&psig=AOvVaw3aThhC2AKgk7yNNRvUbMBY&ust=1679223089679000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCOjx69Sn5f0CFQAAAAAdAAAAABA_"
               },
               new
               {
                   Id = 4,
                   Name = "Danish pastry",
                   Ingredients = "2 cups unsalted butter, softened\r\n\r\n⅔ cup all-purpose flour\r\n\r\n8 cups all-purpose flour, divided\r\n\r\n4 ½ teaspoons active dry yeast\r\n\r\n2 ½ cups milk\r\n\r\n½ cup white sugar\r\n\r\n2 teaspoons salt\r\n\r\n2 large eggs\r\n\r\n1 teaspoon lemon extract\r\n\r\n1 teaspoon almond extract\r\n\r\n2 cups fruit preserves, any flavor\r\n\r\n1 large egg white, beaten",
                   WarmDish = false,
                   ColdDish = true,
                   ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.godare.se%2Frecept%2Fa%2F28a1aq%2Fwienerbrodets-dag--fira-med-hembakade-wienerbrod&psig=AOvVaw0WOqQSEc9GyAOl0OvyRUUX&ust=1679223026777000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCODQ0rKn5f0CFQAAAAAdAAAAABAD"
               });

        modelBuilder.Entity<DishDrink>().HasData(
            new DishDrink { DishId = 1, DrinkId = 1 },
            new DishDrink { DishId = 1, DrinkId = 2 },
            new DishDrink { DishId = 1, DrinkId = 4 },
            new DishDrink { DishId = 2, DrinkId = 3 },
            new DishDrink { DishId = 3, DrinkId = 1 },
            new DishDrink { DishId = 3, DrinkId = 2 },
            new DishDrink { DishId = 3, DrinkId = 4 },
            new DishDrink { DishId = 4, DrinkId = 5 },
            new DishDrink { DishId = 4, DrinkId = 6 }
            );

        modelBuilder.Entity<Entities.Drink>().HasData(
            new
            {
                Id = 1,
                Name = "Red Burgundy Wine",
                HotDrink = false,
                ColdDrink = true,
                ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fcnaluxury.channelnewsasia.com%2Fexperiences%2Fburgundy-wines-prices-200641&psig=AOvVaw0UUbIiqMiN5U5cHrBqXAK8&ust=1679223263396000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCOiCl6ao5f0CFQAAAAAdAAAAABAn"
            },
               new
               {
                   Id = 2,
                   Name = "White Burgundy Wine",
                   HotDrink = false,
                   ColdDrink = true,
                   ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.commonwealthwineschool.com%2Fshop%2Fp%2Fwhite-burgundy-celebration-june-9&psig=AOvVaw3kR-gpMBkvLo7tP3Xcubnq&ust=1679223341571000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCPC5-cmo5f0CFQAAAAAdAAAAABAD"
               },
               new
               {
                   Id = 3,
                   Name = "Sake",
                   HotDrink = false,
                   ColdDrink = true,
                   ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fmatadornetwork.com%2Fread%2Fsake-etiquette-japan%2F&psig=AOvVaw2Lj2xLonQu0yal1CrVNQjU&ust=1679223361624000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCMinoNOo5f0CFQAAAAAdAAAAABAD"
               },
                new
                {
                    Id = 4,
                    Name = "Beer",
                    HotDrink = false,
                    ColdDrink = true,
                    ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.liquor.com%2Fbest-german-beers-5089460&psig=AOvVaw2ozoT_8gxxaI0lmWrP5WLE&ust=1679223423998000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCIipgfKo5f0CFQAAAAAdAAAAABAg"
                },
                 new
                 {
                     Id = 5,
                     Name = "Tea",
                     HotDrink = true,
                     ColdDrink = false,
                     ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.medicalnewstoday.com%2Farticles%2F324771&psig=AOvVaw0QcuMfuZA3JbH2Huhgcxcr&ust=1679223478959000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCMi9wYup5f0CFQAAAAAdAAAAABAD"
                 },
                  new
                  {
                      Id = 6,
                      Name = "Coffe",
                      HotDrink = true,
                      ColdDrink = false,
                      ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.roastycoffee.com%2Fcold-coffee-vs-hot-coffee%2F&psig=AOvVaw20cwPYbtY6zbMr6FVc1JFC&ust=1679223505077000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCJCz75ap5f0CFQAAAAAdAAAAABAJ"
                  });
    }
}
