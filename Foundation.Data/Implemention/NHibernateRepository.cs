using Foundation.Core;
using Foundation.Data.NHibernateExtensions;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Data.Implemention
{
    public class NHibernateRepository : IRepository
    {
        private readonly IProvider<ISession> _sessionProvider;
        protected ISession Session => _sessionProvider.Get();

        public NHibernateRepository(IProvider<ISession> sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Session.Query<T>();
        }

        public IEnumerable<T> Query<T>(string sql, object param = null) where T : class
        {
            return Session.ExecuteQuery<T>(sql, param);
        }

        public PagedData<T> QueryPaged<T>(string sql, string order, int page, int pageSize, object param = null) where T : class
        {
            var countSql = $"select count(*) from ({sql}) as tmp";
            var count = Session.ExecuteScalar<int>(countSql, param);

            //   var pagedSql = $"{sql} order by {order ?? "id"} OFFSET {(page - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var pagedSql = $@"WITH DataSource AS ({sql}) 
SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {order ?? "id" }) AS RowNumber, * FROM DataSource) AS Result 
WHERE RowNumber BETWEEN {(page - 1) * pageSize + 1} AND {page * pageSize}";

            return new PagedData<T>(page, pageSize, count, Session.ExecuteQuery<T>(pagedSql, param));
        }

        public PagedData<T> QueryPaged<T>(IQueryable<T> nhibernateQuery, int page, int pageSize) where T : class
        {
            return new PagedData<T>(page, pageSize, nhibernateQuery.Count(), nhibernateQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public T Get<T>(int id) where T : class
        {
            return Session.Get<T>(id);
        }


        public void Update<T>(T data) where T : Entity
        {
            data.LastModifiedOn = DateTime.Now;
            Session.SaveOrUpdate(data);
        }

        void IRepository.Delete<T>(int id)
        {
            Delete<T>(id);
        }

        public void Delete(Type entityType, int id)
        {
            Session.Delete(Session.Get(entityType, id));
        }

        void IRepository.Delete<T>(T data)
        {
            Delete(data);
        }

        public void Create(Entity data)
        {
            data.LastModifiedOn = DateTime.Now;
            data.CreatedOn = DateTime.Now;
            Session.Save(data);
        }

        public T Scalar<T>(string sql, object param = null)
        {
            return Session.ExecuteScalar<T>(sql, param);
        }

        public void Execute(string sql, object parames = null)
        {
            Session.ExecuteNonQuery(sql, parames);
        }

        void IRepository.Update<T>(T data)
        {
            Update(data);
        }

        public void Delete<T>(int id) where T : class
        {
            Delete(Get<T>(id));
        }


        public void Delete<T>(T data)
        {
            Session.Delete(data);
        }


        public void Delete(string filter)
        {
            Session.Delete(filter);
        }
    }

    public static class Extensions
    {
        public static PagedData<TSource> ToPagedData<TSource>(this IQueryable<TSource> nhibernateQuery, int page, int pageSize)
        {
            return new PagedData<TSource>(page, pageSize, nhibernateQuery.Count(), nhibernateQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public static PagedData<TSource> ToPagedData<TSource>(this IEnumerable<TSource> nhibernateQuery, int page, int pageSize)
        {
            return new PagedData<TSource>(page, pageSize, nhibernateQuery.Count(), nhibernateQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
    }
}