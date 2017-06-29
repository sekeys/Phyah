using Microsoft.EntityFrameworkCore;
using Phyah.Huaxue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Biz
{
    public abstract class ServiceBase<T> where T : class, IEntity
    {
        DbContext db;
        public ServiceBase(DbContext context)
        {
            this.db = context;
        }

        public DbContext Context
        {
            get
            {
                return db;
            }
        }

        #region ICommon<T>  
        public T Add(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Add(model);
            db.SaveChanges();
            return model;
        }
        public async Task<T> AddAsync(T model)
        {
            db.Set<T>().Attach(model);
            await db.Set<T>().AddAsync(model);
            await db.SaveChangesAsync();
            return model;
        }


        public async Task<T> UpdateAsync(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                await db.SaveChangesAsync();
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    T old = Find(model.Id);
                    db.Entry(old).CurrentValues.SetValues(model);
                }
                await db.SaveChangesAsync();
            }
            return model;
        }

        public T Update(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                db.SaveChanges();
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    T old = Find(model.Id);
                    db.Entry(old).CurrentValues.SetValues(model);
                }
                db.SaveChanges();
            }
            return model;
        }
        public T Single(string id)
        {
            var result = from i in this.Context.Set<T>()
                         where i.Id == id
                         select i;
            return result.FirstOrDefault();
        }
        public T Single(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            var result = this.Context.Set<T>().Where(expression);
            return result.FirstOrDefault();
        }

        public async Task<T> SingleAsync(string id)
        {
            var result = from i in this.Context.Set<T>()
                         where i.Id == id
                         select i;
            return await result.FirstOrDefaultAsync();
        }

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
        public T AddOrUpdate(T entity)
        {
            var model = this.Single(entity.Id);
            if (model == null)
            {
                return Add(entity);
            }
            model = Merge(model, entity);
            Update(model);
            return model;
        }
        public async Task<T> AddOrUpdateAsync(T entity)
        {
            var model = await this.SingleAsync(entity.Id);
            if (model == null)
            {
                return await AddAsync(entity);
            }
            model = Merge(model, entity);
            return await UpdateAsync(model);
        }
        public void Delete(T model)
        {
            db.Set<T>().Remove(model);
            db.SaveChanges();
        }
        public async Task<Tuple<int, IList<T>>> PaginationAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression, int current, int size = 12)
        {
            if (current == 0)
            {
                current = 1;
            }
            int count = 0;
            var result = (from i in this.Context.Set<T>()
                          select i);
            if (expression == null)
            {
                expression = m => 1 == 1;
            }
            count = await result.Where(m => 1 == 1).CountAsync();
            if (current == 1)
            {
                return new Tuple<int, IList<T>>(count, await result.Skip(0).Take(current * size).Where(expression).ToListAsync());
            }
            else
            {
                return new Tuple<int, IList<T>>(count
                    , await result.Skip((current - 1) * size).Take(current * size).Where(expression).ToListAsync());
            }
        }
        public IList<T> Pagination(System.Linq.Expressions.Expression<Func<T, bool>> expression, int current, out int count, int size = 12)
        {
            if (current == 0)
            {
                current = 1;
            }
            count = 0;
            var result = (from i in this.Context.Set<T>()
                          select i);
            result = expression == null ? result : result.Where(expression);
            count = result.Count();
            if (current == 1)
            {
                return result.Take(current * size).ToList();
            }
            else
            {
                return result.Skip((current - 1) * size).Take(current * size).ToList();
            }
        }
        public void Delete(string id)
        {
            var model = Single(id);
            if (model != null)
            {
                db.Set<T>().Remove(model);
                db.SaveChanges();
            }
        }
        public void Delete(params object[] keyValues)
        {
            T model = Find(keyValues);
            if (model != null)
            {
                db.Set<T>().Remove(model);
                db.SaveChanges();
            }
        }
        public T Find(params object[] keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }
        public List<T> FindAll()
        {
            return db.Set<T>().ToList();
        }
        public List<T> FindAll(System.Linq.Expressions.Expression<Func<T,bool>> func)
        {
            return db.Set<T>().Where(func).ToList();
        }
        #endregion
    }
}
