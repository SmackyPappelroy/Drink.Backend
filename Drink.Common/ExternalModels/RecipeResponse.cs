using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Common.Models
{
    public class RecipeResponse
    {
        public IEnumerable<Recipe> Recipes { get; set; }
    }
}
