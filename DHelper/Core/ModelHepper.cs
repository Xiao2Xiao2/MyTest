using DHelper.Attributes;
using DHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DHelper.Core
{
    /// <summary>
    /// 解析Model类
    /// </summary>
    public static class ModelHepper
    {
        /// <summary>
        /// 解析Model类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Model类</param>
        /// <returns></returns>
        public static TableInfo GetDataFields<T>(T obj = null) where T : class, new()
        {
            Type type;
            if (obj == null)
            {
                type = typeof(T);
            }
            else
            {
                type = obj.GetType();
            }
            TableInfo tableInfo = new TableInfo();
            //type.Name 即为Class名称
            tableInfo.TableName = type.Name;
            //Model字段List
            tableInfo.Colums = new List<ColumInfo>();
            //判断是否存在缓存中
            object obj2 = Cache.ReadCache(type.Name);
            if (obj2 != null)
            {
                tableInfo = (TableInfo)obj2;
            }
            else
            {
                //读取Model下所有的公共字段(属性)
                PropertyInfo[] properties = type.GetProperties();
                tableInfo.Props = properties;
                PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    PropertyInfo propertyInfo = array[i];
                    //获取SqlFieldAttribute特性
                    SqlFieldAttribute[] array2 = (SqlFieldAttribute[])propertyInfo.GetCustomAttributes(typeof(SqlFieldAttribute), false);
                    ColumInfo columInfo = new ColumInfo();
                    columInfo.ColName = propertyInfo.Name;
                    columInfo.ColType = propertyInfo.PropertyType;
                    if (array2 != null && array2.Length > 0)
                    {
                        //存在SqlFieldAttribute特性就读取
                        SqlFieldAttribute sqlFieldAttribute = array2[0];
                        columInfo.ColCanWrite = !sqlFieldAttribute.Identity;
                        columInfo.Identity = sqlFieldAttribute.Identity;
                        columInfo.ColIsKey = sqlFieldAttribute.Key;
                        columInfo.ColIsIgnore = sqlFieldAttribute.Ignore;
                    }
                    else
                    {
                        //不存在则默认
                        columInfo.ColIsKey = false;
                        columInfo.ColCanWrite = true;
                        columInfo.ColIsIgnore = false;
                        columInfo.Identity = false;
                    }
                    tableInfo.Colums.Add(columInfo);
                }
                //添加到缓存
                Cache.AddCache(tableInfo.TableName, tableInfo, DateTime.Now.AddMonths(1));
            }
            return tableInfo;
        }
    }
}
