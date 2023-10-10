using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Permissions
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        /*public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Acceso");
            }
            base.OnActionExecuting(filterContext);
        }*/
    }
}