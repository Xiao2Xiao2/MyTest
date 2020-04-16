using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTest.Filters
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 重写异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            //filterContext.Controller.ViewData["ErrorMessage"] = filterContext.Exception.Message;
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error",
            //    ViewData = filterContext.Controller.ViewData,
            //};
            //filterContext.ExceptionHandled = true;
        }
    }
}