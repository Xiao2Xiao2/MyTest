using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
namespace MyTest.Filters
{
    public class LogFilters : FilterAttribute, IActionFilter
    {
        private bool HttpLogEnabled
        {
            get {
                return ApplicationSettings.Get("HttpLogEnabled") == "true";
            }
        }
        /// <summary>
        /// 请求执行前
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpLogEnabled)
            {
                SystemExtends.LogWrite("IP:{0}-Controller:{1}-Action:{2}"
               .format(Text.NetworkIp, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName), "HttpLog");
            }
        }
        /// <summary>
        /// 请求执行后
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

    }
}