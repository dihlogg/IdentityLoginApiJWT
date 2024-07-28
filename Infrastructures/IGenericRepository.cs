namespace IdentityWebApiSample.Server.Infrastructures;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IGenericRepository<T> where T : class
{
    T? GetById(Guid id);

    Task<T?> GetByIdAsync(Guid id);

    IEnumerable<T> GetAll();

    Task<IEnumerable<T>> GetAllAsync();

    IEnumerable<T> Get(Expression<Func<T, bool>> expression);

    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);

    T? GetObject(Expression<Func<T, bool>> expression);

    Task<T?> GetObjectAsync(Expression<Func<T, bool>> expression);

    bool Add(T entity);

    Task<bool> AddAsync(T entity);

    bool AddMany(IEnumerable<T> entity);

    Task<bool> AddManyAsync(IEnumerable<T> entity);

    T? GetObject(params object[] pKeys);

    bool Update(T pObj);

    bool UpdateMany(IEnumerable<T> entity);

    Task<T?> GetObjectAsync(params object[] pKeys);

    Task<bool> UpdateAsync(T pObj);

    Task<T> AddReturnModelAsync(T entity);

    Task<bool?> DeleteByKey(Guid pKey);
}
