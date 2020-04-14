using System.Web;
using System.Web.Mvc;
using MyTest.Filters;
namespace MyTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //添加日志过滤器
            filters.Add(new LogFilters());
            //添加授权过滤器
            filters.Add(new LoginAuthorizeFilter());
        }
    }
}