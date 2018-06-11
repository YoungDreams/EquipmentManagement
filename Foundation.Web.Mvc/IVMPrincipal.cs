using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Mvc
{
    public interface IVMPrincipal : IPrincipal
    {
        bool IsInRight(string right);
    }
}
