using DHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHelper.DataBase
{
    public class MSSql : Basical
    {
        public MSSql()
            : base(DataBaseType.MSSQL, "")
        {
        }

        public MSSql(string AppsettingName)
            : base(DataBaseType.MSSQL, AppsettingName)
        {
        }
        /// <summary>
        /// 根据参数生成新增SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override string InsertSqlCreate<T>()
        {
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            List<string> values = (from a in dataFields.Colums
                                   where !a.ColIsIgnore && a.ColCanWrite
                                   select a.ColName).ToList<string>();
            string text = " insert into {0}({1}) values(@{2}) ";
            if (dataFields.Colums.Count((ColumInfo x) => x.Identity) > 0)
            {
                text += ";select @@IDENTITY";
            }
            return string.Format(text, dataFields.TableName, string.Join(",", values), string.Join(",@", values));
        }
        /// <summary>
        /// 根据参数生成新增SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="needKey">如果为自增长</param>
        /// <returns></returns>
        public override string InsertSqlCreate<T>(out bool needKey)
        {
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            List<string> values = (from a in dataFields.Colums
                                   where !a.ColIsIgnore && a.ColCanWrite
                                   select a.ColName).ToList<string>();
            string text = " insert into {0}({1}) values(@{2}) ";
            needKey = (dataFields.Colums.Count((ColumInfo x) => x.Identity) > 0);
            if (needKey)
            {
                text += ";select @@IDENTITY";
            }
            return string.Format(text, dataFields.TableName, string.Join(",", values), string.Join(",@", values));
        }
        /// <summary>
        /// 根据参数生成修改SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="updateFiles">参数</param>
        /// <returns></returns>
        public override string UpdateSqlCreate<T>(string condition = "", List<ColumInfo> updateFiles = null)
        {
            Predicate<ColumInfo> predicate = null;
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            if ((condition ?? "").Trim() == "")
            {
                List<ColumInfo> arg_59_0 = dataFields.Colums;
                if (predicate == null)
                {
                    predicate = ((ColumInfo x) => x.ColIsKey);
                }
                ColumInfo columInfo = arg_59_0.Find(predicate);
                condition = string.Format(" {0}=@{0} ", columInfo.ColName);
            }
            if (updateFiles == null || updateFiles.Count == 0)
            {
                updateFiles = dataFields.Colums.FindAll((ColumInfo x) => !x.ColIsKey && !x.ColIsIgnore && x.ColCanWrite);
            }
            string arg = string.Join(",", (from a in updateFiles
                                           select a.ColName + " = @" + a.ColName).ToList<string>());
            string format = " update {0} set {1} where {2} ";
            return string.Format(format, dataFields.TableName, arg, condition);
        }
        /// <summary>
        /// 根据参数生成删除SQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public override string DeleteSqlCreate<T>(string condition = "")
        {
            Predicate<ColumInfo> predicate = null;
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            if ((condition ?? "").Trim() == "")
            {
                List<ColumInfo> arg_59_0 = dataFields.Colums;
                if (predicate == null)
                {
                    predicate = ((ColumInfo x) => x.ColIsKey);
                }
                ColumInfo columInfo = arg_59_0.Find(predicate);
                condition = string.Format(" {0}=@{0} ", columInfo.ColName);
            }
            //string format = " delete from {0} where {1} ";
            string format = " update {0} set Deleted = 1 where {1} ";
            return string.Format(format, dataFields.TableName, condition);
        }
        /// <summary>
        /// 根据参数生成查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="size">size>0 就取前size条</param>
        /// <returns></returns>
        public override string SelectSqlCreate<T>(string condition = "", string orderby = "", int size = 0)
        {
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            if ((condition ?? "").Trim() != "")
            {
                condition = " where " + condition;
            }
            if ((orderby ?? "").Trim() != "")
            {
                orderby = " order by " + orderby;
            }
            string format = " select * from {0} {1} {2} ";
            if (size > 0)
            {
                format = " select top " + size + " * from {0} {1} {2} ";
            }
            return string.Format(format, dataFields.TableName, condition, orderby);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="startIndex">开始Index</param>
        /// <param name="endIndex">结束Index</param>
        /// <returns></returns>
        public override string SelectByPageSqlCreate<T>(string condition = "", string orderby = "", int startIndex = 0, int endIndex = 0)
        {
            Func<string, string> func = null;
            Predicate<ColumInfo> predicate = null;
            string empty = string.Empty;
            TableInfo dataFields = ModelHepper.GetDataFields<T>(default(T));
            if ((condition ?? "").Trim() != "")
            {
                condition = " where " + condition;
            }
            if ((orderby ?? "").Trim() != "")
            {
                string[] array = orderby.Split(new char[]
				{
					','
				});
                IEnumerable<string> arg_9D_0 = array;
                if (func == null)
                {
                    func = ((string a) => "T." + a.Trim());
                }
                List<string> values = arg_9D_0.Select(func).ToList<string>();
                orderby = " order by " + string.Join(",", values);
            }
            else
            {
                List<ColumInfo> arg_DF_0 = dataFields.Colums;
                if (predicate == null)
                {
                    predicate = ((ColumInfo x) => x.ColIsKey);
                }
                ColumInfo columInfo = arg_DF_0.Find(predicate);
                orderby = string.Format(" order by T.{0} desc ", columInfo.ColName.Trim());
            }
            string text = " SELECT * FROM ( \r\n                                         SELECT ROW_NUMBER() OVER ({1}) AS Row, T.*  \r\n                                         from {0} T \r\n                                         {2} \r\n                                ) TT   ";
            if (startIndex * endIndex > 0)
            {
                text += string.Format(" WHERE TT.Row between {0} and {1} ", startIndex, endIndex);
            }
            return string.Format(text, dataFields.TableName, orderby, condition);
        }
    }
}
