using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Drink.Common.Models
{
    public class Beer : IDrink
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }

        [JsonProperty("image_url")]
        public string Image { get; set; }
    }
}
