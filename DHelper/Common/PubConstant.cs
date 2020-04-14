using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace DHelper.Common
{
    public class PubConstant
    {
        public static string ConnectionString
        {
            get
            {
                string text = ConfigurationManager.AppSettings["ConnectionString"];
                //string a = ConfigurationManager.AppSettings["ConStringEncrypt"];
                //if (a == "true")
                //{
                //    text = DESEncrypt.Decrypt(text);
                //}
                return text;
            }
        }

        public static string GetConnectionString(string configName)
        {
            string text = ConfigurationManager.AppSettings[configName];
            //string a = ConfigurationManager.AppSettings["ConStringEncrypt"];
            //if (a == "true")
            //{
            //    text = DESEncrypt.Decrypt(text);
            //}
            return text;
        }
    }
}
