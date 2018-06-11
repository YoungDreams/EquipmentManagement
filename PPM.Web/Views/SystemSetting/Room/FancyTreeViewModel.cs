using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.SystemSetting.Room
{
    public class FancyTreeViewModel
    {
        public string title { get; set; }

        public bool folder { get; set; }
        public bool expanded { get; set; }
        public FancyTreeViewModelData data { get; set; }

        public List<FancyTreeViewModel> children { get; set; }
    }

    public class FancyTreeViewModelData
    {
        public int Id { get; set; }
        public string Kind { get; set; }
    }

}