using System;
using System.Linq;

namespace Foundation.Core
{
    public static class TypeExtensions
    {
        static public bool IsImplementsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(x => x == interfaceType || x.GetGenericTypeDefinition() == interfaceType);
        }
    }
}
