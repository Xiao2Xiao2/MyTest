using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DHelper.Core;
using DHelper.DataBase;
using DHelper.Interface;

namespace DHelper.Dapper
{
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

        public static long Insert<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(eneity);
        }

        public static long Insert<T>(this object eneity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(eneity);
        }

        public static long Insert<T>(this List<T> listEntity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(listEntity);
        }

        public static long Insert<T>(this List<object> listEntity) where T : class, new()
        {
            return DapperCommand.i.Insert<T>(listEntity);
        }

        public static bool Update<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Update<T>(eneity);
        }

        public static bool Update<T>(this object eneity, string condition, List<ColumInfo> updateFiles) where T : class, new()
        {
            return DapperCommand.i.Update<T>(eneity, condition, updateFiles);
        }

        public static bool Delete<T>(this T eneity) where T : class, new()
        {
            return DapperCommand.i.Delete<T>(eneity);
        }

        public static bool Delete<T>(this object eneity, string condition) where T : class, new()
        {
            return DapperCommand.i.Delete<T>(eneity, condition);
        }

        public static IList<T> Select<T>(this object entity, string condition, string orderby="") where T : class, new()
        {
            return DapperCommand.i.Select<T>(condition, orderby, entity, 0);
        }

        public static T SelectSingle<T>(this object entity, string condition, string orderby="") where T : class, new()
        {
            return DapperCommand.i.SelectSingle<T>(condition, orderby, entity);
        }

        public static IList<T> SelectByPage<T>(this object entity, string condition, int startIndex, int endIndex, string orderby="") where T : class, new()
        {
            return DapperCommand.i.SelectByPage<T>(condition, orderby, entity, startIndex, endIndex);
        }

        public static List<T> Quering<T>(string strSql, object obj = null)
        {
            return DapperCommand.i.GetEntityListBySql<T>(strSql, obj);
        }

        public static T QueringSingle<T>(string strSql, object obj = null)
        {
            return DapperCommand.i.GetEntityBySql<T>(strSql, obj);
        }

        public static long ExcuteCount(string strSql, object obj = null)
        {
            return DapperCommand.i.ExcuteBySql(strSql, obj);
        }
    }
}
