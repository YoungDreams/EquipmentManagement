using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using Dapper;
using NHibernate;
using NHibernate.Transform;

namespace Foundation.Data.NHibernateExtensions
{
    public static class SqlQueryExtensions
    {
        internal static void AddDynamicParameters(this ISQLQuery query, object parmameters)
        {
            if (parmameters != null)
            {
                var values = new RouteValueDictionary(parmameters);

                foreach (var name in values.Keys.Where(x => query.NamedParameters.Contains(x)))
                {
                    query.SetParameter(name, values[name]);
                }
            }
        }
    }

    public static class SessionExtensions
    {
        public static IEnumerable<T> ExecuteQuery<T>(this ISession session, string sql, object param)
        {
            //NHibernate 用:表示命名参数
            return session.Connection.Query<T>(sql, param);
        }

        public static T ExecuteScalar<T>(this ISession session, string sql, object param)
        {
            //NHibernate 用:表示命名参数
            return session.Connection.ExecuteScalar<T>(sql, param);
        }

        public static int ExecuteNonQuery(this ISession session, string sql, object param)
        {
            //NHibernate 用:表示命名参数
            return session.Connection.Execute(sql, param);
        }
    }

    public class SimpleResultTransformer : IResultTransformer
    {
        private Type _resultType;
        private List<PropertyInfo> properties = new List<PropertyInfo>();

        public IList TransformList(IList collection)
        {
            return collection;
        }
        public SimpleResultTransformer(Type resultType)
        {
            this._resultType = resultType;
            foreach (var prop in resultType.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.Public))
            {
                properties.Add(prop);
            }
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            object instance = Activator.CreateInstance(_resultType);
            for (int i = 0; i < tuple.Length; i++)
            {
                var value = tuple[i];
 
                properties.SingleOrDefault(x => x.Name == aliases[i])?.SetValue(instance, tuple[i], null);
            }
            return instance;
        }
    }

}
