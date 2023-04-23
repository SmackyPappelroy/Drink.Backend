using Drink.Common.DTOs;
using Drink.Common.ExternalModels;

namespace Drink.API.Helpers
{
    public static class MapperHelper
    {
        public static FineDish MapFineDish(DishDTO dish)
        {
            return new FineDish
            {
                Id = dish.Id,
                Cheap= dish.Cheap,
                Cuisines = dish.Cuisines.Select(x => x.Name),
                Servings = dish.Servings,
                DairyFree= dish.DairyFree,
                DishTypes = dish.DishTypes.Select(x => x.Name),
                Drinks = dish.Drinks,
                GlutenFree = dish.GlutenFree,
                Image = dish.Image,
                Ingredients = dish.Ingredients,
                Instructions = dish.Instructions,
                Ketogenic = dish.Ketogenic,
                ReadyInMinutes = dish.ReadyInMinutes,
                Title = dish.Title,
                Vegan = dish.Vegan,
                Vegetarian = dish.Vegetarian,
                VeryHealthy = dish.VeryHealthy
            };
        }
    }
}
