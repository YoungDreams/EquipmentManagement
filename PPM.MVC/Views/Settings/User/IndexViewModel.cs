using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PPM.Web.Common;
using PPM.Commands;
using PPM.Query;

namespace PPM.MVC.Views.Settings.User
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public UserQuery Query { get; set; }
        public PagedData<PPM.Entities.User> Items { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "User"),
                Command = new DeleteUserCommand { UserId = id }
            };
        }

        public object ResetPasswordCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("ResetPassword", "User"),
                Command = new ResetPasswordCommand { UserId = id }
            };
        }
    }
}