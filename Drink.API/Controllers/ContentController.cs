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
    public class ContentController : ControllerBase
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
        public async Task<object> ImportRecipes()
        {
            try
            {
                 if (recipes == null)
                    recipes = await foodClient.SendGetAsync<RecipeResponse>(OperationType.GetRecipes, null);

                //TODO Map to DTO
                //TODO: Retrieve pairings
                //TODO: Save recipe and pairing to Db

                return recipes; //change to return Ok response or mapped recipes
            }
            catch
            {
            }

            return Results.NotFound();
        }

        [HttpGet("import-beer/{startPage}/{endPage}")]
        public async Task<object> ImportBeers([FromRoute] int startPage, [FromRoute] int endPage)
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

                return true;
            }
            catch
            {
            }

            return Results.NotFound();
        }

        [HttpGet("import-cocktails/{quantity}")]
        public async Task<object> ImportCocktails([FromRoute] int quantity)
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

                return true;
            }
            catch
            {
            }

            return Results.NotFound();
        }
    }
}
