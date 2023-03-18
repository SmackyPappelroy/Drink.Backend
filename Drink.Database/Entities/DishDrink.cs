using Drink.DataBase.Interface;

namespace Drink.Database.Entities;
public class DishDrink : IReferenceEntity
{
    public int DishId { get; set; }
    public int DrinkId { get; set; }

    public virtual Dish Dish { get; set; } = null!;
    public virtual Drink Drink { get; set; } = null!;
}
