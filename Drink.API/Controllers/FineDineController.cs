using Drink.API.Clients;
using Drink.API.Helpers;
using Drink.Common.DTOs;
using Drink.Common.ExternalModels;
using Drink.Common.Models;
using Drink.Database.Entities;
using Drink.Database.Services;
using Microsoft.AspNetCore.Mvc;

namespace Drink.API.Controllers
{
    public class MainWebsiteController : ControllerBase
    {
        public IDbService _db;

        public MainWebsiteController(IDbService db)
        {
            _db = db;
        }

        [HttpGet("get-recipes/page/{page}/pageSize/{pageSize}")]
        public async Task<IEnumerable<FineDish>> GetRecipes([FromRoute] int page, [FromRoute] int pageSize, [FromQuery] bool glutenFree,
            [FromQuery] bool dairyFree, [FromQuery] bool keto, [FromQuery] bool vegetarian, [FromQuery] bool vegan, [FromQuery] bool cheap,
            [FromQuery] bool veryHealthy, [FromQuery] string cuisine, [FromQuery] string dishType)
        {
            try
            {
                var dishes = await _db.GetAsync<Dish, DishDTO>(d => Convert.ToInt32(d.GlutenFree) >= Convert.ToInt32(glutenFree) &&
                                                                    Convert.ToInt32(d.DairyFree) >= Convert.ToInt32(dairyFree) &&
                                                                    Convert.ToInt32(d.Ketogenic) >= Convert.ToInt32(keto) &&
                                                                    Convert.ToInt32(d.Vegetarian) >= Convert.ToInt32(vegetarian) &&
                                                                    Convert.ToInt32(d.Vegan) >= Convert.ToInt32(vegan) &&
                                                                    Convert.ToInt32(d.Cheap) >= Convert.ToInt32(cheap) &&
                                                                    Convert.ToInt32(d.VeryHealthy) >= Convert.ToInt32(veryHealthy) &&
                                                                    (d.Cuisines.Any(c => c.Name == cuisine) || string.IsNullOrEmpty(cuisine)) &&
                                                                    (d.DishTypes.Any(d => d.Name == dishType) || string.IsNullOrEmpty(dishType))
                                                                    );

                var chunked = dishes.Chunk(pageSize);

                if (chunked.Count() < page)
                    return Enumerable.Empty<FineDish>();

                return chunked.ElementAt(page - 1).Select(d => MapperHelper.MapFineDish(d));
            }
            
            catch
            {
            }

            return Enumerable.Empty<FineDish>();
        }

        [HttpGet("get-drinks/page/{page}/pageSize/{pageSize}")]
        public async Task<IEnumerable<DrinkDTO>> GetDrinks([FromRoute] int page, [FromRoute] int pageSize)
        {
            try
            {
                var drinks = await _db.GetAsync<Database.Entities.Drink, DrinkDTO>();

                var chunked = drinks.Chunk(pageSize);

                if (chunked.Count() < page)
                    return Enumerable.Empty<DrinkDTO>();

                return chunked.ElementAt(page - 1);
            }

            catch
            {
            }

            return Enumerable.Empty<DrinkDTO>();
        }


        [HttpGet("get-from-drink/{id}")]
        public async Task<IEnumerable<FineDish>> GetFromDrinkAsync([FromRoute] int id)
        {
            try
            {
                var dishes = await _db.GetAsync<Dish, DishDTO> (d => d.Drinks.Any(drink => drink.Id == id));

                return dishes.Select(d => MapperHelper.MapFineDish(d));
            }
            catch
            {
            }

            return Enumerable.Empty<FineDish>();
        }

        [HttpGet("get-dish/{id}")]
        public async Task<FineDish> GetDishAsync([FromRoute] int id)
        {
            try
            {
                var dishes = _db.GetInclude(d => d.Id == id);

                return MapperHelper.MapFineDish(dishes.First());
            }
            catch
            {
            }

            return new FineDish();
        }

        [HttpGet("get-from-ingredient/{ingredient}")]
        public async Task<IEnumerable<FineDish>> GetFromIngredientAsync([FromRoute] string ingredient)
        {
            try
            {
                var dishes = await _db.GetAsync<Dish, DishDTO>(d => d.Ingredients.Contains(ingredient));

                return dishes.Select(d => MapperHelper.MapFineDish(d));
            }
            catch
            {
            }

            return Enumerable.Empty<FineDish>();
        }

        [HttpGet("get-from-title/{title}")]
        public async Task<IEnumerable<FineDish>> GetFromTitleAsync([FromRoute] string title)
        {
            try
            {
                var dishes = await _db.GetAsync<Dish, DishDTO>(d => d.Title.Contains(title));

                return dishes.Select(d => MapperHelper.MapFineDish(d));
            }
            catch
            {
            }

            return Enumerable.Empty<FineDish>();
        }
    }
}
