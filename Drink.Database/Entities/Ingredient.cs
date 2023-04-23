using Drink.Common.DTOs;
using Drink.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Entities
{
    public class Ingredient : IngredientDTO, IEntity
    {
        public ICollection<Dish> Dishes { get; set; }
    }
}
