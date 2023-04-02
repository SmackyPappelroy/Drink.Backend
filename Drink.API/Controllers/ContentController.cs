using Microsoft.AspNetCore.Mvc;
using Drink.Database.Services;
using System.IO;
using Drink.API.Infrastructure;
using Drink.Common.Models;
using Drink.API.Clients;
using Drink.Common.DTOs;

namespace Drink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase, IContentController
    {
        private readonly IDbService _db;
        private readonly IContentClient foodClient;
        private static RecipeResponse? recipes;

        public ContentController(IDbService db, IContentClient foodClient)
        {
            this._db = db;
            this.foodClient = foodClient;
        }

        [HttpGet("import-food")]
        public async Task<IEnumerable<DishDTO>> ImportRecipes()
        {
            try
            {
                if (recipes == null)
                    recipes = await foodClient.SendGetAsync<RecipeResponse>(OperationType.GetRecipes, null);

                var mapped = recipes.Recipes.Select(async recipe => await MapDishAsync(recipe));

                //TODO Map to DTO
                //TODO: Retrieve pairings
                //TODO: Save recipe and pairing to Db

                return await Task.WhenAll(mapped); ; //change to return Ok response or mapped recipes
            }
            catch
            {
            }

            return Enumerable.Empty<DishDTO>();
        }

        private async Task<DishDTO> MapDishAsync(Recipe recipe)
        {
            var drinks = await ImportBeers(1, 2);

            return new DishDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Servings = recipe.Servings,
                Image = recipe.Image,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Cheap = recipe.Cheap,
                Cuisines = recipe.Cuisines,
                GlutenFree = recipe.GlutenFree,
                DairyFree = recipe.DairyFree,
                Ketogenic = recipe.Ketogenic,
                Vegan = recipe.Vegan,
                Vegetarian = recipe.Vegetarian,
                VeryHealthy = recipe.VeryHealthy,
                Instructions = recipe.Instructions,
                DishTypes = recipe.DishTypes,
                Ingredients = string.Join(',', recipe.ExtendedIngredients.Select(ing => ing.Original)),
                Drinks = drinks.Take(5)
            };
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
                    drinks = beers.Select(beer => ContentMapper.MapToDrink(beer, DrinkType.Beer)).ToList();
                    //TODO Save to Db
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
                var drinks = cocktails.Select(cocktail => ContentMapper.MapToDrink(cocktail, DrinkType.Cocktail));
                //TODO Save to Db

                return drinks;
            }
            catch
            {
            }

            return Enumerable.Empty<DrinkDTO>();
        }
    }
}
