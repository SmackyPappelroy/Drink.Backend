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

        public async Task<IEnumerable<DishDTO>> MapAndSaveDishesAsync(IEnumerable<Recipe> recipes)
        {
            var random = new Random();

            List<DishDTO> dishes = new();
            var filteredRecipes = recipes.Where(r => r.ExtendedIngredients.Any(ing => ing.SecondaryId != -1))
                                         .Where(r => r.Image is not null);

            foreach (var recipe in filteredRecipes)
            {
                await SaveCuisinesAndDishTypesAsync(recipe);

                await SaveIngredientsAsync(recipe.ExtendedIngredients);

                var r = await MapDishAsync(recipe);
                var d = await _db.AddDishAsync(r, recipe, random);
                dishes.Add(r);
            }

            await _db.SaveChangesAsync();

            return dishes;
        }

        public async Task<DishDTO> MapDishAsync(Recipe recipe)
        {
            return new DishDTO
            {
                Title = recipe.Title,
                Servings = recipe.Servings,
                Image = recipe.Image,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Cheap = recipe.Cheap,
                GlutenFree = recipe.GlutenFree,
                DairyFree = recipe.DairyFree,
                Ketogenic = recipe.Ketogenic,
                Vegan = recipe.Vegan,
                Vegetarian = recipe.Vegetarian,
                VeryHealthy = recipe.VeryHealthy,
                Instructions = recipe.Instructions,
                Ingredients = string.Join('*', recipe.ExtendedIngredients.Select(ing => ing.Original))
            };
        }

        private async Task SaveIngredientsAsync(IEnumerable<Common.Models.Ingredient> extendedIngredients)
        {
            List<IngredientDTO> ingredients = new();

            foreach (var ing in extendedIngredients)
            {
                await GetOrCreateIngredientAsync(ing);
            }
        }

        private async Task GetOrCreateIngredientAsync(Common.Models.Ingredient ing)
        {
            var ingredient = (await _db.GetAsync<Database.Entities.Ingredient, IngredientDTO>(e => e.SecondaryId.Equals(ing.SecondaryId))).FirstOrDefault();

            if (ingredient is not null)
                return;

            var dto = new IngredientDTO
            {
                Name = ing.Name,
                SecondaryId = ing.SecondaryId,
                Image = ing.Image ?? "default.png",
                Original = ing.Original
            };

            ingredient = await _db.AddAsync<Database.Entities.Ingredient, IngredientDTO>(dto);

            await _db.SaveChangesAsync();
        }

        private async Task SaveCuisinesAndDishTypesAsync(Recipe recipe)
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
        }

        public async Task<IEnumerable<DrinkDTO>> MapAndSaveDrinkAsync(IEnumerable<IDrink> iDrinks, DrinkType type)
        {
            var drinks = iDrinks.Select(cocktail => ContentMapper.MapToDrink(cocktail, type));

            foreach (var drink in drinks)
            {
                await _db.AddAsync<Database.Entities.Drink, DrinkDTO>(drink);
            }

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
