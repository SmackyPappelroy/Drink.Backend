using Microsoft.AspNetCore.Mvc;
using Drink.Database.Services;
using System.IO;
using Drink.API.Infrastructure;
using Drink.Common.Models;
using Drink.API.Clients;
using Drink.Common.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Drink.Database.Entities;
using Drink.API.Services;

namespace Drink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService inner;
        private readonly IContentClient foodClient;

        public ContentController(IContentService inner, IContentClient foodClient)
        {
            this.inner = inner;
            this.foodClient = foodClient;
        }

        [HttpGet("import-food")]
        public async Task<IEnumerable<DishDTO>> ImportRecipes()
        {
            try
            {
                var recipes = await foodClient.SendGetAsync<RecipeResponse>(OperationType.GetRecipes, null);

                return await inner.MapAndSaveDishesAsync(recipes.Recipes);
            }
            catch
            {
                return Enumerable.Empty<DishDTO>();
            }
        }

        [HttpGet("import-beer/{startPage}/{endPage}")]
        public async Task<IEnumerable<DrinkDTO>> ImportBeers([FromRoute] int startPage, [FromRoute] int endPage)
        {
            try
            {
                var beers = new List<Beer>();
                var drinks = new List<DrinkDTO>();

                for (int i = startPage; i < endPage; i++)
                {
                    beers = (await foodClient.SendGetAsync<IEnumerable<Beer>>(OperationType.GetBeers, i)).ToList();
                    drinks.AddRange(await inner.MapAndSaveDrinkAsync(beers, DrinkType.Beer));
                }

                return drinks;
            }
            catch
            {
            }

            return Enumerable.Empty<DrinkDTO>();
        }

        [HttpGet("import-cocktails/{quantity}")]
        public async Task<IEnumerable<DrinkDTO>> ImportCocktails([FromRoute] int quantity)
        {
            try
            {
                CocktailResponse response;
                var cocktails = new List<Cocktail>();

                for (int i = 0; i < quantity; i++)
                {
                    response = await foodClient.SendGetAsync<CocktailResponse>(OperationType.GetCocktails);
                    cocktails.AddRange(response.Drinks);
                    await Task.Delay(1000);
                }

                cocktails.ForEach(cocktail => cocktail.GetDescription());

                var drinks = await inner.MapAndSaveDrinkAsync(cocktails, DrinkType.Cocktail);
                

                return drinks;
            }
            catch
            {
            }

            return Enumerable.Empty<DrinkDTO>();
        }
    }
}
