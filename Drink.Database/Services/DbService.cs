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

        public async Task<Dish> AddDishAsync(DishDTO dto, Recipe recipe, Random random, IEnumerable<Entities.Drink> drinks)
        {
            var entity = _mapper.Map<Dish>(dto);
            await _db.Set<Dish>().AddAsync(entity);

            var cuisines = _db.Cuisines.Where(c => recipe.Cuisines.Contains(c.Name));
            entity.Cuisines = cuisines.ToList();

            var types = _db.DishTypes.Where(t => recipe.DishTypes.Contains(t.Name));
            entity.DishTypes = types.ToList();

            entity.Drinks = drinks.ToList().OrderBy(x => random.Next()).Take(8).ToList();

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

        public List<DishDTO> GetInclude(Expression<Func<Dish, bool>> expression)
        {
            var entities = _db.Dishes.Where(expression);
            entities.Include("Drinks").Load();
            entities.Include("DishTypes").Load();
            entities.Include("Cuisines").Load();
            entities.ToList();
            return _mapper.Map<List<DishDTO>>(entities);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) >= 0;
        }

        async Task<List<TDto>> IDbService.GetAsync<TEntity, TDto>()
        {
            var entities = await _db.Set<TEntity>().ToListAsync();

            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<List<TDto>> GetAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
        {
            var entities = await _db.Set<TEntity>().Where(expression).ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public IEnumerable<Entities.Drink> GetAllDrinks()
        {
            return _db.Drinks;
        }
    }
}
