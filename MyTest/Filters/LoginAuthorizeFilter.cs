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
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
             if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
              || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //FormsIdentity formsi = (FormsIdentity)filterContext.HttpContext.User.Identity;
                //CustomIdentity cusIden = new CustomIdentity(formsi);
                //if (cusIden != null)
                //{

                //    _loginuser = cusIden.User;

                //}
            }
            else
            {
                //返回http状态码
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}