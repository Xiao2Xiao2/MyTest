using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace DHelper.Common
{
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string text = ConfigurationManager.AppSettings["ConnectionString"];

                return text;
            }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="configName">Config配置参数</param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string text = ConfigurationManager.AppSettings[configName];
            return text;
        }
    }
}
