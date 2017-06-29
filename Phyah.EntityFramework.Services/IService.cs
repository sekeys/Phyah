

namespace Phyah.EntityFramework.Services
{
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IService<T> where T: class, Phyah.EntityFramework.Services.IEntity
    {
        DbContext Context { get; }
        DbSet<T> Set();
        T Single();
        Task<T> SingleAsync();
        T Single(string id);
        Task<T> SingleAsync(string id);

        T Single(IQueryable<T> queryable);
        Task<T> SingleAsync(IQueryable<T> queryable);
        T Single(IOrderedQueryable<T> queryable);
        Task<T> SingleAsync(IOrderedQueryable<T> queryable);
        T Single(Expression<Func<T, bool>> epxression);
        Task<T> SingleAsync(Expression<Func<T, bool>> epxression);


        IEnumerable<T> Multiple();
        Task<IEnumerable<T>> MultipleAsync();
        IEnumerable<T> Multiple(Expression<Func<T, bool>> epxression);
        Task<IEnumerable<T>> MultipleAsync(Expression<Func<T, bool>> epxression);
        IEnumerable<T> Multiple(IOrderedQueryable<T> queryable);
        Task<IEnumerable<T>> MultipleAsync(IOrderedQueryable<T> queryable);
        IEnumerable<T> Multiple(IQueryable<T> queryable);
        Task<IEnumerable<T>> MultipleAsync(IQueryable<T> queryable);

        int Count();
        Task<int> CountAsync();
        int Count(IQueryable<T> queryable);
        Task<int> CountAsync(IQueryable<T> queryable);
        int Count(IOrderedQueryable<T> queryable);
        Task<int> CountAsync(IOrderedQueryable<T> queryable);


        int Delete(string id);
        Task<int> DeleteAsync(string id);
        int Delete(T entity);
        Task<int> DeleteAsync(T entity);

        int Delete(IEntityConvert<T> entity);
        Task<int> DeleteAsync(IEntityConvert<T> entity);
        int Delete(IQueryable<T> queryable);
        Task<int> DeleteAsync(IQueryable<T> queryable);
        int Delete(IOrderedQueryable<T> queryable);
        Task<int> DeleteAsync(IOrderedQueryable<T> queryable);

        void Update(T entity);
        Task UpdateAsync(T entity);

        void Update(string id,T entity);
        Task UpdateAsync(string id, T entity);
        void Update(IEntityConvert<T> entity);
        Task UpdateAsync(IEntityConvert<T> entity);

        void New(T entity);
        Task NewAsync(T entity);
        void New(IEntityConvert<T> entity);
        Task NewAsync(IEntityConvert<T> entity);

        bool Exist(IEntityConvert<T> entity);
        Task<bool> ExistAsync(IEntityConvert<T> entity);

        bool Exist(T entity);
        Task<bool> ExistAsync(T entity);
        bool Exist(string id);
        Task<bool> ExistAsync(string id);

        T NewOrUpdate(T entity);
        Task<T>  NewOrUpdateAsync(T entity);

        T NewOrUpdate(IEntityConvert<T> entity);
        Task<T> NewOrUpdateAsync(IEntityConvert<T> entity);

        IEnumerable<T> Pagination(out int totalCount, int current = 1, int size = 24);
        Task<Tuple<int, IEnumerable<T>>> PaginationAsync(int current = 1, int size = 24);
        IEnumerable<T> Pagination(Expression<Func<T, bool>> epxression, out int totalCount, int current = 1, int size = 24);
        Task<Tuple<int, IEnumerable<T>>> PaginationAsync(Expression<Func<T, bool>> epxression, int current = 1, int size = 24);


        IEnumerable<T> Pagination(IQueryable<T> queryable, out int totalCount, int current = 1, int size = 24);
        Task<Tuple<int, IEnumerable<T>>> PaginationAsync(IQueryable<T> queryable, int current = 1, int size = 24);
        IEnumerable<T> Pagination(IOrderedQueryable<T> queryable, out int totalCount, int current = 1, int size = 24);
        Task<Tuple<int, IEnumerable<T>>> PaginationAsync(IOrderedQueryable<T> queryable, int current = 1, int size = 24);
    }
}
