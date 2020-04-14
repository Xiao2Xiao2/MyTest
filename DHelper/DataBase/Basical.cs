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
    public abstract class Basical : IDapperCommand
	{
		private string _connectionstring
		{
			get;
			set;
		}

		private DataBaseType _dbtype
		{
			get;
			set;
		}
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

		public virtual string InsertSqlCreate<T>() where T : class, new()
		{
			return "";
		}

		public virtual string InsertSqlCreate<T>(out bool needKey) where T : class, new()
		{
			needKey = false;
			return "";
		}

		public virtual string UpdateSqlCreate<T>(string condition = "", List<ColumInfo> updateFiles = null) where T : class, new()
		{
			return "";
		}

		public virtual string DeleteSqlCreate<T>(string condition = "") where T : class, new()
		{
			return "";
		}

		public virtual string SelectSqlCreate<T>(string condition = "", string orderby = "", int size = 0) where T : class, new()
		{
			return "";
		}

		public virtual string SelectByPageSqlCreate<T>(string condition = "", string orderby = "", int startIndex = 0, int endIndex = 0) where T : class, new()
		{
			return "";
		}

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

		public long Insert<T>(T entity) where T : class, new()
		{
			return this.Insert<T>(entity);
		}

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

		public long Insert<T>(List<T> listEntity) where T : class, new()
		{
			return this.Insert<object>(listEntity as List<object>);
		}

		public bool Update<T>(T entity) where T : class, new()
		{
			return this.Update<T>(entity, "", null);
		}

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

		public bool Delete<T>(T entity) where T : class, new()
		{
			return this.Delete<T>(entity, "");
		}

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
        private void LogWrite(string sql,string msg,object _entity)
        {
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
