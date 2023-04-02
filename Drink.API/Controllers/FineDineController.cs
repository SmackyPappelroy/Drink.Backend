﻿using Drink.API.Clients;
using Drink.Common.DTOs;
using Drink.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Drink.API.Controllers
{
    public class MainWebsiteController : ControllerBase
    {
        public IContentController content;

        public MainWebsiteController(IContentController content)
        {
            this.content = content;
        }

        [HttpGet("get-recipes/page/{page}/pageSize/{pageSize}")]
        public async Task<IEnumerable<DishDTO>> GetRecipes([FromRoute] int page, [FromRoute] int pageSize, [FromQuery] bool glutenFree,
            [FromQuery] bool dairyFree, [FromQuery] bool keto, [FromQuery] bool vegetarian, [FromQuery] bool vegan, [FromQuery] bool cheap,
            [FromQuery] bool veryHealthy, [FromQuery] string cuisine, [FromQuery] string dishType)
        {
            try
            {
                return (await content.ImportRecipes()).Take(pageSize);
            }
            catch
            {
            }

            return Enumerable.Empty<DishDTO>();
        }
    }
}