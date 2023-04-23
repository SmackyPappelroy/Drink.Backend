using Drink.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Common.ExternalModels
{
    public class FineDish
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Servings { get; set; }
        public int ReadyInMinutes { get; set; }
        public bool Cheap { get; set; }
        public IEnumerable<string> Cuisines { get; set; }
        public bool DairyFree { get; set; }
        public bool GlutenFree { get; set; }
        public bool Ketogenic { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool VeryHealthy { get; set; }
        public IEnumerable<string> DishTypes { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }
        public IEnumerable<DrinkDTO> Drinks { get; set; }
    }
}
