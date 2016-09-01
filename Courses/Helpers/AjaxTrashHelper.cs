using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Courses.Helpers
{
    public static class AjaxTrashHelper
    {
        public static IHtmlString TrashHelper(this AjaxHelper ajaxHelper, string linkText,
            string actionName, string controllerName, AjaxOptions ajaxOptions,
            object htmlAttributes)
        {
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, ajaxOptions, htmlAttributes);
        }
    }
}