using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Entities
{
    public class DishDishType : IReferenceEntity
    {
        public int DishId { get; set; }
        public int DishTypeId { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual DishType DishType { get; set; }
    }
}
