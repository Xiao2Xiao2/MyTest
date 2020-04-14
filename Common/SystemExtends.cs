using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Words;
using Aspose.Words.Fields;
using System.IO;
using System.Net.NetworkInformation;

namespace System
{
    #region 系统扩展类
    public static class SystemExtends
    {
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return ip;
        }
        #region json 转 object
        public static T ToObject<T>(this string json)
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
        #endregion
        /// <summary>
        /// 获取mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string physicalAddress = "";
            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adaper in nice)
            {
                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }
            return physicalAddress;
        }

        #region 字符串 正则匹配
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="regexCode">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string regexCode)
        {
            Regex r = new Regex(regexCode);
            return r.IsMatch(source);
        }
        #endregion

        #region MD5 加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="clearCode">明文</param>
        /// <returns>密文</returns>
        public static string ToMD5(this string clearCode)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(clearCode));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }
        #endregion

        #region format
        /// <summary>
        /// format
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string format(this string txt, params object[] args)
        {
            return string.Format(txt, args);
        }
        #endregion

        #region  Object.ToString
        public static string ToMyString(this object obj)
        {
            if (obj == null) return "";
            return obj.ToString();
        }
        #endregion

        #region 根据枚举key or value 获取枚举描述
        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        private static NameValueCollection GetNVCFromEnumByValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        private static NameValueCollection GetNVCFromEnumByText(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }

        public static string GetEnumDescription(this int m, Type t)
        {
            if (t.BaseType.Name.ToLower() != "enum") return "";
            NameValueCollection nvc = GetNVCFromEnumByValue(t);
            return nvc.Get(m.ToString());
        }

        public static string GetEnumDescription(this string m, Type t)
        {
            if (t.BaseType.Name.ToLower() != "enum") return "";
            NameValueCollection nvc = GetNVCFromEnumByText(t);
            return nvc[m];
        }
        #endregion

        #region 数据组装成POST格式
        public static string GetPostData(this IDictionary dic)
        {
            List<string> list = new List<string>();
            foreach (var item in dic.Keys)
            {
                list.Add(string.Format("{0}={1}", item, dic[item]));
            }
            return string.Join("&", list);
        }
        #endregion

        #region object 转json
        public static string ToJson(this object obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, timeFormat);
        }
        #endregion

        #region 缓存过期时间
        public static DateTimeOffset CacheExpiredTime = DateTimeOffset.Now.AddHours(24);
        #endregion

        #region word书签模版 导出 word 数据  返回文件MemoryStream
        /// <summary>
        /// word书签模版 导出 word 数据  返回文件地址
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="Source">数据源</param>
        /// <param name="TempletePath">模版路径(相对)</param>
        /// <param name="dateTimeFormatter">时间格式 选填</param>
        /// <returns></returns>
        public static MemoryStream GetWordTemp<T>(this T Source, string TempletePath, string dateTimeFormatter = "yyyy-MM-dd") where T : class,new()
        {
            TempletePath = System.AppDomain.CurrentDomain.BaseDirectory + TempletePath;
            Document doc = new Document(TempletePath);

            BookmarkCollection bks = doc.Range.Bookmarks;
            List<Bookmark> listBookMarks = new List<Bookmark>();
            foreach (Bookmark bk in bks) listBookMarks.Add(bk);
            if (listBookMarks.Count == 0) throw new Exception("模版未加书签，请确认书签是否准确添加");
            var ps = Source.GetType().GetProperties();
            foreach (var p in ps)
            {
                string markName = p.Name;
                if (!listBookMarks.Exists(x => { return x.Name == p.Name; })) break;
                var typeName = p.PropertyType.GenericTypeArguments.Length > 0 ? p.PropertyType.GenericTypeArguments[0].FullName : p.PropertyType.ToString();
                switch (typeName)
                {
                    case "System.DateTime":
                        var o = p.GetValue(Source);
                        string val = "";
                        if (o != null)
                        {
                            DateTime dt;
                            bool b = DateTime.TryParse(o.ToString(), out dt);
                            if (b && dt > DateTime.MinValue) val = dt.ToString(dateTimeFormatter);
                        }
                        doc.Range.Bookmarks[markName].Text = string.Format("{0}", val);
                        break;
                    default:
                        doc.Range.Bookmarks[markName].Text = string.Format("{0}", p.GetValue(Source));
                        break;
                }
            }
            string fileLastName = TempletePath.Split('.')[TempletePath.Split('.').Length - 1];
            MemoryStream ms = new MemoryStream();
            switch (fileLastName.ToLower().Trim())
            {
                case "doc":
                    doc.Save(ms, SaveFormat.Doc);
                    break;
                case "docx":
                    doc.Save(ms, SaveFormat.Docx);
                    break;
                default:
                    doc.Save(ms, SaveFormat.Doc);
                    break;
            }
            ms.Position = 0;
            return ms;
        }
        #endregion


        #region word书签模版 导出 word 数据  返回文件MemoryStream
        /// <summary>
        /// word书签模版 导出 word 数据  返回文件地址
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="Source">数据源</param>
        /// <param name="TempletePath">模版路径(相对)</param>
        /// <param name="dateTimeFormatter">时间格式 选填</param>
        /// <returns></returns>
        public static MemoryStream GetWordTempByMergeField<T>(this T Source, string TempletePath, string dateTimeFormatter = "yyyy-MM-dd") where T : class,new()
        {
            TempletePath = System.AppDomain.CurrentDomain.BaseDirectory + TempletePath;
            Document doc = new Document(TempletePath);
            DocumentBuilder buider = new DocumentBuilder(doc);
            var ps = Source.GetType().GetProperties();
            foreach (var p in ps)
            {
                string markName = p.Name;
                var typeName = p.PropertyType.GenericTypeArguments.Length > 0 ? p.PropertyType.GenericTypeArguments[0].FullName : p.PropertyType.ToString();
                if (!buider.MoveToMergeField(markName)) continue;
                string val = string.Empty;
                switch (typeName)
                {
                    case "System.DateTime":
                        var o = p.GetValue(Source);
                        if (o != null)
                        {
                            DateTime dt;
                            bool b = DateTime.TryParse(o.ToString(), out dt);
                            if (b && dt > DateTime.MinValue) val = dt.ToString(dateTimeFormatter);
                            buider.Write(string.Format("{0}", val));
                        }
                        break;
                    case "System.Drawing.Image":
                        var img = p.GetValue(Source) as System.Drawing.Image;
                        if (img != null)
                        {
                            Aspose.Words.Drawing.Shape shape = buider.InsertImage(img);
                            //if (img.Height <= shape.Height)
                            //{                            
                            //    shape.Height = img.Height;
                            //}
                            if (img.Height > 300)//xu_lz 20160908
                            {
                                shape.Height = 300;

                            }
                            else
                            {
                                shape.Height = img.Height;
                            }
                            if (img.Width > 400)
                            {
                                shape.Width = 400;
                            }
                            else
                            {
                                shape.Width = img.Width;
                            }
                        }
                        break;
                    default:
                        val = string.Format("{0}", p.GetValue(Source));
                        buider.Write(string.Format("{0}", val));
                        break;
                }

            }
            string fileLastName = TempletePath.Split('.')[TempletePath.Split('.').Length - 1];
            MemoryStream ms = new MemoryStream();
            switch (fileLastName.ToLower().Trim())
            {
                case "doc":
                    doc.Save(ms, SaveFormat.Doc);
                    break;
                case "docx":
                    doc.Save(ms, SaveFormat.Docx);
                    break;
                default:
                    doc.Save(ms, SaveFormat.Doc);
                    break;
            }
            ms.Position = 0;
            return ms;
        }
        #endregion

        #region 把法律条款中的罗马数字转换成中文数字
        /// <summary>
        /// 把法律条款中的罗马数字转换成中文数字
        /// 如将“第1条第1款第1项”转换为“第一条第一款第（一）项”。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ParseFltkToCN(String str)
        {
            Regex reg = new Regex("第[0-9]+条|第[0-9]+款|第[0-9]+项");
            MatchCollection mc = reg.Matches(str);

            ArrayList lst = new ArrayList();
            for (int i = 0; i < mc.Count; i++)
            {
                Match m = mc[i];
                lst.Add(m.Value);

            }

            ArrayList lst2 = new ArrayList();
            for (int i = 0; i < lst.Count; i++)
            {
                String tempStr = lst[i] as String;
                Regex r = new Regex("[0-9]+");
                Match m = r.Match(tempStr);
                if (i == 2)
                {
                    tempStr = tempStr.Replace(m.Value, "（" + ParseDoubleToCN(Convert.ToDouble(m.Value), 0) + "）");
                }
                else
                {
                    tempStr = tempStr.Replace(m.Value, ParseDoubleToCN(Convert.ToDouble(m.Value), 0));
                }
                str = str.Replace(lst[i] as String, tempStr);
            }

            return str;
        }
        #endregion

        #region 去掉字符串中间的空格（包括全角空格）。
        /// <summary>
        /// 去掉字符串中间的空格（包括全角空格）。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String TrimSpace(String str)
        {
            str = str.Replace(" ", ""); // 半角空格
            str = str.Replace("　", ""); // 全角空格
            return str;
        }
        #endregion

        #region 去掉字符串中间的回车
        /// <summary>
        /// 去掉字符串中间的回车。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String TrimEnter(String str)
        {
            str = str.Replace("\r\n", ""); // 回车换行
            return str;
        }
        #endregion

        public const string UploadPath = "/UploadFile/";

        #region 日期转换为中文大写
        private static String[] numberBig = new String[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        /// <summary>
        /// 日期转换为中文大写
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String ParseDateToCNBig(DateTime date)
        {
            String result = "";
            Char[] yearChr = date.Year.ToString().ToCharArray();
            for (int i = 0; i < yearChr.Length; i++)
            {
                result += xxs[Convert.ToInt32(yearChr[i].ToString())];
            }
            result += "年" + ParseDoubleToCN(Convert.ToDouble(date.Month), 0) + "月" + ParseDoubleToCN(Convert.ToDouble(date.Day), 0) + "日";
            return result;
        }
        #endregion

        private static String[] dw = new String[] { "", "拾", "佰", "仟", "万", "拾万", "佰万", "仟万", "亿", "拾亿" };
        private static String[] zs = new String[] { "", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static String[] xs = new String[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static String[] xdw = new String[] { "", "十", "百", "千", "万", "十万", "百万", "千万", "亿", "十亿" };
        private static String[] xzs = new String[] { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        private static String[] xxs = new String[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        #region 把数字转换为中文
        /// <summary>
        /// 把数字转换为中文。
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="type">0 中文小写数字，1 中文大写数字。</param>
        /// <returns></returns>
        public static String ParseDoubleToCN(Double number, Int32 type)
        {
            String[] strs = number.ToString("0.###").Split('.');
            String zsStr = strs[0];
            String xsStr = (strs.Length > 1) ? strs[1] : "";
            String result = "";
            ArrayList lst = new ArrayList();

            Char[] zsChr = zsStr.ToCharArray();
            for (int i = 0; i < zsChr.Length; i++)
            {
                String temp = "";
                if (zsChr.Length == 2 && i == 0 && zsChr[i] == '1')
                {

                }
                else
                    temp = (type == 0) ? xzs[Convert.ToInt32(Convert.ToString(zsChr[i]))] : zs[Convert.ToInt32(Convert.ToString(zsChr[i]))];
                if (zsChr.Length > 0) // if (temp != null && temp != String.Empty)
                {
                    if (zsChr[i] != '0')
                    {
                        temp += (type == 0) ? xdw[zsChr.Length - i - 1] : dw[zsChr.Length - i - 1];
                    }
                }
                else
                {
                    temp = "";
                }
                lst.Add(temp);
            }

            result = "";
            foreach (String temp in lst)
            {
                result += temp;
            }

            if (xsStr.Length > 0)
            {
                Char[] xsChr = xsStr.ToCharArray();
                result += ".";
                for (int i = 0; i < xsStr.Length; i++)
                {
                    result += (type == 0) ? xxs[Convert.ToInt32(Convert.ToString(xsStr[i]))] : xs[Convert.ToInt32(Convert.ToString(xsStr[i]))];
                }
            }

            return result;
        }

        #endregion

        #region 中文姓名改成拼音
        private static int[] pyValue = new int[]{  
    -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,  
    -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,  
    -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,  
    -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,  
    -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,  
    -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,  
    -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,  
    -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,  
    -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,  
    -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,  
    -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,  
    -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,  
    -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,  
    -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,  
    -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,  
    -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,  
    -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,  
    -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,  
    -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,  
    -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,  
    -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,  
    -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,  
    -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,  
    -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,  
    -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,  
    -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,  
    -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,  
    -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,  
    -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,  
    -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,  
    -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,  
    -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,  
    -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254  
    };

        private static string[] pyName = new string[]  
    {  
    "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",  
    "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",  
    "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",  
    "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",  
    "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",  
    "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",  
    "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",  
    "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",  
    "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",  
    "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",  
    "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",  
    "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",  
    "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",  
    "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",  
    "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",  
    "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",  
    "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",  
    "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",  
    "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",  
    "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",  
    "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",  
    "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",  
    "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",  
    "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",  
    "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",  
    "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",  
    "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",  
    "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",  
    "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",  
    "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",  
    "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",  
    "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",  
    "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"  
    };

        /// <summary>
        /// 变拼音
        /// </summary>
        /// <param name="hzString"></param>
        /// <returns></returns>
        public static string Converts(string hzString)
        {
            // 匹配中文字符  
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符  
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    if (j == 1)
                    {
                        pyString += "_";
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].Substring(0, 1).ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (j > 1)
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].Substring(0, 1).ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                // 非中文字符  
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }


        /// <summary>
        /// 变拼音(全拼)
        /// </summary>
        /// <param name="hzString"></param>
        /// <returns></returns>
        public static string Converts2(string hzString)
        {
            // 匹配中文字符  
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符  
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    if (j == 1)
                    {
                        //pyString += "_";
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (j > 1)
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字  
                            if (chrAsc == -9254) // 修正“圳”字  
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyName[i] = pyName[i].ToLower();
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                // 非中文字符  
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }


        #endregion

        #region 财务大写金额
        public static string MoneyToUpper(string strAmount)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > 0)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }
            strLower = strAmount;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }
        #endregion

        #region 文本日志
        private static object logObj = new object();
        public static void LogWrite(string logs,string dir = "default")
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\APP_LOG\\{0}\\".format(dir);
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                lock (logObj)
                {
                    try
                    {
                        System.IO.File.AppendAllText(path + "{0}.log".format(DateTime.Now.ToString("yyyyMMdd")),
                            "\r\n<{0}>|".format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")) + obj,
                            Encoding.Default);
                    }
                    catch
                    {

                    }
                }
            }, logs);

        }
        #endregion

    }
    #endregion

    #region Mime类型
    /// <summary>
    /// Mime类型
    /// </summary>
    public static class MimeType
    {
        /// <summary>
        /// doc
        /// </summary>
        public const string doc = "application/msword";

        /// <summary>
        /// docx
        /// </summary>
        public const string docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        /// <summary>
        /// xls
        /// </summary>
        public const string xls = "application/vnd.ms-excel";

        /// <summary>
        /// xlsx
        /// </summary>
        public const string xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
    #endregion

    #region 附件类型
    public enum AttachmentType
    {
        [Description("Notice")]
        Notices,
        [Description("SendDocument")]
        SendDocuments,
        [Description("DocuKouYaWPJDSWPQD")]
        Docu_KouYaWPJDS_WPQD,
        [Description("DocuXianqiFj")]
        Docu_XianqiFj,
        [Description("DocuXianqiMemoFj")]
        Docu_XianqiMemoFj,
        [Description("DocuXianqiFzkMemoFj")]
        Docu_XianqiFzkMemoFj,
        [Description("DocuZelingFj")]
        Docu_ZelingFj,
        [Description("DocuZelingMemoFj")]
        Docu_ZelingMemoFj,
        [Description("DocuZelingFzkMemoFj")]
        Docu_ZelingFzkMemoFj,
        [Description("DocuRevoke")]
        Docu_Revoke,
        [Description("DocuRevokezj")]//xu_lz 20160920 注记和解除注记
        Docu_Revoke_zj,
        [Description("DocuRevokejz")]//xu_lz 20160920 注记和解除注记
        Docu_Revoke_yqks,
        [Description("DocuRevokeyqks")]//xu_lz 20160923 舆情
        Docu_Revoke_jz,
        [Description("DocuXianChangJCBL")]
        Docu_XianChangJCBL,
        //[Description("DocuKouYaWPJDSWPQD")]  //重复
        //Docu_KouYaWPJDS_WPQD,
        [Description("DocuKouYaWPJDSWPZP")]
        Docu_KouYaWPJDS_WPZP,
        [Description("DocuKouYaWPJDSDSRBL")]
        Docu_KouYaWPJDS_DSRBL,
        [Description("DocuJieChuKYWPJDSJDS")]
        Docu_JieChuKYWPJDS_JDS,
        [Description("DocuJieChuKYWPJDSSDHZ")]
        Docu_JieChuKYWPJDS_SDHZ,
        [Description("DocuKouYaWPJDSQTWS")]
        Docu_KouYaWPJDS_QTWS,
        [Description("DocuXunWenTZS")]
        Docu_XunWenTZS,
        [Description("DocuXunWenBL")]
        Docu_XunWenBL,
        [Description("DocuZeLingGZTZS")]
        Docu_ZeLingGZTZS,
        [Description("DocuXianChangJCBL1")]//现场检查笔录临时类型
        Docu_XianChangJCBL_1,
        [Description("DocuAnJianDCZJSPB")]
        Docu_AnJianDCZJSPB,
        [Description("DocuXingZhengCFSXGZS")]
        Docu_XingZhengCFSXGZS,
        [Description("DocuChenShuSBFHYJ")]
        Docu_ChenShuSBFHYJ,
        [Description("DocuOthers")]//todo:其他文书   
        Docu_Others,
        [Description("DocuHuoQinXZCFJDS")]
        Docu_HuoQinXZCFJDS,
        [Description("DocuSongDaHZXZCFSXGZS")]
        Docu_SongDaHZ_XZCFSXGZS,
        [Description("DocuSongDaHZXZCFJDS")]
        Docu_SongDaHZ_XZCFJDS,
        [Description("DocuJieAnBGB")]
        Docu_JieAnBGB,
        [Description("PLetterInfoFiles")]
        PLetterInfoFiles,
        [Description("PLetterAllotProcess")]//信访管理 反馈
        PLetterAllotProcess,
        [Description("DocuKouYaWPJDSKYJDS")]//扣押物品决定书_扣押决定书
        Docu_KouYaWPJDS_KYJDS,
        [Description("PopuChengban")]//舆情抄告单 督办
        Popu_Chengban,
        [Description("PopuDuban")]//舆情抄告单 督办
        Popu_Duban,
        [Description("WorkRewardFiles")]//员工行政奖励
        WorkRewardFiles,
        [Description("Docu_Zelingdccc")]//责令整改附件
        Docu_Zelingdccc,
        [Description("PopuJuban")]//提交局办 
        PopuJuban,
        [Description("DYPopuJuban")]//队员考核提交局办 
        DYPopuJuban

    }
    #endregion



}
