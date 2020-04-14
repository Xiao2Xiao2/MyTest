/**  版本信息模板在安装目录下，可自行修改。
* Sys_Log.cs
*
* 功 能： N/A
* 类 名： Sys_Log
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/4/14 16:52:41   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using DHelper.Attributes;
using System;
namespace Model
{
	/// <summary>
	/// Sys_Log:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Log
	{
		public Sys_Log()
		{}
		#region Model
		private string _guid;
		private string _userid;
		private DateTime? _optime;
		private string _ipaddress;
		private string _opname;
		private string _memo;
		private string _operation;
		private string _tablename;
		private string _objectid;
        /// <summary>
        /// 
        /// </summary>
        [SqlField(false, true)]
        public string GUID
		{
			set{ _guid=value;}
			get{return _guid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OpTime
		{
			set{ _optime=value;}
			get{return _optime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IPAddress
		{
			set{ _ipaddress=value;}
			get{return _ipaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OpName
		{
			set{ _opname=value;}
			get{return _opname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Memo
		{
			set{ _memo=value;}
			get{return _memo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Operation
		{
			set{ _operation=value;}
			get{return _operation;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TableName
		{
			set{ _tablename=value;}
			get{return _tablename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ObjectId
		{
			set{ _objectid=value;}
			get{return _objectid;}
		}
		#endregion Model

	}
}

