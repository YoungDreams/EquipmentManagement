using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Web.Mvc
{
    public class VMAuthorizeAttribute : AuthorizeAttribute
    {
        //GenericPrincipal
        private static readonly char[] _splitParameter = new[] { ',' };
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext.HttpContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                string path = filterContext.HttpContext.Request.Path;
                string loginUrl = "/Account/LogOn?returnUrl={0}";
                filterContext.HttpContext.Response.Redirect(string.Format(loginUrl, HttpUtility.UrlEncode(path)), true);
            }
            //base.HandleUnauthorizedRequest(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!string.IsNullOrEmpty(Rights))
            {
                var user = httpContext.User as IVMPrincipal;
                if (user != null && (_rightsSplite.Length > 0 && !_rightsSplite.Any(user.IsInRight)))
                {
                    return false;
                }
            }

            var result = base.AuthorizeCore(httpContext);
            return base.AuthorizeCore(httpContext);
        }

        private string _rights;
        private string[] _rightsSplite = new string[0];
        /// <summary>
        /// 获取或者设置允许用户访问的权限
        /// </summary>
        public string Rights
        {
            get { return _rights ?? string.Empty; }
            set
            {
                _rights = value;
                _rightsSplite = SplitString(value);
            }
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(_splitParameter)
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}
