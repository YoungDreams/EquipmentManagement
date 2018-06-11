using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.UserMessages
{
    public class IndexViewModel
    {
        public IEnumerable<Entities.UserNotification> UndoMessages { get; set; }
        public IEnumerable<Entities.UserNotification> DoneMessages { get; set; }
    }
}