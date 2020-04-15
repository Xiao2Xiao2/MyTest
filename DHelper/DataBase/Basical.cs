using Dapper;
using DHelper.Common;
using DHelper.Core;
using DHelper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common;
namespace DHelper.DataBase
{
    /// <summary>
    /// 实际Dapper操作
    /// </summary>
    public abstract class Basical : IDapperCommand
	{
        /// <summary>
        /// 连接字符串
        /// </summary>
		private string _connectionstring
		{
			get;
			set;
		}
        /// <summary>
        /// 数据库类型
        /// </summary>
		private DataBaseType _dbtype
		{
			get;
			set;
		}
        /// <summary>
        /// 日志启用  config配置开关
        /// </summary>
        private bool SqlLog
        {
            get;
            set;
        }

		public Basical(DataBaseType databasetype, string AppsettingName = "")
		{
			this._dbtype = databasetype;
			if (string.IsNullOrEmpty(AppsettingName))
			{
				this._connectionstring = PubConstant.ConnectionString;
			}
			else
			{
				this._connectionstring = PubConstant.GetConnectionString(AppsettingName);
			}
            this.SqlLog = ApplicationSettings.Get("SqlLog") == "true";
		}

		private IDbConnection GetDbSession()
		{
			DataBaseType dbtype = this._dbtype;
			if (dbtype != DataBaseType.MSSQL)
			{
			}
			SqlConnection sqlConnection = new SqlConnection(this._connectionstring);
			IDbConnection dbConnection = sqlConnection;
			dbConnection.Open();
			return dbConnection;
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
		public virtual string InsertSqlCreate<T>() where T : class, new()
		{
			return "";
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="needKey"></param>
        /// <returns></returns>
		public virtual string InsertSqlCreate<T>(out bool needKey) where T : class, new()
		{
			needKey = false;
			return "";
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="updateFiles"></param>
        /// <returns></returns>
		public virtual string UpdateSqlCreate<T>(string condition = "", List<ColumInfo> updateFiles = null) where T : class, new()
		{
			return "";
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
		public virtual string DeleteSqlCreate<T>(string condition = "") where T : class, new()
		{
			return "";
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <param name="size"></param>
        /// <returns></returns>
		public virtual string SelectSqlCreate<T>(string condition = "", string orderby = "", int size = 0) where T : class, new()
		{
			return "";
		}
        /// <summary>
        /// 虚方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
		public virtual string SelectByPageSqlCreate<T>(string condition = "", string orderby = "", int startIndex = 0, int endIndex = 0) where T : class, new()
		{
			return "";
		}
        /// <summary>
        /// 根据参数新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <returns></returns>
        public long Insert<T>(object entity) where T : class, new()
		{
			long result = 0L;
			try
			{
				bool flag = false;
				string sql = this.InsertSqlCreate<T>(out flag);
                if (flag)
                {
                    result = CommandQuery<long?>(sql,entity).FirstOrDefault<long?>() ?? 0L;
                }
                else
                {
                    result = CommandExecute(sql, entity);
                }
			}
			catch (Exception var_6_D5)
			{
			}
			return result;
		}
        /// <summary>
        /// 根据参数新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eneity">参数</param>
        /// <returns></returns>
        public long Insert<T>(T entity) where T : class, new()
		{
			return this.Insert<T>(entity);
		}
        /// <summary>
        /// 根据参数List新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity">参数List</param>
        /// <returns></returns>
		public long Insert<T>(List<object> listEntity) where T : class, new()
		{
			long result = 0L;
			try
			{
				bool flag = false;
				string sql = this.InsertSqlCreate<T>(out flag);
                result = CommandExecute(sql, listEntity);
			}
			catch (Exception var_6_8D)
			{
			}
			return result;
		}
        /// <summary>
        /// 根据参数List新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity">参数List</param>
        /// <returns></returns>
		public long Insert<T>(List<T> listEntity) where T : class, new()
		{
			return this.Insert<object>(listEntity as List<object>);
		}
        /// <summary>
        /// 根据参数修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <returns></returns>
        public bool Update<T>(T entity) where T : class, new()
		{
			return this.Update<T>(entity, "", null);
		}
        /// <summary>
        /// 根据参数修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <param name="condition">条件</param>
        /// <param name="updateCols">字段</param>
        /// <returns></returns>
        public bool Update<T>(object entity, string condition, List<ColumInfo> updateCols) where T : class, new()
		{
			long num = 0L;
			try
			{
				string sql = this.UpdateSqlCreate<T>(condition, updateCols);
                num = CommandExecute(sql, entity);
			}
			catch (Exception var_5_85)
			{
			}
			return num > 0L;
		}
        /// <summary>
        /// 查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="_entity">参数</param>
        /// <param name="size">size>0 就取前size条</param>
        /// <returns></returns>
        public List<T> Select<T>(string condition, string orderby, object _entity, int size = 0) where T : class, new()
		{
			List<T> list = new List<T>();
			try
			{
				string sql = this.SelectSqlCreate<T>(condition, orderby, size);
                list = CommandQuery<T>(sql, _entity).ToList<T>();
			}
			catch (Exception var_5_8F)
			{
			}
			return list ?? new List<T>();
		}
        /// <summary>
        /// 查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
        public T SelectSingle<T>(string condition, string orderby, object _entity) where T : class, new()
		{
			List<T> list = this.Select<T>(condition, orderby, _entity, 1);
			T result;
			if (list == null || list.Count == 0)
			{
				result = default(T);
			}
			else
			{
				result = list[0];
			}
			return result;
		}
        /// <summary>
        /// 分页查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="_entity">参数</param>
        /// <param name="startIndex">开始Index</param>
        /// <param name="endIndex">结束Index</param>
        /// <returns></returns>
        public List<T> SelectByPage<T>(string condition, string orderby, object _entity, int startIndex, int endIndex) where T : class, new()
		{
			List<T> list = new List<T>();
			try
			{
				string sql = this.SelectByPageSqlCreate<T>(condition, orderby, startIndex, endIndex);
                list = CommandQuery<T>(sql, _entity).ToList<T>() ?? new List<T>();
			}
			catch (Exception var_5_91)
			{
			}
			return list ?? new List<T>();
		}
        /// <summary>
        /// 根据SQL语句查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
        public T GetEntityBySql<T>(string strSql, object _entity = null)
		{
			T result = default(T);
			try
			{
                result = CommandQuery<T>(strSql, _entity).FirstOrDefault<T>();
			}
			catch (Exception var_4_B7)
			{
			}
			return result;
		}
        /// <summary>
        /// 根据SQL语句查询List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
		public List<T> GetEntityListBySql<T>(string strSql, object _entity = null)
		{
			List<T> result = new List<T>();
			try
			{
                result = CommandQuery<T>(strSql, _entity).ToList<T>() ?? new List<T>();
			}
			catch (Exception var_4_C7)
			{
			}
			return result;
		}
        /// <summary>
        /// 调用SQL语句
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
        public long ExcuteBySql(string strSql, object _entity = null)
		{
			long result = 0L;
			try
			{
                result = CommandExecute(strSql, _entity);
			}
			catch (Exception var_4_A8)
			{
			}
			return result;
		}
        /// <summary>
        /// 根据参数删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <returns></returns>
		public bool Delete<T>(T entity) where T : class, new()
		{
			return this.Delete<T>(entity, "");
		}
        /// <summary>
        /// 根据参数删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">参数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
		public bool Delete<T>(object entity, string condition) where T : class, new()
		{
			long num = 0L;
			try
			{
				string sql = this.DeleteSqlCreate<T>(condition);
                num = CommandExecute(sql, entity);
			}
			catch (Exception var_5_84)
			{
			}
			return num > 0L;
		}
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
        private IEnumerable<T> CommandQuery<T>(string strSql,object _entity)
        {
            IEnumerable<T> result = null;
            string msg = "succeed";
            using (IDbConnection dbSession = this.GetDbSession())
            {
                using (IDbTransaction dbTransaction = dbSession.BeginTransaction())
                {
                    try
                    {
                        if (_entity == null)
                        {
                            result = dbSession.Query<T>(strSql, null, dbTransaction, true, null, null);
                        }
                        else
                        {
                            result = dbSession.Query<T>(strSql, _entity, dbTransaction, true, null, null);
                        }
                    }
                    catch (DataException ex)
                    {
                        dbTransaction.Rollback();
                        msg = ex.Message;
                        //throw ex;
                    }
                    dbTransaction.Commit();
                }
            }
            LogWrite(strSql, msg, _entity);
            return result;
        }
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="_entity">参数</param>
        /// <returns></returns>
        private long CommandExecute(string strSql, object _entity)
        {
            long result = 0L;
            string msg = "succeed";
            using (IDbConnection dbSession = this.GetDbSession())
            {
                using (IDbTransaction dbTransaction = dbSession.BeginTransaction())
                {
                    try
                    {
                        if (_entity == null)
                        {
                            result = (long)dbSession.Execute(strSql, null, dbTransaction, null, null);
                        }
                        else
                        {
                            result = (long)dbSession.Execute(strSql, _entity, dbTransaction, null, null);
                        }
                    }
                    catch (DataException ex)
                    {
                        dbTransaction.Rollback();
                        msg = ex.Message;
                        //throw ex;
                    }
                    dbTransaction.Commit();
                }
            }
            LogWrite(strSql, msg, _entity);
            return result;
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="msg"></param>
        /// <param name="_entity"></param>
        private void LogWrite(string sql,string msg,object _entity)
        {
            //SQLLog is true
            if (this.SqlLog)
            {
                string text = " sql： {0} \r\n msg：{1} \r\n entity： {2}";
                try
                {
                    SystemExtends.LogWrite(text.format(sql, msg, _entity.ToJson()),"SqlLog");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
