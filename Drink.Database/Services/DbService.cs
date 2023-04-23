using AutoMapper;
using Drink.Common.DTOs;
using Drink.Common.Models;
using Drink.Database.Contexts;
using Drink.Database.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Drink.Database.Services
{
    public class DbService : IDbService
    {
        private readonly DishDrinkContext _db;
        private readonly IMapper _mapper;
        public DbService(DishDrinkContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<Dish> AddDishAsync(DishDTO dto, Recipe recipe, Random random)
        {
            var entity = _mapper.Map<Dish>(dto);
            await _db.Set<Dish>().AddAsync(entity);

            var cuisines = _db.Cuisines.Where(c => recipe.Cuisines.Contains(c.Name));
            entity.Cuisines = cuisines.ToList();

            var types = _db.DishTypes.Where(t => recipe.DishTypes.Contains(t.Name));
            entity.DishTypes = types.ToList();

            var drinks = _db.Drinks;
            entity.Drinks = drinks.ToList().OrderBy(x => random.Next()).Take(8).ToList();

            var ingredients = _db.Ingredients.Where(i => recipe.ExtendedIngredients.Select(e => e.SecondaryId).Contains(i.SecondaryId));
            entity.ExtendedIngredients = ingredients.ToList();

            return entity;
        }

        public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
            where TEntity : class
            where TDto : class
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(entity);

            return entity;
        }

        public async Task<List<TDto>> GetAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
        {
            Include<TEntity>();
            var entities = await _db.Set<TEntity>().Where(expression).ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) >= 0;
        }

        public void Include<TEntity>() where TEntity : class
        {
            var entityType = _db.Model.FindEntityType(typeof(TEntity));

            if (entityType is null)
                return;

            var propertyNames = entityType.GetNavigations()
                                           .Concat<INavigationBase>(entityType.GetSkipNavigations())
                                           .Select(e => e.Name);

            if (propertyNames is null) 
                return;

            foreach (var name in propertyNames)
                _db.Set<TEntity>().Include(name).Load();
        }

        public async Task<List<TDto>> GetAsync<TEntity, TDto>(string query)
        where TEntity : class, IEntity
        where TDto : class
        {
            Include<TEntity>();
            var entities = _db.Dishes.FromSqlRaw(query);

            return _mapper.Map<List<TDto>>(entities);
        }

        async Task<List<TDto>> IDbService.GetAsync<TEntity, TDto>()
        {
            Include<TEntity>();
            var entities = await _db.Set<TEntity>().ToListAsync();
 
            return _mapper.Map<List<TDto>>(entities);
        }
    }
}
