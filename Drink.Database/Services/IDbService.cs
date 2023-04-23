using Drink.Common.DTOs;
using Drink.Common.Models;
using Drink.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Services
{
    public interface IDbService
    {
        Task<Dish> AddDishAsync(DishDTO dto, Recipe recipe, Random random);

        Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
            where TEntity : class
            where TDto : class;

        Task<bool> SaveChangesAsync();

        Task<List<TDto>> GetAsync<TEntity, TDto>(string query)
            where TEntity : class, IEntity
            where TDto : class;

        Task<List<TDto>> GetAsync<TEntity, TDto>() where TEntity : class, IEntity
            where TDto : class;

        Task<List<TDto>> GetAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity
            where TDto : class;
    }
}
