using System.Collections.Generic;
using System.Linq;

namespace PensionInsurance.Web.Common
{
    public static class EnumerableExtensions
    {
        public static int RowNumberOf<T>(this IEnumerable<T> @this, T item)
        {
            return @this.ToList().IndexOf(item) + 1;
        }
    }
}