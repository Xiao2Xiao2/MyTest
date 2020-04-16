using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Common
{
    /// <summary>
    /// 常用文本处理方法
    /// </summary>
    public class Text
    {
        #region 32 位 MD5 加密，返回加密后的字符串（小写） public static string MD5(string s)
        /// <summary>
        /// 32 位 MD5 加密 
        /// </summary>
        public static string MD5(string s)
        {
            if (null == s || 0 == s.Length)
            {
                s = string.Empty;
            }

            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5").ToLower();
        }
        #endregion

        #region Base-64加密，返回加密后的字符串 public static string EncodeBase64(string s)
        public static string EncodeBase64(string s)
        {
            string encode = "";
            byte[] bytes = System.Text.Encoding.GetEncoding(54936).GetBytes(s);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = s;
            }
            return encode;
        }
        #endregion

        #region Base-64解密，返回解密后的字符串 public static string DecodeBase64(string s)
        public static string DecodeBase64(string s)
        {
            string decode = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(s);
                decode = System.Text.Encoding.GetEncoding(54936).GetString(bytes);
            }
            catch
            {
                decode = s;
            }
            return decode;
        }
        #endregion

        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        #region DES加密字符串  public static string EncryptDES(string encryptString, string encryptKey)
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        public static string EncryptDES(string encryptString)
        {
            return EncryptDES(encryptString, "TaoBaoPW");//后面这个key不要改，改了以前的用户组都不能用了
        }

        #endregion

        #region DES解密字符串  public static string DecryptDES(string decryptString, string decryptKey)
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        public static string DecryptDES(string decryptString)
        {
            return DecryptDES(decryptString, "TaoBaoPW");//后面这个key不要改，改了以前的用户组都不能用了
        }
        #endregion

        #region 自定义密钥加密字符串

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "iskey");
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        #endregion

        #region 自定义密钥解密字符串
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "iskey");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region 获取DV模式的MD5密码 public static string MD5(string s,int start,int length)
        /// <summary>
        /// 获取DV模式的MD5密码
        /// </summary>
        public static string MD5(string s, int start, int length)
        {
            if (null == s || 0 == s.Length)
            {
                s = string.Empty;
            }

            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5").ToLower().Substring(start, length);
        }
        #endregion

        #region 以 32 位 MD5 加密加CookieToken后缀的形式产生 cookie 密文 public static string GenerateToken(string s)
        /// <summary>
        /// 以 32 位 MD5 加密加CookieToken后缀的形式产生 cookie 密文
        /// </summary>
        public static string GenerateToken(string s)
        {
            if (null == s || 0 == s.Length)
            {
                s = string.Empty;
            }

            return MD5(s + ApplicationSettings.Get("CookieToken"));
        }
        #endregion

        #region 密码比较，支持 32位，16位密码比较 public static bool ComparePassword(string pwd1, string pwd2)

        /// <summary>
        /// 密码比较，支持 32位，16位密码比较
        /// </summary>
        public static bool ComparePassword(string pwd1, string pwd2)
        {

            if (null == pwd1 && null == pwd2)
            {
                return true;
            }
            else if (null == pwd1 || null == pwd2)
            {
                return false;
            }

            int len1 = pwd1.Length, len2 = pwd2.Length;
            if (len1 == len2 && 0 != len1)
            {
                // 执行不区分大小写的检查
                return (0 == string.Compare(pwd1, pwd2, true));
            }
            else if (32 == len1 && 16 == len2)
            {
                // 执行不区分大小写的检查
                return (0 == string.Compare(pwd1.Substring(8, 16), pwd2, true));
            }
            else if (16 == len1 && 32 == len2)
            {
                // 执行不区分大小写的检查
                return (0 == string.Compare(pwd2.Substring(8, 16), pwd1, true));
            }


            return false;
        }
        #endregion

        #region 为字符串加0  public static string AddZero(int i)
        /// <summary>
        /// add zero
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string AddZero(int i)
        {
            return (i > 9 ? string.Empty : "0") + i;
        }
        #endregion

        #region 编码成 sql 文本可以接受的格式
        /// <summary>
        /// 编码成 sql 文本可以接受的格式
        /// </summary>
        public static string SqlEncode(string s)
        {
            if (null == s || 0 == s.Length)
            {
                return string.Empty;
            }
            return s.Trim().Replace("'", "''").Replace("--", "－－");
        }
        #endregion

        #region 过滤脏词 public static string ShitEncode(string s)
        /// <summary>
        /// 过滤脏词，默认有为“妈的|你妈|他妈|妈b|妈比|fuck|shit|我日|法轮|我操”
        /// 可以在 web.config 里设置 BadWords 的值
        /// </summary>
        public static string ShitEncode(string s)
        {
            string bw = null;
            if (bw == null || 0 == bw.Length)
            {
                bw = "妈的|你妈|他妈|妈b|妈比|fuck|shit|我日|法轮|我操|妈妈的|我靠|操|fuck|sb|bitch|他妈的|性爱|操你妈|三级片|sex|腚|妓|娼|阴蒂|奸|尻|贱|婊|靠|叉|龟头|屄|赑|妣|肏|尻|屌";
            }
            else
            {
                bw = Regex.Replace(bw, @"\|{2,}", "|");
                bw = Regex.Replace(bw, @"(^\|)|(\|$)", string.Empty);
            }
            return Regex.Replace(s, bw, "**", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 文本编码 	public static string TextEncode(string s)
        /// <summary>
        /// 文本编码
        ///</summary>
        public static string TextEncode(string s)
        {
            if (null == s || 0 == s.Length)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            sb.Replace("\'", "&#39;");
            return ShitEncode(sb.ToString());
        }
        #endregion

        #region HTML 编码 public static string HtmlEncode(string s)
        /// <summary>
        /// HTML 编码
        ///</summary>
        public static string HtmlEncode(string s)
        {
            return HtmlEncode(s, false);
        }

        public static string HtmlEncode(string s, bool bln)
        {
            if (null == s || 0 == s.Length)
            {
                return s;
            }

            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            sb.Replace("\'", "&#39;");

            if (bln)
            {
                return ShitEncode(sb.ToString());
            }
            else
            {
                return sb.ToString();
            }
        }
        #endregion

        #region HTML 解码 public static string HtmlDecode(string s)
        /// <summary>
        /// HTML 解码
        ///</summary>
        public static string HtmlDecode(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("&amp;", "&");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&quot;", "\"");
            sb.Replace("&#39;", "'");

            return sb.ToString();
        }
        #endregion

        #region TEXT解码		public static string TextDecode(string s)
        /// <summary>
        /// TEXT解码
        /// </summary>
        public static string TextDecode(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("<br/><br/>", "\r\n");
            sb.Replace("<br/>", "\r");
            sb.Replace("<p></p>", "\r\n\r\n");
            return sb.ToString();
        }
        #endregion

        #region 字符串的长度 public static int Len(string s)
        /// <summary>
        /// 字符串的长度
        /// </summary>
        public static int Len(string s)
        {
            return HttpContext.Current.Request.ContentEncoding.GetByteCount(s);
        }
        #endregion

        #region 截断字符串 public static string Left(string s, int need, bool encode)
        /// <summary>
        /// 截断字符串，如果str 的长度超过 need，则提取 str 的前 need 个字符，并在尾部加 “...”
        /// </summary>
        public static string Left(string s, int need, bool encode)
        {
            if (s == null || s == "")
            {
                return string.Empty;
            }

            int len = s.Length;
            if (len < need / 2)
            {
                return encode ? TextEncode(s) : s;
            }

            int i, j, bytes = 0;
            for (i = 0; i < len; i++)
            {
                bytes += RegExp.IsUnicode(s[i].ToString()) ? 2 : 1;
                if (bytes >= need)
                {
                    break;
                }
            }

            string result = s.Substring(0, i);
            if (len > i)
            {
                for (j = 0; j < 5; j++)
                {
                    bytes -= RegExp.IsUnicode(s[i - j].ToString()) ? 2 : 1;
                    if (bytes <= need)
                    {
                        break;
                    }
                }
                result = s.Substring(0, i - j) + "...";
            }
            return encode ? TextEncode(result) : result;
        }
        #endregion

        #region 截断字符串 public static string Left(string s, int need, bool encode,string tail)
        /// <summary>
        /// 截断字符串，如果str 的长度超过 need，则提取 str 的前 need 个字符
        /// </summary>
        public static string Left(string s, int need, bool encode, string tail)
        {
            if (s == null || s == "")
            {
                return string.Empty;
            }

            int len = s.Length;
            if (len < need / 2)
            {
                return encode ? TextEncode(s) : s;
            }

            int i, j, bytes = 0;
            for (i = 0; i < len; i++)
            {
                bytes += RegExp.IsUnicode(s[i].ToString()) ? 2 : 1;
                if (bytes >= need)
                {
                    break;
                }
            }

            string result = s.Substring(0, i);
            if (len > i)
            {
                for (j = 0; j < 5; j++)
                {
                    bytes -= RegExp.IsUnicode(s[i - j].ToString()) ? 2 : 1;
                    if (bytes <= need)
                    {
                        break;
                    }
                }
                result = s.Substring(0, i - j) + tail;
            }
            return encode ? TextEncode(result) : result;
        }
        #endregion

        #region Email 编码 public static string EmailEncode(string s)
        /// <summary>
        /// Email 编码
        /// </summary>
        public static string EmailEncode(string s)
        {
            string email = (TextEncode(s)).Replace("@", "&#64;").Replace(".", "&#46;");

            return JoinString("<a href='mailto:", email, "'>", email, "</a>");
        }
        #endregion

        #region 高效字符串连接操作 public static string JoinString(params string[] value)
        /// <summary>
        /// 高效字符串连接操作。
        /// </summary>
        /// <param name="value">要连接的字符串</param>
        /// <returns>连接后的字符串</returns>
        public static string JoinString(params string[] value)
        {
            if (null == value)
            {
                throw new System.ArgumentNullException("value");
            }
            if (0 == value.Length)
            {
                return string.Empty;
            }
            return string.Join(string.Empty, value);
        }
        #endregion

        #region 判断一个字符串是否是一个由逗号分隔的数字列表 public static bool IsNumberList(string str)
        /// <summary>
        /// 判断一个字符串是否是一个由逗号分隔的数字列表。
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNumberList(string str)
        {
            return IsNumberList(str, ',');
        }

        /// <summary>
        /// 判断一个字符串是否是一个由指定的字符分隔的数字列表。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static bool IsNumberList(string str, char separator)
        {
            if (null == str)
            {
                return false;
            }
            int len = str.Length;
            if (0 == len)
            {
                return false;
            }
            if (!char.IsNumber(str[0]) || !char.IsNumber(str[len - 1]))
            {
                return false;
            }
            len--;
            for (int i = 1; i < len; i++)
            {
                if (separator == str[i])
                {
                    if (!char.IsNumber(str[i - 1]) || !char.IsNumber(str[i + 1]))
                    {
                        return false;
                    }
                }
                else if (!char.IsNumber(str[i]))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 判断目标对象是否由数字组成 public static bool IsNumeric(object value)
        /// <summary>
        /// 判断目标对象是否由数字组成
        /// </summary>
        /// <param name="value">要判断的目标对象。</param>
        /// <returns>判断成功返回 true ， 否则返回 false 。</returns>
        public static bool IsNumeric(object value)
        {
            if (null == value)
            {
                return false;
            }
            return IsNumeric(value.ToString());
        }
        #endregion

        #region 判断字符串是否由数字组成 	public static bool IsNumeric(string value)
        /// <summary>
        /// 判断字符串是否由数字组成
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>判断成功返回 true ， 否则返回 false 。</returns>
        public static bool IsNumeric(string value)
        {
            if (null == value)
            {
                return false;
            }
            int len = value.Length;
            if (0 == len)
            {
                return false;
            }
            if ('-' != value[0] && !char.IsNumber(value[0]))
            {
                return false;
            }
            for (int i = 1; i < len; i++)
            {
                if (!char.IsNumber(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region JavaScript 编码 public static string JavaScriptEncode(string str)
        /// <summary>
        /// JavaScript 编码
        /// </summary>
        public static string JavaScriptEncode(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("\\", "\\\\");
            sb.Replace("\r", "\\0Dx");
            sb.Replace("\n", "\\x0A");
            sb.Replace("\"", "\\x22");
            sb.Replace("\'", "\\x27");
            return sb.ToString();
        }
        #endregion

        #region JavaScript 编码 public static string JavaScriptEncode(object obj)
        /// <summary>
        /// JavaScript 编码
        /// </summary>
        public static string JavaScriptEncode(object obj)
        {
            if (null == obj)
            {
                return string.Empty;
            }
            return JavaScriptEncode(obj.ToString());
        }
        #endregion

        #region 去除 htmlCode 中所有的HTML标签(包括标签中的属性) public static string StripHtml(string htmlCode)
        /// <summary>
        /// 去除 htmlCode 中所有的HTML标签(包括标签中的属性)
        /// </summary>
        /// <param name="htmlCode">包含 HTML 代码的字符串。</param>
        /// <returns>返回一个不包含 HTML 代码的字符串</returns>
        public static string StripHtml(string htmlCode)
        {
            if (null == htmlCode || 0 == htmlCode.Length)
            {
                return string.Empty;
            }
            return Regex.Replace(htmlCode, @"<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
        #endregion

        #region 新闻内容分页
        /// <summary>
        /// 新闻内容分页
        /// </summary>
        /// <param name="n_ID">新闻ID</param>
        /// <param name="content">内容</param>
        /// <param name="currentpage">当前页</param>
        /// <returns></returns>
        public static string PageCount(int n_ID, string content, int currentpage)
        {
            string p = "\\[page\\]";

            int Pages;

            if (content.IndexOf("[page]") != -1)
            {
                string[] arrContent = Regex.Split(content, p, RegexOptions.IgnoreCase);
                Pages = arrContent.Length;
                if (currentpage > Pages - 1)
                    return "<span style=\"color:red\">页码参数错误</span>";
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(arrContent[currentpage].ToString());
                    sb.Append("<div id=\"newspager\">");

                    //显示上一页按钮
                    if (currentpage == 0)
                    {
                        sb.Append("<ul><li class=\"qh\">上一页</li>");
                    }
                    else
                    {
                        sb.AppendFormat("<li class=\"w\" onclick=\"jump({0},{1})\"><img src=\"/images/news/pagre_left.gif\" align=\"absmiddle\" />&nbsp;上一页</li>", n_ID, currentpage - 1);
                    }
                    //循环显示页码数按钮
                    for (int i = 0; i < Pages; i++)
                    {
                        if (i == currentpage)
                            sb.AppendFormat("<li class=\"n\">{0}</li>", i + 1);
                        else
                            sb.AppendFormat("<li id=\"li" + i.ToString() + "\" class=\"y\"  onclick=\"jump({0},{1})\" onmouseover=\"mouseovercss(" + i.ToString() + ");\" onmouseout=\"mouseoutcss(" + i.ToString() + ");\" >{2}</li>", n_ID, i, (i + 1).ToString());
                    }

                    //显示下一页按钮
                    if (currentpage == Pages - 1)
                        sb.Append("<li class=\"qh\">末页</li>");
                    else
                        sb.AppendFormat("<li class=\"w\" onclick=\"jump({0},{1})\">下一页&nbsp;<img src=\"/images/news/pagre_right.gif\" align=\"absmiddle\" /></li></ul>", n_ID, currentpage + 1);

                    sb.Append("</div>");

                    return sb.ToString();
                }
            }

            return content;
        }
        #endregion

        #region 获取汉字的首字母public static public string GetSpell(string cnChar)
        public static string GetSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) }).ToLower();
                    }
                }
                return "*";
            }
            else return cnChar;
        }
        #endregion

        #region 获取汉字的拼音
        /// <summary>
        /// 获取汉字的拼音
        /// </summary>
        /// <param name="x">汉字字符串</param>
        /// <param name="flag">全部拼音还是首字母：0：首字母，1：全部拼音</param>
        /// <returns></returns>
        public static string GetChineseSpell(string x, int flag)
        {
            string s = "";
            if (flag == 0)
            {
                int len = x.Length;
                for (int i = 0; i < len; i++)
                {
                    s += GetSpell(x.Substring(i, 1));
                }
            }
            if (flag == 1)
            {
                #region 数组
                int[] iA = new int[]
                              {
                               -20319 ,-20317 ,-20304 ,-20295 ,-20292 ,-20283 ,-20265 ,-20257 ,-20242 ,-20230
                               ,-20051 ,-20036 ,-20032 ,-20026 ,-20002 ,-19990 ,-19986 ,-19982 ,-19976 ,-19805
                               ,-19784 ,-19775 ,-19774 ,-19763 ,-19756 ,-19751 ,-19746 ,-19741 ,-19739 ,-19728
                               ,-19725 ,-19715 ,-19540 ,-19531 ,-19525 ,-19515 ,-19500 ,-19484 ,-19479 ,-19467
                               ,-19289 ,-19288 ,-19281 ,-19275 ,-19270 ,-19263 ,-19261 ,-19249 ,-19243 ,-19242
                               ,-19238 ,-19235 ,-19227 ,-19224 ,-19218 ,-19212 ,-19038 ,-19023 ,-19018 ,-19006
                               ,-19003 ,-18996 ,-18977 ,-18961 ,-18952 ,-18783 ,-18774 ,-18773 ,-18763 ,-18756
                               ,-18741 ,-18735 ,-18731 ,-18722 ,-18710 ,-18697 ,-18696 ,-18526 ,-18518 ,-18501
                               ,-18490 ,-18478 ,-18463 ,-18448 ,-18447 ,-18446 ,-18239 ,-18237 ,-18231 ,-18220
                               ,-18211 ,-18201 ,-18184 ,-18183 ,-18181 ,-18012 ,-17997 ,-17988 ,-17970 ,-17964
                               ,-17961 ,-17950 ,-17947 ,-17931 ,-17928 ,-17922 ,-17759 ,-17752 ,-17733 ,-17730
                               ,-17721 ,-17703 ,-17701 ,-17697 ,-17692 ,-17683 ,-17676 ,-17496 ,-17487 ,-17482
                               ,-17468 ,-17454 ,-17433 ,-17427 ,-17417 ,-17202 ,-17185 ,-16983 ,-16970 ,-16942
                               ,-16915 ,-16733 ,-16708 ,-16706 ,-16689 ,-16664 ,-16657 ,-16647 ,-16474 ,-16470
                               ,-16465 ,-16459 ,-16452 ,-16448 ,-16433 ,-16429 ,-16427 ,-16423 ,-16419 ,-16412
                               ,-16407 ,-16403 ,-16401 ,-16393 ,-16220 ,-16216 ,-16212 ,-16205 ,-16202 ,-16187
                               ,-16180 ,-16171 ,-16169 ,-16158 ,-16155 ,-15959 ,-15958 ,-15944 ,-15933 ,-15920
                               ,-15915 ,-15903 ,-15889 ,-15878 ,-15707 ,-15701 ,-15681 ,-15667 ,-15661 ,-15659
                               ,-15652 ,-15640 ,-15631 ,-15625 ,-15454 ,-15448 ,-15436 ,-15435 ,-15419 ,-15416
                               ,-15408 ,-15394 ,-15385 ,-15377 ,-15375 ,-15369 ,-15363 ,-15362 ,-15183 ,-15180
                               ,-15165 ,-15158 ,-15153 ,-15150 ,-15149 ,-15144 ,-15143 ,-15141 ,-15140 ,-15139
                               ,-15128 ,-15121 ,-15119 ,-15117 ,-15110 ,-15109 ,-14941 ,-14937 ,-14933 ,-14930
                               ,-14929 ,-14928 ,-14926 ,-14922 ,-14921 ,-14914 ,-14908 ,-14902 ,-14894 ,-14889
                               ,-14882 ,-14873 ,-14871 ,-14857 ,-14678 ,-14674 ,-14670 ,-14668 ,-14663 ,-14654
                               ,-14645 ,-14630 ,-14594 ,-14429 ,-14407 ,-14399 ,-14384 ,-14379 ,-14368 ,-14355
                               ,-14353 ,-14345 ,-14170 ,-14159 ,-14151 ,-14149 ,-14145 ,-14140 ,-14137 ,-14135
                               ,-14125 ,-14123 ,-14122 ,-14112 ,-14109 ,-14099 ,-14097 ,-14094 ,-14092 ,-14090
                               ,-14087 ,-14083 ,-13917 ,-13914 ,-13910 ,-13907 ,-13906 ,-13905 ,-13896 ,-13894
                               ,-13878 ,-13870 ,-13859 ,-13847 ,-13831 ,-13658 ,-13611 ,-13601 ,-13406 ,-13404
                               ,-13400 ,-13398 ,-13395 ,-13391 ,-13387 ,-13383 ,-13367 ,-13359 ,-13356 ,-13343
                               ,-13340 ,-13329 ,-13326 ,-13318 ,-13147 ,-13138 ,-13120 ,-13107 ,-13096 ,-13095
                               ,-13091 ,-13076 ,-13068 ,-13063 ,-13060 ,-12888 ,-12875 ,-12871 ,-12860 ,-12858
                               ,-12852 ,-12849 ,-12838 ,-12831 ,-12829 ,-12812 ,-12802 ,-12607 ,-12597 ,-12594
                               ,-12585 ,-12556 ,-12359 ,-12346 ,-12320 ,-12300 ,-12120 ,-12099 ,-12089 ,-12074
                               ,-12067 ,-12058 ,-12039 ,-11867 ,-11861 ,-11847 ,-11831 ,-11798 ,-11781 ,-11604
                               ,-11589 ,-11536 ,-11358 ,-11340 ,-11339 ,-11324 ,-11303 ,-11097 ,-11077 ,-11067
                               ,-11055 ,-11052 ,-11045 ,-11041 ,-11038 ,-11024 ,-11020 ,-11019 ,-11018 ,-11014
                               ,-10838 ,-10832 ,-10815 ,-10800 ,-10790 ,-10780 ,-10764 ,-10587 ,-10544 ,-10533
                               ,-10519 ,-10331 ,-10329 ,-10328 ,-10322 ,-10315 ,-10309 ,-10307 ,-10296 ,-10281
                               ,-10274 ,-10270 ,-10262 ,-10260 ,-10256 ,-10254
                              };
                string[] sA = new string[]
                                      {
                                       "a","ai","an","ang","ao"

                                       ,"ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao","bie","bin"
                                       ,"bing","bo","bu"

                                       ,"ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che"
                                       ,"chen","cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun"
                                       ,"chuo","ci","cong","cou","cu","cuan","cui","cun","cuo"

                                       ,"da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu"
                                       ,"dong","dou","du","duan","dui","dun","duo"

                                       ,"e","en","er"

                                       ,"fa","fan","fang","fei","fen","feng","fo","fou","fu"

                                       ,"ga","gai","gan","gang","gao","ge","gei","gen","geng","gong","gou","gu","gua","guai"
                                       ,"guan","guang","gui","gun","guo"

                                       ,"ha","hai","han","hang","hao","he","hei","hen","heng","hong","hou","hu","hua","huai"
                                       ,"huan","huang","hui","hun","huo"

                                       ,"ji","jia","jian","jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue"
                                       ,"jun"

                                       ,"ka","kai","kan","kang","kao","ke","ken","keng","kong","kou","ku","kua","kuai","kuan"
                                       ,"kuang","kui","kun","kuo"

                                       ,"la","lai","lan","lang","lao","le","lei","leng","li","lia","lian","liang","liao","lie"
                                       ,"lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo"

                                       ,"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min"
                                       ,"ming","miu","mo","mou","mu"

                                       ,"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie"
                                       ,"nin","ning","niu","nong","nu","nv","nuan","nue","nuo"

                                       ,"o","ou"

                                       ,"pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie","pin","ping"
                                       ,"po","pu"

                                       ,"qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que"
                                       ,"qun"

                                       ,"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo"

                                       ,"sa","sai","san","sang","sao","se","sen","seng","sha","shai","shan","shang","shao","she"
                                       ,"shen","sheng","shi","shou","shu","shua","shuai","shuan","shuang","shui","shun","shuo","si"
                                       ,"song","sou","su","suan","sui","sun","suo"

                                       ,"ta","tai","tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu"
                                       ,"tuan","tui","tun","tuo"

                                       ,"wa","wai","wan","wang","wei","wen","weng","wo","wu"

                                       ,"xi","xia","xian","xiang","xiao","xie","xin","xing","xiong","xiu","xu","xuan","xue","xun"

                                       ,"ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you","yu","yuan","yue","yun"

                                       ,"za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang","zhao"
                                       ,"zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui"
                                       ,"zhun","zhuo","zi","zong","zou","zu","zuan","zui","zun","zuo"
                                      };
                #endregion

                byte[] B = new byte[2];
                char[] c = x.ToCharArray();
                for (int j = 0; j < c.Length; j++)
                {
                    B = System.Text.Encoding.Default.GetBytes(c[j].ToString());
                    if ((int)(B[0]) <= 160 && (int)(B[0]) >= 0)
                    {
                        s += c[j];
                    }
                    else
                    {
                        for (int i = (iA.Length - 1); i >= 0; i--)
                        {
                            if (iA[i] <= (int)(B[0]) * 256 + (int)(B[1]) - 65536)
                            {
                                s += sA[i];
                                break;
                            }
                        }
                    }
                }
            }
            return s;
        }
        #endregion

        #region 读取file文件的文本内容
        public static string Read(string file)
        {
            string path = Fetch.MapPath(file);

            if (!File.Exists(path))
            {
#if DEBUG
                new Terminator().Throw("无法读取到文件 <u>" + path + "</u>，可能这个文件不存在或无法被访问。（需管理员解决）<br /><br />用于美丽伊甸园的提示：<a href='http://www.meilieasy.com/logout.aspx'>删除Cookies再重试</a>", null, null, null, false);
#else
				new Terminator().Throw("无法读取到文件 <u>" + path.Replace(HttpContext.Current.Server.MapPath("/"), "") + "</u>，可能这个文件不存在或无法被访问。（需管理员解决）<br /><br />用于美丽伊甸园的提示：<a href='http://www.meilieasy.com/logout.aspx'>删除Cookies再重试</a>", null, null, null, false);
#endif
            }

            string s = string.Empty;

            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                s = sr.ReadToEnd();
            }
            return s;
        }
        #endregion

        #region 获取指定网页的源文件
        public static string GetSource(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            return reader.ReadToEnd();
        }
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.EndsWith(","))
                return str.Substring(0, str.LastIndexOf(","));
            else
                return str;
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        public static string GetTurnCodeString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("\t", "");
                value = value.Replace("\r\n", " ");
                value = value.Replace("\n", " ");
                value = value.Replace("\"", "“");
                value = value.Replace("\\", "\\\\");

            }
            return value;
        }
        public static string CleanSpecChar(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("`", "");
                value = value.Replace("~", "");
                //value = value.Replace("!", "！");20130622 fn 导出已发订单明细时英文变中文的错误
                value = value.Replace("#", "");
                value = value.Replace("$", "");
                value = value.Replace("%", "");
                value = value.Replace("^", "");

                value = value.Replace("&", "");
                //value = value.Replace("*", "");
                //value = value.Replace("(", "！");20130622 fn 导出已发订单明细时英文变中文的错误
                value = value.Replace(")", "");
                //value = value.Replace("-", "");
                value = value.Replace("_", "");
                value = value.Replace("=", "");
                value = value.Replace("+", "");


                value = value.Replace("{", "");
                value = value.Replace("}", "");
                value = value.Replace("[", "");
                value = value.Replace("]", "");
                //value = value.Replace(":", "：");
                value = value.Replace(";", "；");
                //value = value.Replace("'", "‘");2013-05-17 fn 去掉 因为6050-这样的数据容易转成日期，所以需要加单引号，这里将英文单引改成中文单引
                value = value.Replace("\"", "");

                value = value.Replace("\\", "");
                value = value.Replace("|", "");
                value = value.Replace("<", "《");
                value = value.Replace(">", "》");
                value = value.Replace(",", "，");
                //value = value.Replace(".", "。");20130622 fn 导出已发订单明细时英文变中文的错误
                //value = value.Replace("?", "？");
                //value = value.Replace("/", "");20130624 影响日期格式

                value = value.Replace("\t", "");
                value = value.Replace("\r\n", " ");
                value = value.Replace("\n", " ");
                value = value.Replace("\"", "“");
                value = value.Replace("\\", "");
            }
            return value;
        }

        #region 获取客户端IP
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
        #region 获取网络IP
        /// <summary>
        /// 获取网络IP
        /// </summary>
        public static string NetworkIp
        {
            get
            {

                string AddressIP = string.Empty;
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        AddressIP = _IPAddress.ToString();
                    }
                }
                return AddressIP;
            }
        }

        #endregion

        #region 获取域名
        /// <summary>
        /// 获取域名
        /// </summary>
        /// <returns></returns>
        public static string GetHttpUrl()
        {
            string Url = HttpContext.Current.Request.Url.ToString();
            string PagePath = HttpContext.Current.Request.RawUrl.ToString();
            return Url.Replace(PagePath, "");
        }
        #endregion

        #region 计算时间差（DateTimeNow减去DateTime1）
        /// <summary>
        /// 计算时间差（DateTimeNow减去DateTime1）
        /// </summary>
        /// <param name="DateTimeNow"></param>
        /// <param name="DateTime1"></param>
        /// <returns>返回一个字符串</returns>
        public static string GetTimeDifference(DateTime DateTimeNow, DateTime DateTime1)
        {
            string dateDiff = null;
            TimeSpan ts = DateTimeNow - DateTime1;
            if (ts.Days >= 1)
            {
                dateDiff = DateTime1.Year.ToString() + "年" + DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours.ToString() + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes.ToString() + "分钟前";
                }
            }
            return dateDiff;
        }
        #endregion

        #region 计算时间差（现在的时间减去输入的时间）
        /// <summary>
        /// 计算时间差（现在的时间减去输入的时间）
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <returns>返回一个字符串</returns>
        public static string GetTimeDifference(DateTime DateTime1)
        {
            return GetTimeDifference(DateTime.Now, DateTime1);
        }
        #endregion

        #region base64转文件
        /// <summary>
        /// base64转文件
        /// </summary>
        /// <param name="input">base64编码</param>
        /// <param name="saveFileName">文件名(带路径)</param>
        /// <returns></returns>
        public static bool ConvertFromBase64String(string input, string saveFileName)
        {
            try
            {
                //Regex r = new Regex(@"(?<=data.*?base64,).*");
                //input = r.Match(input).Value;
                var contents = Convert.FromBase64String(input);
                var path = new Regex(@".*/").Match(saveFileName).Value;
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                }

                using (var fs = new FileStream(HttpContext.Current.Server.MapPath(saveFileName), FileMode.Create, FileAccess.Write))
                {
                    fs.Write(contents, 0, contents.Length);
                    fs.Flush();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// 枚举转换select
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static string EnumTransferToSelect<TEnum>()
        {
            string result = string.Empty;
            Type enumtype = typeof(TEnum);
            foreach (var item in Enum.GetNames(typeof(TEnum)))
            {
                string desc = string.Empty;
                object[] objs = enumtype.GetField(item).GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    desc = item;
                }
                else
                {
                    DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                    desc = attr.Description;
                }
                result += "<option value='" + item + "'>" + desc + "</option>";
            }
            return result;
        }

        public static Dictionary<string, string> EnumTransferToDictionary<TEnum>()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string result = string.Empty;
            Type enumtype = typeof(TEnum);
            foreach (var item in Enum.GetNames(typeof(TEnum)))
            {
                string desc = string.Empty;
                object[] objs = enumtype.GetField(item).GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    desc = item;
                }
                else
                {
                    DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                    desc = attr.Description;
                }
                dic.Add(item, desc);
                //result += "<option value='" + item + "'>" + desc + "</option>";
            }
            return dic;
        }

        #region 获取枚举注释
        /// <summary>
        /// 获取枚举注释
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(object value)
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumItem requires a Enum ");
            }
            var name = Enum.GetName(enumType, Convert.ToInt32(value));
            if (name == null)
                return string.Empty;
            object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                return attr.Description;
            }
        }
        #endregion

        public static List<String> GetString(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (list.IndexOf(values[i].ToLower()) == -1)//对每个成员做一次新数组查询如果没有相等的则加到新数组
                    list.Add(values[i]);

            }

            return list;


        }
    }
}
