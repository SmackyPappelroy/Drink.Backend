using Drink.Common.DTOs;
using Drink.Database.Interface;

namespace Drink.Database.Entities;
public class Dish : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public int Servings { get; set; }
    public int ReadyInMinutes { get; set; }
    public bool Cheap { get; set; }
    public ICollection<Cuisine> Cuisines { get; set; }
    public bool DairyFree { get; set; }
    public bool GlutenFree { get; set; }
    public bool Ketogenic { get; set; }
    public bool Vegan { get; set; }
    public bool Vegetarian { get; set; }
    public bool VeryHealthy { get; set; }
    public ICollection<DishType> DishTypes { get; set; }
    public ICollection<Ingredient> ExtendedIngredients { get; set; }
    public string Instructions { get; set; }
    //TODO public string Ingredients { get; set; }
    public ICollection<Drink> Drinks { get; set; }
}
