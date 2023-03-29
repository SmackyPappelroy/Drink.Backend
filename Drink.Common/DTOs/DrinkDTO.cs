using Drink.Common.Models;

namespace Drink.Common.DTOs;

public class DrinkDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public DrinkType DrinkType { get; set; }
}
