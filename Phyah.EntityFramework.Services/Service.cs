using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Phyah.EntityFramework.Services
{
    public class Service<T> : IService<T> where T : class, Phyah.EntityFramework.Services.IEntity
    {
        public DbContext Context { get; private set; }
        public Service(DbContext Context)
        {
            this.Context = Context;

        }

        public int Count() => Context.Set<T>().Count();

        public int Count(IQueryable<T> queryable) => queryable.Count();

        public int Count(IOrderedQueryable<T> queryable) => queryable.Count();

        public async Task<int> CountAsync() => await Set().CountAsync();

        public async Task<int> CountAsync(IQueryable<T> queryable)
        {
            return await queryable.CountAsync();
        }

        public async Task<int> CountAsync(IOrderedQueryable<T> queryable) => await queryable.CountAsync();

        public int Delete(string id) => Delete(Single(id));

        public int Delete(IQueryable<T> queryable) => Delete(Single(queryable));

        public int Delete(IOrderedQueryable<T> queryable) => Delete(Single(queryable));

        public async Task<int> DeleteAsync(string id) => await DeleteAsync(await SingleAsync(id));

        public async Task<int> DeleteAsync(IQueryable<T> queryable) => await DeleteAsync(await SingleAsync(queryable));

        public async Task<int> DeleteAsync(IOrderedQueryable<T> queryable) => await DeleteAsync(await SingleAsync(queryable));

        public bool Exist(IEntityConvert<T> entity) => Exist(entity.Convert());

        public bool Exist(T entity) => Count(Set().Where(m => m.Id == entity.Id)) > 0;

        public bool Exist(string id) => Count(Set().Where(m => m.Id == id)) > 0;
        public Task<bool> ExistAsync(IEntityConvert<T> entity) => ExistAsync(entity.Convert());
        public async Task<bool> ExistAsync(T entity) => await CountAsync(Set().Where(m => m.Id == entity.Id)) > 0;

        public async Task<bool> ExistAsync(string id) => await CountAsync(Set().Where(m => m.Id == id)) > 0;

        public IEnumerable<T> Multiple() => Set().ToList();

        public IEnumerable<T> Multiple(Expression<Func<T, bool>> expression) => Set().Where(expression).ToList();

        public IEnumerable<T> Multiple(IOrderedQueryable<T> queryable) => queryable.ToList();

        public IEnumerable<T> Multiple(IQueryable<T> queryable) => queryable.ToList();

        public async Task<IEnumerable<T>> MultipleAsync() => await Set().ToListAsync();

        public async Task<IEnumerable<T>> MultipleAsync(Expression<Func<T, bool>> expression) => await Set().Where(expression).ToListAsync();

        public async Task<IEnumerable<T>> MultipleAsync(IOrderedQueryable<T> queryable) => await queryable.ToListAsync();

        public async Task<IEnumerable<T>> MultipleAsync(IQueryable<T> queryable) => await queryable.ToListAsync();

        public void New(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void New(IEntityConvert<T> entity) => New(entity.Convert());

        public async Task NewAsync(T entity)
        {
            Context.Set<T>().Attach(entity);
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task NewAsync(IEntityConvert<T> entity) => await NewAsync(entity.Convert());

        public T NewOrUpdate(T entity)
        {
            var model = this.Single(entity.Id);
            if (model == null)
            {
                New(entity);
                return entity;
            }
            model = Merge(model, entity);
            Update(model);
            return model;
        }

        public T NewOrUpdate(IEntityConvert<T> entity) => NewOrUpdate(entity.Convert());

        public async Task<T> NewOrUpdateAsync(T entity)
        {
            var model = await this.SingleAsync(entity.Id);
            if (model == null)
            {
                await NewAsync(entity);
                return entity;
            }
            model = Merge(model, entity);
            await UpdateAsync(model);
            return model;
        }

        public async Task<T> NewOrUpdateAsync(IEntityConvert<T> entity) => await NewOrUpdateAsync(entity.Convert());
        public IEnumerable<T> Pagination(out int totalCount, int current = 1, int size = 24) => Pagination(m => 1 == 1, out totalCount, current, size);

        public IEnumerable<T> Pagination(Expression<Func<T, bool>> expression, out int totalCount, int current = 1, int size = 24)
        {
            if (current == 0)
            {
                current = 1;
            }
            totalCount = 0;
            var result = (from i in this.Context.Set<T>()
                          select i);
            result = expression == null ? result : result.Where(expression);
            totalCount = result.Count();
            if (current == 1)
            {
                return result.Take(current * size).ToList();
            }
            else
            {
                return result.Skip((current - 1) * size).Take(current * size).ToList();
            }
        }

        public IEnumerable<T> Pagination(IQueryable<T> queryable, out int totalCount, int current = 1, int size = 24)
        {
            if (current == 0)
            {
                current = 1;
            }
            totalCount = queryable.Count();
            if (current == 1)
            {
                return queryable.Take(current * size).ToList();
            }
            else
            {
                return queryable.Skip((current - 1) * size).Take(current * size).ToList();
            }
        }

        public IEnumerable<T> Pagination(IOrderedQueryable<T> queryable, out int totalCount, int current = 1, int size = 24)
        {
            if (current == 0)
            {
                current = 1;
            }
            totalCount = queryable.Count();
            if (current == 1)
            {
                return queryable.Take(current * size).ToList();
            }
            else
            {
                return queryable.Skip((current - 1) * size).Take(current * size).ToList();
            }
        }

        public async Task<Tuple<int, IEnumerable<T>>> PaginationAsync(int current = 1, int size = 24) => await PaginationAsync(m => 1 == 1, current, size);

        public async Task<Tuple<int, IEnumerable<T>>> PaginationAsync(Expression<Func<T, bool>> epxression, int current = 1, int size = 24)
        => await PaginationAsync(Set().Where(epxression), current, size);

        public async Task<Tuple<int, IEnumerable<T>>> PaginationAsync(IQueryable<T> queryable, int current = 1, int size = 24)
        {
            if (current == 0)
            {
                current = 1;
            }
            int count = 0;
            if (current == 1)
            {
                return new Tuple<int, IEnumerable<T>>(count, await queryable.Skip(0).Take(current * size).ToListAsync());
            }
            else
            {
                return new Tuple<int, IEnumerable<T>>(count
                    , await queryable.Skip((current - 1) * size).Take(current * size).ToListAsync());
            }
        }

        public async Task<Tuple<int, IEnumerable<T>>> PaginationAsync(IOrderedQueryable<T> queryable, int current = 1, int size = 24)
        {
            if (current == 0)
            {
                current = 1;
            }
            int count = 0;
            if (current == 1)
            {
                return new Tuple<int, IEnumerable<T>>(count, await queryable.Skip(0).Take(current * size).ToListAsync());
            }
            else
            {
                return new Tuple<int, IEnumerable<T>>(count
                    , await queryable.Skip((current - 1) * size).Take(current * size).ToListAsync());
            }
        }

        public T Single() => Set().FirstOrDefault();

        public T Single(string id) => Set().Where(m => m.Id == id).FirstOrDefault();

        public T Single(Expression<Func<T, bool>> expression) => Set().Where(expression).FirstOrDefault();

        public async Task<T> SingleAsync() => await Set().FirstOrDefaultAsync();

        public async Task<T> SingleAsync(string id) => await Set().Where(m => m.Id == id).FirstOrDefaultAsync();

        public async Task<T> SingleAsync(Expression<Func<T, bool>> expression) => await Set().Where(expression).FirstOrDefaultAsync();

        public void Update(T entity)
        {
            if (Context.Entry<T>(entity).State == EntityState.Modified)
            {
                Context.SaveChanges();
            }
            else if (Context.Entry<T>(entity).State == EntityState.Detached)
            {
                try
                {
                    Context.Set<T>().Attach(entity);
                    Context.Entry<T>(entity).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    T old = Single(entity.Id);
                    Context.Entry(old).CurrentValues.SetValues(entity);
                }
                Context.SaveChanges();
            }
        }

        public void Update(string id, T entity)
        {
            var model = this.Single(id);
            if (model == null)
            {
                New(entity);
            }
            model = Merge(model, entity);
            Update(model);
        }

        public void Update(IEntityConvert<T> entity) => Update(entity.Convert());

        public async Task UpdateAsync(T entity)
        {
            if (Context.Entry<T>(entity).State == EntityState.Modified)
            {
                await Context.SaveChangesAsync();
            }
            else if (Context.Entry<T>(entity).State == EntityState.Detached)
            {
                try
                {
                    Context.Set<T>().Attach(entity);
                    Context.Entry<T>(entity).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    T old = Single(entity.Id);
                    Context.Entry(old).CurrentValues.SetValues(entity);
                }
                await Context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var model = await this.SingleAsync(entity.Id);
            if (model == null)
            {
                await NewAsync(entity);
            }
            model = Merge(model, entity);
            await UpdateAsync(model);
        }

        public async Task UpdateAsync(IEntityConvert<T> entity) => await UpdateAsync(entity.Convert());


        protected T Merge(T old, T newValue)
        {
            var newValueType = newValue.GetType();
            foreach (var item in newValueType.GetTypeInfo().GetProperties(BindingFlags.Public))
            {
                object value = item.GetValue(newValue);
                if (value != null)
                {
                    item.SetValue(old, value);
                }
            }
            return old;
        }
        public int Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChanges();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public int Delete(IEntityConvert<T> entity) => Delete(entity.Convert());

        public async Task<int> DeleteAsync(IEntityConvert<T> entity) => await DeleteAsync(entity.Convert());

        public DbSet<T> Set() => Context.Set<T>();

        public T Single(IQueryable<T> queryable) => queryable.FirstOrDefault();

        public async Task<T> SingleAsync(IQueryable<T> queryable) => await queryable.FirstOrDefaultAsync();

        public T Single(IOrderedQueryable<T> queryable) => queryable.FirstOrDefault();

        public async Task<T> SingleAsync(IOrderedQueryable<T> queryable) => await queryable.FirstOrDefaultAsync();
    }
}
