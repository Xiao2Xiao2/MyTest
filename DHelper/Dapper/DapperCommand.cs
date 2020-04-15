using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DHelper.Core;
using DHelper.DataBase;
using DHelper.Interface;

namespace DHelper.Dapper
{
    /// <summary>
    /// Dapper操作类
    /// </summary>
    public static class DapperCommand
    {
        private static IDapperCommand i
        {
            get;
            set;
        }

        static DapperCommand()
        {
            DapperCommand.i = new MSSql();
        }
        /// <summary>
        /// 根据参数新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <returns></returns>
        public static long Insert<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(eneity);
        }
        /// <summary>
        /// 根据参数新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <returns></returns>
        public static long Insert<T>(this object eneity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(eneity);
        }
        /// <summary>
        /// 根据参数List新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity">参数List</param>
        /// <returns></returns>
        public static long Insert<T>(this List<T> listEntity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(listEntity);
        }
        /// <summary>
        /// 根据参数List新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity">参数List</param>
        /// <returns></returns>
        public static long Insert<T>(this List<object> listEntity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(listEntity);
        }
        /// <summary>
        /// 根据参数修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <returns></returns>
        public static bool Update<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Update<T>(eneity);
        }
        /// <summary>
        /// 根据参数修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <param name="condition">条件</param>
        /// <param name="updateFiles">字段</param>
        /// <returns></returns>
        public static bool Update<T>(this object eneity, string condition, List<ColumInfo> updateFiles) where T : class, new()
        {
            return DapperCommand.i.Update<T>(eneity, condition, updateFiles);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <returns></returns>
        public static bool Delete<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Delete<T>(eneity);
        }
        /// <summary>
        /// 根据参数删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static bool Delete<T>(this object eneity, string condition) where T : class, new()
        {
            return DapperCommand.i.Delete<T>(eneity, condition);
        }
        /// <summary>
        /// 查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public static IList<T> Select<T>(this object entity, string condition, string orderby="") where T : class, new()
        {
            return DapperCommand.i.Select<T>(condition, orderby, entity, 0);
        }
        /// <summary>
        /// 查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public static T SelectSingle<T>(this object entity, string condition, string orderby="") where T : class, new()
        {
            return DapperCommand.i.SelectSingle<T>(condition, orderby, entity);
        }
        /// <summary>
        /// 分页查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <param name="condition">条件</param>
        /// <param name="startIndex">开始Index</param>
        /// <param name="endIndex">结束Index</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public static IList<T> SelectByPage<T>(this object entity, string condition, int startIndex, int endIndex, string orderby="") where T : class, new()
        {
            return DapperCommand.i.SelectByPage<T>(condition, orderby, entity, startIndex, endIndex);
        }
        /// <summary>
        /// 根据SQL语句查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        public static List<T> Quering<T>(string strSql, object obj = null)
        {
            return DapperCommand.i.GetEntityListBySql<T>(strSql, obj);
        }
        /// <summary>
        /// 根据SQL语句查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        public static T QueringSingle<T>(string strSql, object obj = null)
        {
            return DapperCommand.i.GetEntityBySql<T>(strSql, obj);
        }
        /// <summary>
        /// 调用SQL语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        public static long ExcuteCount(string strSql, object obj = null)
        {
            return DapperCommand.i.ExcuteBySql(strSql, obj);
        }
    }
}
