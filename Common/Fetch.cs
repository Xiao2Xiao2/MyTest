using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Common
{
    /// <summary>
    /// 对常用方法重新进行封装，及获取一些常用环境变量
    /// </summary>
    public class Fetch
    {
        #region Fields
        public static readonly string SiteMapPath;
        #endregion

        #region 构造函数,初始化变量前缀等。
        static Fetch()
        {
            //mTablePrefix = ApplicationSettings.Get("TablePrefix");
            //if (null == mTablePrefix || string.Empty == mTablePrefix)
            //{
            //    mTablePrefix = "meili_";
            //}
            //else
            //{
            //    mTablePrefix = mTablePrefix.Replace("'", "''");
            //}
            //SiteMapPath = Fetch.MapPath(".");
        }
        #endregion

        #region 获取页面提交的参数值，相当于 Request.Form public static string Post(string name)
        /// <summary>
        /// 获取页面提交的参数值，相当于 Request.Form
        /// </summary>
        public static string Post(string name)
        {
            string value = HttpContext.Current.Request.Form[name];
            return value == null ? string.Empty : value.Trim();
        }

        public static T Post<T>(string name)
        {
            T value = default(T);
            object str = HttpContext.Current.Request.Form[name];
            if (str != null)
            {
                if (value is ValueType)
                {
                    System.Reflection.MethodInfo parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        try
                        {
                            value = (T)parse.Invoke(null, new object[] { str.ToString() });
                        }
                        catch { }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)str;
                }
            }
            return value;
        }
        public static string Requestp(string name)
        {
            string value = HttpContext.Current.Request[name];
            return value == null ? string.Empty : value.Trim();
        }
        public static T Requestp<T>(string name)
        {
            T value = default(T);

            object str = HttpContext.Current.Request[name];
            if (str != null)
            {
                if (value is ValueType)
                {
                    System.Reflection.MethodInfo parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        try
                        {
                            value = (T)parse.Invoke(null, new object[] { str.ToString() });
                        }
                        catch { }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)str;
                }
            }
            return value;
        }
        #endregion

        #region 获取页面地址的参数值，相当于 Request.QueryString public static string Get(string name)
        /// <summary>
        /// 获取页面地址的参数值，相当于 Request.Params 此方法失败后返回空字符串
        /// </summary>
        public static string Get(string name)
        {
            string value = HttpContext.Current.Request.QueryString[name];  //三种状态一起获取，易混淆 querystring,post ,cookie
            return value == null ? string.Empty : value.Trim();
        }
        /// <summary>
        /// 获取页面地址的参数值，相当于 Request.Params 
        /// </summary>
        public static T Get<T>(string name)
        {
            T value = default(T);
            object str = HttpContext.Current.Request.QueryString[name];
            if (str != null)
            {
                if (value is ValueType)
                {
                    System.Reflection.MethodInfo parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        try
                        {
                            value = (T)parse.Invoke(null, new object[] { str.ToString() });
                        }
                        catch { }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)str;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取页面地址的参数值，相当于 Request.Params 
        /// </summary>
        public static T Get<T>(string name, T defaultValue)
        {
            T value = defaultValue;
            object str = HttpContext.Current.Request.QueryString[name];
            if (str != null)
            {
                if (value is ValueType)
                {
                    System.Reflection.MethodInfo parse = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                    {
                        try
                        {
                            value = (T)parse.Invoke(null, new object[] { str.ToString() });
                        }
                        catch { }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)str;
                }
            }
            return value;
        }
        #endregion

        #region 获取页面地址的参数值并检查安全性，相当于 Request.QueryString public static string Get(string name, CheckGetEnum chkType)
        /// <summary>
        /// 获取页面地址的参数值并检查安全性，相当于 Request.QueryString
        /// chkType 有 CheckGetEnum.Int， CheckGetEnum.Safety两种类型，
        /// CheckGetEnum.Int 保证参数是数字型
        /// CheckGetEnum.Safety 保证提交的参数值没有操作数据库的语句
        /// </summary>
        public static string Get(string name, CheckGetEnum chkType)
        {
            string value = Get(name);
            bool isPass = false;
            switch (chkType)
            {
                default:
                    isPass = true;
                    break;
                case CheckGetEnum.Int:
                    {
                        try
                        {
                            int.Parse(value);
                            isPass = RegExp.IsNumeric(value);
                        }
                        catch
                        {
                            isPass = false;
                        }
                        break;
                    }
                case CheckGetEnum.Safety:
                    isPass = RegExp.IsSafety(value);
                    break;
            }
            if (!isPass)
            {
                new Terminator().Throw("地址栏中参数“" + name + "”的值不符合要求或具有潜在威胁，请不要手动修改URL。");
                return string.Empty;
            }
            return value;
        }
        #endregion

        #region 跟踪调试输出一个对象 public static void  Trace(object obj)
        /// <summary>
        /// 跟踪调试输出一个对象
        /// </summary>
        public static void Trace(object obj)
        {
            HttpContext.Current.Response.Write(obj.ToString());
        }
        #endregion

        #region 将相对地址转化为绝对地址，进一步封装和优化 Server.MapPath public static string MapPath(string path)
        /// <summary>
        /// 将相对地址转化为绝对地址，进一步封装和优化 Server.MapPath
        /// </summary>
        public static string MapPath(string path)
        {
            if (RegExp.IsPhysicalPath(path))
            {
                return path;
            }

            if (null != HttpContext.Current)
            {
                if (!RegExp.IsRelativePath(path))
                {
                    return HttpContext.Current.Server.MapPath(path);
                }
#if DEBUG
                if (null == HttpContext.Current)
                {
                    throw new Exception("HttpContext.Current 为空引用");
                }
#endif
                return HttpContext.Current.Server.MapPath(PathUpSeek + "/" + path);
            }
            else if (SiteMapPath.Length > 0)
            {
                return Text.JoinString(SiteMapPath + "/" + path);
            }
            else
            {
                throw new Exception("HttpContext.Current 为空引用");
            }
        }
        #endregion

        #region IP 地址字符串形式转换成长整型 public static long Ip2Int(string ip)
        /// <summary>
        /// IP 地址字符串形式转换成长整型
        /// </summary>
        public static long Ip2Int(string ip)
        {
            if (!RegExp.IsIp(ip))
            {
                return -1;
            }
            string[] arr = ip.Split('.');
            long lng = long.Parse(arr[0]) * 16777216;
            lng += int.Parse(arr[1]) * 65536;
            lng += int.Parse(arr[2]) * 256;
            lng += int.Parse(arr[3]);
            return lng;
        }
        #endregion

        #region 获取客户端IP  public static string UserIp
        /// <summary>
        /// 获取客户端IP 
        /// </summary>
        public static string UserIp
        {
            get
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ip == null || ip == string.Empty)
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (ip.ToLower() == "localhost" || ip == "::1")
                    ip = "127.0.0.1";
                if (!RegExp.IsIp(ip))
                {
                    return "Unknown";
                }
                return ip;
            }
        }
        #endregion

        #region  获取访问者所使用的浏览器名 public static string UserBrowser
        /// <summary>
        /// 获取访问者所使用的浏览器名
        /// </summary>
        public static string UserBrowser
        {
            get
            {
                string agent = HttpContext.Current.Request.UserAgent;
                if (agent == null || agent == string.Empty)
                    return "Unknown";
                agent = agent.ToLower();

                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                if (agent.IndexOf("firefox") >= 0
                    || agent.IndexOf("firebird") >= 0
                    || agent.IndexOf("myie") >= 0
                    || agent.IndexOf("opera") >= 0
                    || agent.IndexOf("netscape") >= 0
                    || agent.IndexOf("msie") >= 0
                    )
                    return bc.Browser + bc.Version;

                return "Unknown";
            }
        }
        #endregion

        #region 获得网站域名 	public static string ServerDomain
        /// <summary>
        /// 获得网站域名
        /// </summary>
        public static string ServerDomain
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host.ToLower();
                string[] arr = host.Split('.');
                if (arr.Length < 3 || RegExp.IsIp(host))
                {
                    return host;
                }
                string domain = host.Remove(0, host.IndexOf(".") + 1);
                if (domain.StartsWith("com.") || domain.StartsWith("net.") || domain.StartsWith("org.") || domain.StartsWith("gov."))
                {
                    return host;
                }
                return domain;
            }
        }
        #endregion

        #region 获得当前路径
        /// <summary>
        /// 获得当前路径
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                string path = HttpContext.Current.Request.Path;
                path = path.Substring(0, path.LastIndexOf("/"));
                if (path == "/")
                {
                    return string.Empty;
                }
                return path;
            }
        }
        #endregion

        #region 获得网站虚拟根目录 	public static string PathUpSeek
        /// <summary>
        /// 获得网站虚拟根目录
        /// </summary>
        public static string PathUpSeek
        {
            get
            {
                string currentPath = CurrentPath;
                string lower_current_path = currentPath.ToLower();


#if DEBUG
                string[] arr = (Text.JoinString(ApplicationSettings.Get("PathUpSeek"), "|/Members|/Auction|/Ask|/News")).ToLower().Split('|');
#else
				string[] arr = (Text.JoinString(ApplicationSettings.Get("PathUpSeek"), "|/install|/upgrade")).ToLower().Split('|');
#endif


                foreach (string s in arr)
                {
                    if (null == s || 0 == s.Length)
                    {
                        continue;
                    }
                    if (s[0] != '/')
                    {
                        continue;
                    }
                    if (lower_current_path.EndsWith(s))
                    {
                        return currentPath.Remove(currentPath.Length - s.Length, s.Length);
                    }
                }

                int indexof = currentPath.IndexOf("/templates/");
                if (-1 != indexof)
                {
                    return currentPath.Substring(0, indexof);
                }
                return currentPath;
            }
        }
        #endregion

        #region 获得当前URL public static string CurrentUrl
        /// <summary> 
        /// 获得当前URL
        /// </summary>
        public static string CurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString();
            }
        }
        #endregion

        #region 获取当前请求的原始URL
        /// <summary>
        /// 获取当前请求的原始 URL
        /// </summary>
        public static string webCurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.RawUrl.ToString();
            }
        }
        #endregion

        #region 获得来源URL public static string Referrer
        /// <summary>
        /// 获得来源URL
        /// </summary>
        public static string Referrer
        {
            get
            {
                Uri uri = HttpContext.Current.Request.UrlReferrer;
                if (uri == null)
                {
                    return string.Empty;
                }
                return Convert.ToString(uri);
            }
        }
        #endregion

        #region 是否从其他连接向本域名连接 	public static bool IsPostFromAnotherDomain
        /// <summary>
        /// 是否从其他连接向本域名连接
        /// </summary>
        public static bool IsPostFromAnotherDomain
        {
            get
            {
                if (HttpContext.Current.Request.HttpMethod == "GET")
                {
                    return false;
                }
                return Referrer.IndexOf(ServerDomain) == -1;
            }
        }
        #endregion

        #region 是否从其他连接向本域名以POST方式提交表单 	public static bool IsGetFromAnotherDomain
        /// <summary>
        /// 是否从其他连接向本域名以POST方式提交表单
        /// </summary>
        public static bool IsGetFromAnotherDomain
        {
            get
            {
                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    return false;
                }
                return Referrer.IndexOf(ServerDomain) == -1;
            }
        }
        #endregion

        #region 是否被判断为机器人 public static bool IsRobots
        /// <summary>
        /// 是否被判断为机器人 
        /// </summary>
        public static bool IsRobots
        {
            get
            {
                return IsWebSearch();
            }
        }
        #endregion

        #region 搜索引擎来源判断 public static bool IsWebSearch()

        /// <summary>
        /// 搜索引擎来源判断
        /// </summary>
        private static string[] _WebSearchList = new string[] { "google", "isaac", "surveybot", "baiduspider", "yahoo", "yisou", "3721", "qihoo", "daqi", "ia_archiver", "p.arthur", "fast-webcrawler", "java", "microsoft-atl-native", "turnitinbot", "webgather", "sleipnir", "msn" };
        public static bool IsWebSearch()
        {
            string user_agent = HttpContext.Current.Request.UserAgent;
            if (null == user_agent || string.Empty == user_agent)
            {
                return true;
            }
            else
            {
                user_agent = user_agent.ToLower();
            }

            for (int i = 0; i < _WebSearchList.Length; i++)
            {
                if (-1 != user_agent.IndexOf(_WebSearchList[i]))
                {
                    return true;
                }
            }
            return UserBrowser.Equals("Unknown");
        }
        #endregion

        #region Url编码，相当于 Server.UrlEncode public static string UrlEncode(string url)
        /// <summary>
        /// Url编码，相当于 Server.UrlEncode
        /// </summary>
        public static string UrlEncode(string url)
        {
            return HttpContext.Current.Server.UrlEncode(url);
        }
        #endregion

        #region 返回论坛的物理路径，默认与网站同目录 public static string ForumDirectory()
        public static string ForumDirectory()
        {
            string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string[] p = path.Split("\\".ToCharArray());
            int arrlength = p.Length;
            if (arrlength > 0)
            {
                path = "";
                for (int i = 0; i < arrlength - 2; i++)
                {
                    path += p[i] + "/";
                }
            }
            return path + "bbs/";
        }
        #endregion

        #region 属性
        ///// <summary>
        ///// 变量前缀
        ///// </summary>
        //public static string TablePrefix
        //{
        //    get
        //    {
        //        return mTablePrefix;
        //    }
        //}

        ///// <summary>
        ///// 获取验证码文件的地址
        ///// </summary>
        //public static string ValidateCodeFileName
        //{
        //    get
        //    {
        //        string file_name = Config.Settings["ValidateCodeFileName"];
        //        if (null == file_name || 0 == file_name.Length)
        //        {
        //            file_name = "image.aspx";
        //        }
        //        return file_name;
        //    }
        //}
        #endregion

    }
}
