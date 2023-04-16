using AutoMapper;
using Drink.Database.Contexts;
using Drink.Database.Interface;
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

        public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
            where TEntity : class
            where TDto : class
        {
            _db.ChangeTracker.Clear();
            var entity = _mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(entity);

            return entity;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) >= 0;
        }

        public void Include<TEntity>() where TEntity : class
        {
            var propertyNames = _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

            if (propertyNames is null) return;

            foreach (var name in propertyNames)
                _db.Set<TEntity>().Include(name).Load();
        }

        public async Task<List<TDto>> GetAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
        {
            var entities = await _db.Set<TEntity>().Where(expression).ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        async Task<List<TDto>> IDbService.GetAsync<TEntity, TDto>()
        {
            var entities = await _db.Set<TEntity>().ToListAsync();

            return _mapper.Map<List<TDto>>(entities);
        }
    }
}
