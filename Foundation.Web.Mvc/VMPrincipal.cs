using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Mvc
{
    [ComVisible(true)]
    [Serializable]
    public class VMPrincipal : GenericPrincipal, IVMPrincipal
    {
        private readonly string[] _rights;

        public VMPrincipal(IIdentity identity, string[] roles, string[] rights)
            : base(identity, roles)
        {
            _rights = rights;
        }

        public bool IsInRight(string right)
        {
            return _rights.Contains(right);
        }
    }
}
