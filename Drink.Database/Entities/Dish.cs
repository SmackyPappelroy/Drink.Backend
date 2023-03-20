using Drink.Database.Interface;

namespace Drink.Database.Entities;
public class Dish : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Ingredients { get; set; }
    public bool WarmDish { get; set;}
    public bool ColdDish { get; set; }
    public string ImageUrl { get; set; }
    public virtual ICollection<Drink>? Drinks { get; set; }
}
