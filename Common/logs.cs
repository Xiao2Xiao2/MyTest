using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class logs
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="MessageContent">消息</param>
        public static void WriteLog(string FileName, string MessageContent)
        {
            try
            {
                lock (objLock)
                {
                    string logpath = AppDomain.CurrentDomain.BaseDirectory + @"APP_LOG\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
                    if (!Directory.Exists(logpath))
                    {
                        Directory.CreateDirectory(logpath);
                    }
                    StreamWriter writer = new StreamWriter(new FileStream(logpath + FileName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log", FileMode.Append, FileAccess.Write), System.Text.Encoding.GetEncoding("GBK"));
                    writer.WriteLine(MessageContent);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                //WriteLog(FileName, MessageContent);
            }
        }
        private static object objLock = new object();
    }
}
