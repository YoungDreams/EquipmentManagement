using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Data
{
    public interface IFetcher
    {
        IQueryable<T> Query<T>() where T : class;
        IEnumerable<T> Query<T>(string sql, object param = null) where T : class;
        PagedData<T> QueryPaged<T>(string sql, string order, int page, int pageSize, object param = null) where T : class;
        PagedData<T> QueryPaged<T>(IQueryable<T> nhibernateQuery, int page, int pageSize) where T : class;
        T Get<T>(int id) where T : class;
        T Scalar<T>(string sql, object param = null);
    }

    public interface IRepository : IFetcher
    {
        void Update<T>(T data) where T : Entity;
        void Delete<T>(int id) where T : Entity;
        void Delete(Type entityType, int id);
        void Delete<T>(T data) where T : Entity;
        void Create(Entity data);
        
        void Execute(string sql, object parames = null);
    }
}
