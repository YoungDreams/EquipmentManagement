using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.Customer
{
    public class EditAvatarViewModel
    {
        public int CustomerId { get; set; }
        public string AvatarFilePath { get; set; }
    }
}