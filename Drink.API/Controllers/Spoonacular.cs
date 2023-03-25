using Microsoft.AspNetCore.Mvc;
using Drink.Database.Services;
using System.IO;
using Drink.API.Infrastructure;
using Drink.Common.Models;
using Drink.API.Clients;

namespace Drink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpoonacularController : ControllerBase
    {
        private readonly IDbService _db;
        private readonly IContentClient foodClient;

        public SpoonacularController(IDbService db, IContentClient foodClient)
        {
            this._db = db;
            this.foodClient = foodClient;
        }

        [HttpGet]
        public async Task<object> ImportRecipes()
        {
            try
            {
                var recipes = await foodClient.SendGetAsync<RecipeResponse>(OperationType.GetRecipes, null);

                //TODO: Retrieve pairings
                //TODO: Save recipe and pairing to Db

                return recipes;
            }
            catch
            {
            }

            return Results.NotFound();
        }
    }
}
