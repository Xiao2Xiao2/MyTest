using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyTest.Extensions
{
    public enum LoginState
    {
        [Description("登陆成功")]
        Succeed,
        [Description("账号或密码错误")]
        NameOrPwdError,
        [Description("账号已锁定")]
        StatusError
    }
    public static class MyException
    {
        /// <summary>
        /// 获取枚举类型的描述
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static string ToDescription<T>(this T enumeration)
        {
            Type type = enumeration.GetType();
            FieldInfo[] fields = typeof(T).GetFields();
            string result = "";
            foreach (FieldInfo item in fields)
            {
                if (item.FieldType.IsEnum)

                {

                    object[] attr = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    result = attr.Length == 0 ? item.Name : ((DescriptionAttribute)attr[0]).Description;

                }
            }
           
            return result;
        }

    }
}
