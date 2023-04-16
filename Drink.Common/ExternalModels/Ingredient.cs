using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Common.Models
{
    public class Ingredient
    {
        [JsonProperty("Id")]
        public int SecondaryId { get; set; }
        public string Original { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
