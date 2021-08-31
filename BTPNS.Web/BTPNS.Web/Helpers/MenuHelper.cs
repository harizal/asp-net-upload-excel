using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTPNS.Helpers
{
    public static class MenuHelper
    {
        public static string IsMenuActive(this IHtmlHelper htmlHelper, string controller)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController);

            return returnActive ? "active" : "";
        }
    }
}
