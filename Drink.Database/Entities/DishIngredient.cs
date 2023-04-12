using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Entities
{
    public class DishIngredient : IReferenceEntity
    {
        public int DishId { get; set; }
        public int IngredientId { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
