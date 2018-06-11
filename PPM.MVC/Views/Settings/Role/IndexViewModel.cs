using System.Collections.Generic;
using PPM.Query;

namespace PPM.MVC.Views.Settings.Role
{
    public class IndexViewModel
    {
        public RoleQuery Query { get; set; }
        public List<string> Items { get; set; }
    }
}