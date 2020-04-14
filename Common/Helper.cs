using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Common
{
    /// <summary>
    /// 系统全局帮助基类
    /// </summary>
    public sealed class Helper
    {
        /// <summary>
        /// 判断字符串是否不为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNull(object str)
        {
            return (str != null) && (!str.Equals(""));
        }

        /// <summary>
        /// 判断字符串是不是时间类型格式
        /// </summary>
        public static bool IsDateTime(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是不是Decimal类型格式
        /// </summary>
        public static bool IsDecimal(string strDate)
        {
            try
            {
                Decimal.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceString(string str)
        {
            return
                str.Replace("\\", "").Replace("//", "").Replace(",", "").Replace(".", "").Replace(")", "").Replace("(",
                                                                                                                   "").
                    Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("。", "").Replace(":", "");
        }

        /// <summary>
        /// 获取当前URL后缀
        /// </summary>
        /// <returns></returns>
        public static string UrlEndString()
        {
            string urlApsx = HttpContext.Current.Request.Url.ToString();
            urlApsx = urlApsx.Substring(urlApsx.LastIndexOf('/'));
            urlApsx = urlApsx.Substring(1, urlApsx.IndexOf('.') - 1);
            return urlApsx;
        }

        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <returns></returns>
        public static string GetRootURL()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在Web站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }
        /// <summary>
        /// 将HTML代码替换为页面文本形式。
        /// </summary>
        /// <param name="str"></param>
        /// <remarks>
        /// <![CDATA[替换了：&、>、<、'、"、Tab、空格、换行符、回车符。]]>
        /// </remarks>
        public static string HtmlEnCode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else
            {
                var sb = new StringBuilder(str);
                sb.Replace(@"&", @"&amp;");
                sb.Replace(@">", @"&gt;");
                sb.Replace(@"<", @"&lt;");
                sb.Replace(Char.ConvertFromUtf32(32), @"&nbsp;");
                sb.Replace(Char.ConvertFromUtf32(9), @"&nbsp;&nbsp;&nbsp;&nbsp;");
                sb.Replace(Char.ConvertFromUtf32(34), @"&quot;");
                sb.Replace(Char.ConvertFromUtf32(39), @"&#39;");
                sb.Replace(Char.ConvertFromUtf32(13), @"");
                sb.Replace(Char.ConvertFromUtf32(10), @"<br />");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 将页面文本形式字符串还原成HTML代码。
        /// </summary>
        /// <param name="str"></param>
        /// <remarks>
        /// <![CDATA[替换了：&、>、<、'、"、Tab、空格、换行符、回车符。]]>
        /// </remarks>
        public static string HtmlDeCode(string str)
        {
            if (string.IsNullOrEmpty((str)))
            {
                return string.Empty;
            }
            else
            {
                var sb = new StringBuilder(str);
                sb.Replace(@"&amp;", @"&");
                sb.Replace(@"&gt;", @">");
                sb.Replace(@"&lt;", @"<");
                sb.Replace(@"&nbsp;&nbsp;&nbsp;&nbsp;", Char.ConvertFromUtf32(9));
                sb.Replace(@"&nbsp;", Char.ConvertFromUtf32(32));
                sb.Replace(@"&#39;", Char.ConvertFromUtf32(39));
                sb.Replace(@"<br />", Char.ConvertFromUtf32(10) + Char.ConvertFromUtf32(13));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Logons the user IP address.\
        /// 获取登陆用户的IP地址
        /// </summary>
        /// <returns></returns>
        public static string LogonUserIPAddress()
        {
            string ipAdress = string.Empty;
            IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            if (addressList.Length > 1)
            {
                ipAdress = addressList[0] + "--" + addressList[1];
            }
            else
            {
                ipAdress = addressList.Length < 1 ? "没有可用的连接" : addressList[0].ToString();
            }
            return ipAdress;
        }

        /// <summary>
        /// DataTable To XML
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>
        public static string ConvertDataTableToXML(DataTable xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.UTF8);
                xmlDS.WriteXml(writer, XmlWriteMode.WriteSchema);
                var count = (int)stream.Length;
                var arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                var utf = new UTF8Encoding();
                return utf.GetString(arr).Trim();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件路径:~/template/test.xml</param>
        /// <param name="fullName">文件名</param>
        public static void DownFile(string filePath, string fullName)
        {
            DownFullFile(HttpContext.Current.Server.MapPath(filePath), fullName);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        /// <param name="fullName">文件名</param>
        public static void DownFullFile(string filePath, string fullName)
        {
            string path = filePath;
            var file = new FileInfo(path);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                                                   "attachment; filename=" +
                                                   HttpUtility.UrlEncode(fullName, Encoding.UTF8));
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            // 指定返回的是一个不能被客户端读取的流，必须被下载 
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            // 把文件流发送到客户端 
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.Flush();
        }

        /// <summary>
        /// 获取枚举类型的描述列表（根据当前界面线程区域自动获取）。
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IDictionary<int, string> GetEnumDescriptions(Type enumType)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral)
                {
                    object[] objs = field.GetCustomAttributes(
                        typeof(DescriptionAttribute), false);
                    result.Add((int)field.GetValue(enumType),
                        ((DescriptionAttribute)objs[0]).Description);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取枚举值的描述信息。
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, int value)
        {
            IDictionary<int, string> descriptions = GetEnumDescriptions(enumType);
            try
            {
                return descriptions[value];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取枚举值的描述信息。
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, object value)
        {
            int iValue = 0;
            if (value != null &&
                Int32.TryParse(value.ToString(), out iValue))
            {
                return GetEnumDescription(enumType, iValue);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 替换字符串中的单引号。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceSinglequotes(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return (s.Trim().Replace("'", "''"));
        }

        /// <summary>
        /// 获取客户端的 IP 地址。
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPAddress()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 替换字符串中的单引号
        /// </summary>
        /// <param name="srcStr"></param>
        /// <returns></returns>
        public static string StringSqlSafe(string srcStr)
        {
            if (String.IsNullOrEmpty(srcStr))
            {
                return srcStr;
            }
            return srcStr.Replace("'", "''").Replace("[", "[[").Replace("]", "]]")
                .Replace("%", "[%]").Replace("_", "[_]");
        }

        /// <summary>
        /// 获取两个日期之间的差值。
        /// </summary>
        /// <param name="intervalFormat">计算部分：年(yy)，月(mm)或日(dd)。</param>
        /// <param name="date1">日期。</param>
        /// <param name="date2">日期。</param>
        /// <returns></returns>
        public static int DateInterval(string intervalFormat, DateTime date1, DateTime date2)
        {
            DateTime min;
            DateTime max;
            int year;
            int month;
            int y, m;
            if (date1 < date2)
            {
                min = date1;
                max = date2;
            }
            else
            {
                min = date2;
                max = date1;
            }
            y = max.Year;
            m = max.Month;
            if (max.Month < min.Month)
            {
                y--;
                m = m + 12;
            }
            year = y - min.Year;
            month = m - min.Month;
            switch (intervalFormat.ToLower())
            {
                case "y":
                case "yy":
                    return year;
                case "m":
                case "mm":
                    return month + year * 12;
                case "d":
                case "dd":
                    return max.Subtract(min).Days;
                default:
                    throw new ArgumentException("Invalid interval.");
            }
        }

        /// <summary>
        /// 计算当前日期所在的星期的第一天。
        /// </summary>
        /// <param name="date">要参与计算的日期。</param>
        /// <param name="firstWeekDay">以星期几作为一周的第一天。</param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDay(DateTime date, DayOfWeek firstWeekDay)
        {
            // 将枚举转换为整型进行计算间隔天数
            int firstDay = (int)firstWeekDay;
            int currentDay = (int)date.DayOfWeek;
            int intervalDays = currentDay - firstDay;

            if (intervalDays < 0) { intervalDays += 7; }
            return date.AddDays(-intervalDays);
        }

        /// <summary>
        /// 计算当前日期所在的星期的最后一天。
        /// </summary>
        /// <param name="date">要参与计算的日期。</param>
        /// <param name="firstWeekDay">以星期几作为一周的第一天。</param>
        /// <returns></returns>
        public static DateTime GetWeekLastDay(DateTime date, DayOfWeek firstWeekDay)
        {
            return GetWeekFirstDay(date, firstWeekDay).AddDays(6);
        }

        /// <summary>
        /// 将对象序列化成 XML 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化后的 XML 字符串。</returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    string str = sr.ReadToEnd();
                    writer.Close();
                    return str;
                }
            }
        }

        public static byte[] BinarySerialize(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                return bytes;
            }
        }

        /// <summary>
        /// 从 XML 字符串反序列化出对象。
        /// </summary>      
        /// <param name="xml">XML 字符串</param>    
        /// <param name="type">反序列化对象的类型。</param>    
        /// <returns>反序列化后的对象。</returns>
        public static object Deserialize(string xml, Type type)
        {
            object obj = null;
            XmlSerializer serializer = new XmlSerializer(type);
            using (TextReader reader = new StringReader(xml))
            {
                obj = serializer.Deserialize(reader);
            }

            return obj;
        }

        public static object Deserialize(byte[] bytes)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream(bytes))
            {
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>   
        /// 得到本周第一天(以星期一为第一天)   
        /// </summary>   
        /// <param name="datetime"></param>   
        /// <returns></returns>   
        public static DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天   
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。   
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天   
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        /// <summary>
        /// 验证日期是否是有效的（有效日期需大于1900-01-01）
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(object datetime)
        {
            //默认结果
            bool _IsTrue = false;

            //验证过程
            try
            {
                DateTime d;
                int y;

                if (DateTime.TryParse(datetime.ToString(), out d))
                {
                    if (d > new DateTime(1900, 1, 1)) _IsTrue = true;
                }
                else if (int.TryParse(datetime.ToString(), out y))
                {
                    if (y >= 1900) _IsTrue = true;
                }
            }
            catch
            {
                _IsTrue = false;
            }

            //返回结果
            return _IsTrue;
        }

        /// <summary>
        /// 检查文件夹，若没有就新建
        /// </summary>
        /// <param name="directory">文件夹</param>
        public static void CheckDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 返回文件夹，若没有就新建
        /// </summary>
        /// <param name="directory">文件夹</param>
        public static string ReturnDirectory(string directory)
        {
            CheckDirectory(directory);

            return directory;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDirectory">源文件夹</param>
        /// <param name="destDirectory">目标文件夹</param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            //判断源目录和目标目录是否存在，如果不存在，则创建一个目录
            CheckDirectory(sourceDirectory);
            CheckDirectory(destDirectory);

            //拷贝文件
            CopyFile(sourceDirectory, destDirectory);

            //拷贝子目录       
            //获取所有子目录名称
            string[] directionName = Directory.GetDirectories(sourceDirectory);

            foreach (string directionPath in directionName)
            {
                //根据每个子目录名称生成对应的目标子目录名称
                string directionPathTemp = destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);

                //递归下去
                CopyDirectory(directionPath, directionPathTemp);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceDirectory">源文件夹</param>
        /// <param name="destDirectory">目标文件夹</param>
        public static void CopyFile(string sourceDirectory, string destDirectory)
        {
            //获取所有文件名称
            string[] fileName = Directory.GetFiles(sourceDirectory);

            foreach (string filePath in fileName)
            {
                //根据每个文件名称生成对应的目标文件名称
                string filePathTemp = destDirectory + "\\" + filePath.Substring(sourceDirectory.Length + 1);

                //若不存在，直接复制文件；若存在，覆盖复制
                if (File.Exists(filePathTemp))
                {
                    File.Copy(filePath, filePathTemp, true);
                }
                else
                {
                    File.Copy(filePath, filePathTemp);
                }
            }
        }
    }
}