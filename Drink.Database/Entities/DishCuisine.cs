using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Entities
{
    public class DishCuisine : IReferenceEntity
    {
        public int DishId { get; set; }
        public int CuisineId { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual Cuisine Cuisine { get; set; }
    }
}
