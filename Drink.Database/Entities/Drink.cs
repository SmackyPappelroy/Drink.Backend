using Drink.Common.Models;
using Drink.Database.Interface;

namespace Drink.Database.Entities;
public class Drink : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public int DrinkType { get; set; }
    public ICollection<Dish> Dishes { get; set; }
}
