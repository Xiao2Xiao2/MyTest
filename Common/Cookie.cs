using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    /// <summary>
    /// 对Cookie操作进行封装
    /// </summary>
    public class Cookie
    {
        #region 获取Cookie值 public static HttpCookie Get(string name)
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie Get(string name)
        {           

            return HttpContext.Current.Request.Cookies[name];
        }
        #endregion

        #region 设置Cookie值 	public static HttpCookie Set(string name)
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie Set(string name)
        {            
            return new HttpCookie(name);//创建并命名新的Cookie
        }
        #endregion

        public static HttpCookie Reset(string name)
        {
            HttpCookie cookie = Get(name);
            if (cookie == null)
            {
                return Set(name);
            }
            else
            {
                return cookie;
            }
        }

        #region 保存Cookie值 public static void Save(HttpCookie cookie)
        /// <summary>
        ///  保存Cookie值
        /// </summary>
        /// <param name="cookie"></param>
        public static void Save(HttpCookie cookie)
        {
            string domain = Fetch.ServerDomain;//网站域名
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            if (domain != host)
            {
                cookie.Domain = domain;
            }
            if (Get(cookie.Name) != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 保存单值COOKIE，便于js调用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cookieValue"></param>
        public static void SetSingleCookie(string name, string cookieValue)
        {
            HttpCookie mcookie = Cookie.Reset(name);
            //mcookie.Domain = Fetch.ServerDomain;//网站域名 这个不能加，加了就保存不住了
            mcookie.Value = HttpUtility.UrlEncode(cookieValue);
            mcookie.Expires = DateTime.Now.AddDays(1);
            Cookie.Save(mcookie);
        }
        public static string GetSingCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return System.Web.HttpContext.Current.Server.UrlDecode(cookie.Value);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region 移除Cookie值 public static void Remove(HttpCookie cookie)
        /// <summary>
        ///移除Cookie值
        /// </summary>
        /// <param name="cookie"></param>
        public static void Remove(HttpCookie cookie)
        {
            if (cookie != null)
            {
                cookie.Expires = new System.DateTime(1983, 5, 21);
                cookie.Domain = Fetch.ServerDomain;//网站域名 
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                //System.Web.HttpContext.Current.Request.Cookies.Remove(cookie.Name);
            }
        }
        #endregion

        #region 移除Cookie值 public static void Remove(string name)
        /// <summary>
        ///移除Cookie值
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            Remove(Get(name));
        }
        #endregion

        #region 读取或写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = Text.UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = Text.UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = Text.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = Text.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return Text.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return Text.UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion


    }
}
