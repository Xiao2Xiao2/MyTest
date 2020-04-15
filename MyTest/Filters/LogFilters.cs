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
        /// <summary>
        /// 请求执行前
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            SystemExtends.LogWrite("IP:{0}-Controller:{1}-Action:{2}".format(Text.UserIp, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName));
            //filterContext.Controller.ViewData["ExecutingLogger"] = "正要添加公告，已以写入日志！时间：" + DateTime.Now;
        }
        /// <summary>
        /// 请求执行后
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //filterContext.Controller.ViewData["ExecutedLogger"] = "公告添加完成，已以写入日志！时间：" + DateTime.Now;
        }

    }
}