using System.IO;
using System.Web.Mvc;

namespace PPM.Web.Common
{
    public static class ControllerExtensions
    {
        public static string RenderView<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            var controllerContext = controller.ControllerContext;
            controllerContext.Controller.ViewData.Model = model;

            // To be or not to be (partial)
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(controllerContext, viewName) : ViewEngines.Engines.FindView(controllerContext, viewName, null);

            StringWriter stringWriter;

            using (stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(
                    controllerContext,
                    viewResult.View,
                    controllerContext.Controller.ViewData,
                    controllerContext.Controller.TempData,
                    stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
            }

            return stringWriter.ToString();
        }
    }
}