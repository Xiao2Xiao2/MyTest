using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class DataMap
    {
        ///// <summary>
        ///// 创建实例类数组
        ///// </summary>
        ///// <typeparam name="T">实体类类型</typeparam>
        ///// <param name="dt">包含实体类数据的表</param>
        ///// <returns></returns>
        //public static T[] From<T>( DataTable dt) where T : class
        //{
        //    return ToList<T>(dt).ToArray();
        //}

        public static List<T> ToList<T>(this DataSet ds) where T : class,new()
        {
            return ds.Tables[0].ToList<T>() ?? new List<T>();
        }
        public static T ToModel<T>(this DataSet ds) where T : class,new()
        {
            return ds.Tables[0].ToModel<T>() ?? new T();
        }
        /// <summary>
        /// 穿件实例类List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class,new()
        {
            List<T> li = new List<T>();
            if (dt == null) return li;
            foreach (DataRow dr in dt.Rows)
            {
                li.Add(From<T>(dr));
            }
            return li;
        }

        public static T ToModel<T>(this DataTable dt) where T : class,new()
        {
            T t = new T();
            if (dt == null)
            {
                t = null;
            }
            else if (dt.Rows.Count > 0)
            {
                t = From<T>(dt.Rows[0]);
            }
            else
            {
                t = null;
            }

            return t;
        }

        /// <summary>
        /// 创建实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="dr">包含实例类数据的行</param>
        /// <returns></returns>
        public static T From<T>(DataRow dr) where T : class
        {
            if (dr == null) throw new ArgumentException("dr");
            T obj = Activator.CreateInstance<T>();
            if (dr != null)
            {
                PropertyInfo[] props = obj.GetType().GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (!p.CanWrite) continue;
                    DataMapFieldAttribute mapAttr = (DataMapFieldAttribute)Attribute.GetCustomAttribute(p, typeof(DataMapFieldAttribute));
                    string mapName = "";//要映射的字段名
                    if (mapAttr != null)
                    {
                        mapName = mapAttr.ColumnName;
                    }
                    else
                    {
                        mapName = p.Name;
                    }
                    if (dr.Table.Columns.Contains(mapName))
                    {
                        if (dr[mapName] != DBNull.Value)
                        {
                            if (dr.Table.Columns[mapName].DataType == p.PropertyType || dr.Table.Columns[mapName].DataType.BaseType == p.PropertyType.BaseType)//可空数据类型的时候不完全相等
                            {
                                var val = dr[mapName];
                                //switch (val.GetType().ToString())
                                var typeName = p.PropertyType.GenericTypeArguments.Length > 0 ? p.PropertyType.GenericTypeArguments[0].FullName : val.GetType().ToString();
                                switch (typeName)
                                {
                                    case "System.Double":
                                        val = Math.Round(Convert.ToDouble(val), 4);
                                        p.SetValue(obj, Convert.ToDouble(val), null);

                                        break;
                                    case "System.Decimal":
                                        val = Math.Round(Convert.ToDecimal(val), 4);
                                        p.SetValue(obj, Convert.ToDecimal(val), null);
                                        break;
                                    case "System.Int64":
                                        p.SetValue(obj, Convert.ToInt64(val), null);
                                        break;
                                    default:
                                        p.SetValue(obj, val, null);
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("实体类字段【{0}】类型与数据【{1}】类型不一致！", p.Name, mapName));
                            }
                        }
                    }
                }
            }
            return obj;
        }
    }
}
