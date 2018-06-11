using System.Collections.Generic;

namespace PensionInsurance.Web.Views.Home
{
    public class NavHeaderViewModel
    {
        public IList<Entities.UserBacklog> UserBacklogs { get; set; }

        public IList<Entities.UserNotification> UserNotices { get; set; }
    }
}