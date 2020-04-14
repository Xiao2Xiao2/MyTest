/**  版本信息模板在安装目录下，可自行修改。
* Sys_Right.cs
*
* 功 能： N/A
* 类 名： Sys_Right
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/4/14 16:52:42   N/A    初版
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
	/// Sys_Right:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Right
	{
		public Sys_Right()
		{}
		#region Model
		private string _guid;
		private string _ricode;
		private string _riname;
		private string _formoduleid;
		private int? _deleted;
		private string _comments;
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
		public string RiCode
		{
			set{ _ricode=value;}
			get{return _ricode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RiName
		{
			set{ _riname=value;}
			get{return _riname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ForModuleID
		{
			set{ _formoduleid=value;}
			get{return _formoduleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Deleted
		{
			set{ _deleted=value;}
			get{return _deleted;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Comments
		{
			set{ _comments=value;}
			get{return _comments;}
		}
		#endregion Model

	}
}

