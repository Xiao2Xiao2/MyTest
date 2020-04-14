using DHelper.Attributes;
using DHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DHelper.Core
{
    public static class ModelHepper
    {
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
            tableInfo.TableName = type.Name;
            tableInfo.Colums = new List<ColumInfo>();
            object obj2 = Cache.ReadCache(type.Name);
            if (obj2 != null)
            {
                tableInfo = (TableInfo)obj2;
            }
            else
            {
                PropertyInfo[] properties = type.GetProperties();
                tableInfo.Props = properties;
                PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    PropertyInfo propertyInfo = array[i];
                    SqlFieldAttribute[] array2 = (SqlFieldAttribute[])propertyInfo.GetCustomAttributes(typeof(SqlFieldAttribute), false);
                    ColumInfo columInfo = new ColumInfo();
                    columInfo.ColName = propertyInfo.Name;
                    columInfo.ColType = propertyInfo.PropertyType;
                    if (array2 != null && array2.Length > 0)
                    {
                        SqlFieldAttribute sqlFieldAttribute = array2[0];
                        columInfo.ColCanWrite = !sqlFieldAttribute.Identity;
                        columInfo.Identity = sqlFieldAttribute.Identity;
                        columInfo.ColIsKey = sqlFieldAttribute.Key;
                        columInfo.ColIsIgnore = sqlFieldAttribute.Ignore;
                    }
                    else
                    {
                        columInfo.ColIsKey = false;
                        columInfo.ColCanWrite = true;
                        columInfo.ColIsIgnore = false;
                        columInfo.Identity = false;
                    }
                    tableInfo.Colums.Add(columInfo);
                }
                Cache.AddCache(tableInfo.TableName, tableInfo, DateTime.Now.AddMonths(1));
            }
            return tableInfo;
        }
    }
}
