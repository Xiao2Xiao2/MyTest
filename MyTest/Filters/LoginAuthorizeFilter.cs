using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyTest.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class LoginAuthorizeFilter : AuthorizeAttribute,IAuthorizationFilter
    {
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    base.HandleUnauthorizedRequest(filterContext);
        //    if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //    {
        //        return;
        //    }
        //    if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {

        //    }
        //    else
        //    {
        //        //返回http状态码
        //        filterContext.HttpContext.Response.StatusCode = 401;
        //        filterContext.HttpContext.Response.End();
        //    }
        //}



        //}
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
          || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //返回http状态码
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
        }
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    base.OnAuthorization(filterContext);

        //    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Error")
        //    {
        //        return;
        //    }
        //    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName=="Home"&& filterContext.ActionDescriptor.ActionName =="Default")
        //    {
        //        return;
        //    }
        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {

        //        if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //        {
        //            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Login")
        //            {
        //                ViewResult vr = new ViewResult();
        //                vr.ViewName = "/Views/Error/Http401.cshtml";
        //                //vr.ViewName = "/Views/Shared/ToLogin.cshtml";
        //                filterContext.Result = vr;
        //                //filterContext.Result = new RedirectResult("/Error/Http401");
        //                //filterContext.HttpContext.Response.Write("<script>location.href = '/Error/Http401';</script>");
        //                //filterContext.HttpContext.Response.End();
        //            }
        //            else
        //            {
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            //返回http状态码
        //            filterContext.HttpContext.Response.StatusCode = 401;
        //            filterContext.HttpContext.Response.End();
        //        }

        //    }
        //}
    }
}