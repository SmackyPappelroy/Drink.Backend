namespace Drink.Common.DTOs;

public class DishDTO
{   
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ingredients { get; set; }
    public bool WarmDish { get; set; }
    public bool ColdDish { get; set; }
    public string ImageUrl { get; set; }
}
