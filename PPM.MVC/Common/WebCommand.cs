using Foundation.Messaging;

namespace PPM.Web.Common
{
    public class WebCommand
    {
        public string Url { get; set; }
        public ICommand Command { get; set; }
    }
}