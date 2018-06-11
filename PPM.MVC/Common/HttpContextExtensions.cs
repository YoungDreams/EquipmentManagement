using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace PPM.Web.Common
{
    public static class HttpContextExtensions
    {
        public static string UploadFolder = HostingEnvironment.MapPath("~/upload");

        public static byte[] ReadBytes(this HttpPostedFileBase httpPostedFile)
        {
            var tmp = Guid.NewGuid().ToString("N") + ".tmp";
            var tmpFilePath = Path.Combine(UploadFolder, tmp);
            httpPostedFile.SaveAs(tmpFilePath);
            return File.ReadAllBytes(tmpFilePath);
        }
    }
}