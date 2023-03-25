using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Common.Models
{
    public class Cocktail : IDrink
    {
        [JsonProperty("idDrink")]
        public int Id { get; set; }

        [JsonProperty("strDrink")]
        public string Name { get; set; }

        [JsonProperty("strDrinkThumb")]
        public string Image { get; set; }
        public string Description { get; set; }
        public string StrIngredient1 { get; set; }
        public string StrIngredient2 { get; set; }
        public string StrIngredient3 { get; set; }
        public string StrIngredient4 { get; set; }
        public string StrIngredient5 { get; set; }

        public void GetDescription()
        {
            var ingredients = new List<string>() { StrIngredient1, StrIngredient2, StrIngredient3, StrIngredient4, StrIngredient5 };
            this.Description = string.Join(", ", ingredients.Where(i => i is not null));
        }
    }
}
