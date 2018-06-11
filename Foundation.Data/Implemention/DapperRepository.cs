using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Foundation.Data.Implemention
{
    public class DapperRepository : IRepository
    {
        public IQueryable<T> Query<T>() where T : class
        {
            using (var connnection = OpenConnection())
            {
                return connnection.GetAll<T>().AsQueryable();
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null) where T : class
        {
            using (var connnection = OpenConnection())
            {
                return connnection.Query<T>(sql, param);
            }
        }

        public PagedData<T> QueryPaged<T>(string sql, string order, int page, int pageSize, object param = null) where T : class
        {
            var countSql = $"select count(*) from ({sql}) as tmp";
            var count = 0;
            using (var connnection = OpenConnection())
            {
                var command = new CommandDefinition(countSql, param);
                count = connnection.ExecuteScalar<int>(command);

                //   var pagedSql = $"{sql} order by {order ?? "id"} OFFSET {(page - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                var pagedSql = $@"WITH DataSource AS ({sql}) 
SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {order ?? "id" }) AS RowNumber, * FROM DataSource) AS Result 
WHERE RowNumber BETWEEN {(page - 1) * pageSize + 1} AND {page * pageSize}";

                return new PagedData<T>(page, pageSize, count, connnection.Query<T>(pagedSql, param));
            }
        }

        public PagedData<T> QueryPaged<T>(IQueryable<T> nhibernateQuery, int page, int pageSize) where T : class
        {
            return new PagedData<T>(page, pageSize, nhibernateQuery.Count(), nhibernateQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public PagedData<T> QueryPaged<T>(IList<T> nhibernateQuery, int page, int pageSize) where T : class
        {
            return new PagedData<T>(page, pageSize, nhibernateQuery.Count(), nhibernateQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public T Get<T>(int id) where T : class
        {
            using (var connnection = OpenConnection())
            {
                return connnection.Get<T>(id);
            }
        }

        public void Update<T>(T data) where T : Entity
        {
            using (var connnection = OpenConnection())
            {
                data.LastModifiedOn = DateTime.Now;
                var result = connnection.UpdateEx(data);
                if (!result)
                {
                    throw new Exception($"Update failed {data}");
                }
            }
        }

        public void Delete<T>(int id) where T : Entity
        {
            using (var connnection = OpenConnection())
            {
                var result = connnection.Delete(Get<T>(id));
                if (!result)
                {
                    throw new Exception($"Delete failed {id}");
                }
            }
        }

        public void Delete(Type entityType, int id)
        {
            using (var connnection = OpenConnection())
            {
                var result = connnection.DeleteEx(entityType, id);
                if (!result)
                {
                    throw new Exception($"Delete failed {id}");
                }
            }
        }

        public void Delete<T>(T data) where T : Entity
        {
            using (var connnection = OpenConnection())
            {
                var result = connnection.Delete(data);
                if (!result)
                {
                    throw new Exception($"Delete failed {data}");
                }
            }
        }

        public void Create(Entity data)
        {
            using (var connnection = OpenConnection())
            {
                data.LastModifiedOn = DateTime.Now;
                data.CreatedOn = DateTime.Now;
                var result = connnection.InsertEx(data);
                if (result == 0)
                {
                    throw new Exception($"Create failed {data}");
                }
            }
        }

        public T Scalar<T>(string sql, object param = null)
        {
            using (var connnection = OpenConnection())
            {
                return connnection.ExecuteScalar<T>(sql, param);
            }
        }

        protected IDbConnection OpenConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PPM"].ConnectionString);
            connection.Open();
            return connection;
        }

        public void Execute(string sql, object parames = null)
        {
            using (var connnection = OpenConnection())
            {
                connnection.Execute(sql, parames);
            }
        }
    }
}
