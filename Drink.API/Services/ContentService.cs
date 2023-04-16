using Drink.API.Clients;
using Drink.Common.DTOs;
using Drink.Common.Models;
using Drink.Database.Entities;
using Drink.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace Drink.API.Services
{
    public class ContentService : IContentService
    {
        public IDbService _db;

        public ContentService(IDbService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Dish>> MapAndSaveDishesAsync(IEnumerable<Recipe> recipes)
        {
            var drinks = await GetDrinks();

            var random = new Random();

            List<Dish> dishes = new();

            foreach (var recipe in recipes.Where(r => r.ExtendedIngredients.Any(ing => ing.SecondaryId != -1)))
            {
                var r = await MapDishAsync(recipe, drinks, random);
                var d = await _db.AddAsync<Dish, DishDTO>(r);
                dishes.Add(d);
            }

            await _db.SaveChangesAsync();

            return dishes;
        }

        public async Task<DishDTO> MapDishAsync(Recipe recipe, IEnumerable<DrinkDTO> drinks, Random random)
        {
            (var cuisines, var dishTypes) = await GetCuisinesAndDishTypesAsync(recipe);

            var extendedIngredients = await GetIngredientsAsync(recipe.ExtendedIngredients);

            return new DishDTO
            {
                Title = recipe.Title,
                Servings = recipe.Servings,
                Image = recipe.Image,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Cheap = recipe.Cheap,
                Cuisines = cuisines.Where(c => recipe.Cuisines.Contains(c.Name)),
                GlutenFree = recipe.GlutenFree,
                DairyFree = recipe.DairyFree,
                Ketogenic = recipe.Ketogenic,
                Vegan = recipe.Vegan,
                Vegetarian = recipe.Vegetarian,
                VeryHealthy = recipe.VeryHealthy,
                Instructions = recipe.Instructions,
                DishTypes = dishTypes.Where(d => recipe.DishTypes.Contains(d.Name)),
                ExtendedIngredients = extendedIngredients,
                Ingredients = string.Join('*', recipe.ExtendedIngredients.Select(ing => ing.Original)),
                Drinks = drinks.OrderBy(x => random.Next()).Take(8)
            };
        }

        private async Task<IEnumerable<IngredientDTO>> GetIngredientsAsync(IEnumerable<Common.Models.Ingredient> extendedIngredients)
        {
            List<IngredientDTO> ingredients = new();

            foreach (var ing in extendedIngredients)
            {
                var ingredient = await GetOrCreateIngredientAsync(ing);
                ingredients.Add(ingredient);
            }

            return ingredients;
        }

        private async Task<IngredientDTO> GetOrCreateIngredientAsync(Common.Models.Ingredient ing)
        {
            var ingredient = (await _db.GetAsync<Database.Entities.Ingredient, IngredientDTO>(e => e.SecondaryId.Equals(ing.SecondaryId))).FirstOrDefault();

            if (ingredient is not null)
                return ingredient;

            var dto = new IngredientDTO
            {
                Name = ing.Name,
                SecondaryId = ing.SecondaryId,
                Image = ing.Image ?? "default.png",
                Original = ing.Original
            };

            ingredient = await _db.AddAsync<Database.Entities.Ingredient, IngredientDTO>(dto);

            await _db.SaveChangesAsync();

            return ingredient;
        }

        private async Task<(IEnumerable<CuisineDTO>, IEnumerable<DishTypeDTO>)> GetCuisinesAndDishTypesAsync(Recipe recipe)
        {
            var cuisines = await GetCuisines();
            var cuisinesToSave = recipe.Cuisines.Where(cuisine => !cuisines.Select(c => c.Name).Contains(cuisine));
            var taskC = cuisinesToSave.Select(async c => await _db.AddAsync<Cuisine, CuisineDTO>(new CuisineDTO() { Name = c }));
            await Task.WhenAll(taskC);

            var dishTypes = await GetDishTypes();
            var dishTypesToSave = recipe.DishTypes.Where(type => !dishTypes.Select(t => t.Name).Contains(type));
            var taskD = dishTypesToSave.Select(async d => await _db.AddAsync<DishType, DishTypeDTO>(new DishTypeDTO() { Name = d }));
            await Task.WhenAll(taskD);

            await _db.SaveChangesAsync();

            cuisines = await GetCuisines();
            dishTypes = await GetDishTypes();

            return (cuisines, dishTypes);
        }

        public async Task<IEnumerable<DrinkDTO>> MapAndSaveDrinkAsync(IEnumerable<IDrink> iDrinks, DrinkType type)
        {
            var drinks = iDrinks.Select(cocktail => ContentMapper.MapToDrink(cocktail, type));

            var tasks = drinks.Select(dto => _db.AddAsync<Database.Entities.Drink, DrinkDTO>(dto));

            await Task.WhenAll(tasks);

            await _db.SaveChangesAsync();

            return drinks;
        }

        private async Task<IEnumerable<DrinkDTO>> GetDrinks()
        {
            return await _db.GetAsync<Database.Entities.Drink, DrinkDTO>();
        }

        private async Task<IEnumerable<CuisineDTO>> GetCuisines()
        {
            return await _db.GetAsync<Cuisine, CuisineDTO>();
        }

        private async Task<IEnumerable<DishTypeDTO>> GetDishTypes()
        {
            return await _db.GetAsync<DishType, DishTypeDTO>();
        }
    }
}
