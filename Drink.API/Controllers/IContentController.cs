using Drink.Common.DTOs;

namespace Drink.API.Controllers
{
    public interface IContentController
    {
        Task<IEnumerable<DishDTO>> ImportRecipes();
    }
}
