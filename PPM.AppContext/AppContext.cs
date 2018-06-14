using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Foundation.Core;
using Foundation.Data;
using NHibernate;
using NHibernate.Linq;
using PPM.Entities;

namespace PPM.Shared
{
    public class WebAppContext
    {
        private static IWindsorContainer _container;
        public User User => _container.Resolve<IWebAppContext>().User;


        public static IWebAppContext Current => _container.Resolve<IWebAppContext>();

        public static void InitForWebApplication(IWindsorContainer container)
        {
            container.Register(
                Component.For<HttpContextBase>().UsingFactoryMethod(k => new HttpContextWrapper(HttpContext.Current)).LifestylePerWebRequest(),
                Component.For<IWebAppContext>().ImplementedBy<WebAppcontextImpl>().LifestylePerWebRequest()
            );
            _container = container;
        }

        public static void InitForNonWebApplication(IWindsorContainer container)
        {
            container.Register(Component.For<IWebAppContext>().ImplementedBy<ThreadAppContext>().LifestyleScoped());
            _container = container;
        }
    }

    public interface IWebAppContext
    {
        User User { get; }
    }

    internal class WebAppcontextImpl : IWebAppContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IWindsorContainer _container;


        public WebAppcontextImpl(HttpContextBase httpContext, IWindsorContainer container)
        {
            _httpContext = httpContext;
            _container = container;
        }

        private User _user;
        public User User
        {
            get
            {
                if (_user != null) return _user;

                var tokenValue = _httpContext.Request.Headers["Token"];
                _user = !string.IsNullOrWhiteSpace(tokenValue)
                    ? _container.Resolve<IFetcher>().Query<UserToken>().First(x => x.AuthToken == tokenValue).User
                    : _container.Resolve<IFetcher>()
                        .Query<User>()
                        .First(x => x.Username == _httpContext.User.Identity.Name);

                return _user;
            }
        }
    }

    internal class ThreadAppContext : IWebAppContext
    {
        private readonly IWindsorContainer _container;

        public ThreadAppContext(IWindsorContainer container)
        {
            _container = container;
        }

        private User _user;
        public User User
        {
            get
            {
                if (_user == null)
                {
                    var query = _container.Resolve<ISession>().Query<User>();
                    query = query.FetchMany(x => x.Roles).ThenFetchMany(x => x.RolePermissions);
                    _user = query.First(x => x.Username == Thread.CurrentPrincipal.Identity.Name);
                    return _user;
                }
                return _user;
            }
        }
    }

    public static class UserExtensions
    {
        public static bool HasPermission(this User user, ModuleType module, Permission permission)
        {
            if (user.Roles.Count > 0 && user.RoleTypeIs(RoleType.管理员))
            {
                return true;
            }

            return user.Roles.SelectMany(x => x.RolePermissions).Any(x => x.ModuleType == module && x.Permission == permission);
        }

        public static bool HasPermission(this User user, List<ModuleType> modules, Permission permission)
        {
            if (user.Roles.Count > 0 && user.RoleTypeIs(RoleType.管理员))
            {
                return true;
            }

            return user.Roles.SelectMany(x => x.RolePermissions).Any(x => modules.Contains(x.ModuleType) && x.Permission == permission);
        }

        public static bool HasModule(this User user, ModuleType module)
        {
            if (user.RoleTypeIs(RoleType.管理员))
            {
                return true;
            }

            return user.Roles.SelectMany(x => x.RolePermissions).Any(x => x.ModuleType == module);
        }

        public static bool HasModule(this User user, List<ModuleType> modules)
        {
            if (user.RoleTypeIs(RoleType.管理员))
            {
                return true;
            }

            return user.Roles.SelectMany(x => x.RolePermissions).Any(x => modules.Contains(x.ModuleType));

        }

        //public static string GetSqlFilterFor(this User user, string userIdFieldName = "UserId", string projectIdFieldName = "ProjectId")
        //{
        //    if (user.RoleTypeIs(RoleType.超级管理员))
        //    {
        //        return " 1=1 ";
        //    }

        //    if (user.RoleTypeIs(RoleType.销售人员))
        //    {
        //        return $" {userIdFieldName}={user.Id} ";
        //    }

        //    return $"{projectIdFieldName} IN ({string.Join(",", user.Projects.Select(x => x.Id))})";
        //}

        //public static string GetProjectFilterFor(this User user, string projectIdFieldName = "ProjectId")
        //{
        //    if (user.RoleTypeIs(RoleType.超级管理员))
        //    {
        //        return " 1=1 ";
        //    }

        //    return $"{projectIdFieldName} IN ({string.Join(",", user.Projects.Select(x => x.Id))})";
        //}

        //public static bool HasPermissionForProject(this User user, int projectId)
        //{
        //    return user.RoleTypeIs(RoleType.超级管理员) || user.Projects.Any(x => x.Id == projectId);
        //}

        //public static List<Project> GetAllowedProjects(this User user)
        //{
        //    return user.RoleTypeIs(RoleType.超级管理员)
        //        ? ServiceLocator.Current.Resolve<IFetcher>().Query<Project>().OrderBy(o => o.Sort).ToList()
        //        : user.Projects.OrderBy(o => o.Sort).ToList();
        //}

        public static bool RoleTypeIs(this User user, RoleType roleType)
        {
            return user.Roles.Any(x => x.RoleType == roleType);
        }
    }
}