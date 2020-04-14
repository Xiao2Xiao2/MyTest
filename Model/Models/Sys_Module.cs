/**  版本信息模板在安装目录下，可自行修改。
* Sys_Module.cs
*
* 功 能： N/A
* 类 名： Sys_Module
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
	/// Sys_Module:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Module
	{
		public Sys_Module()
		{}
		#region Model
		private string _guid;
		private string _mocode;
		private string _moname;
		private string _parentid;
		private int? _deleted=0;
		private string _comments;
		private int? _modulevisible=0;
		private string _menuurl;
		private int? _sequence;
		private string _iconcode;
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
		public string MoCode
		{
			set{ _mocode=value;}
			get{return _mocode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MoName
		{
			set{ _moname=value;}
			get{return _moname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
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
		/// <summary>
		/// 
		/// </summary>
		public int? ModuleVisible
		{
			set{ _modulevisible=value;}
			get{return _modulevisible;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MenuUrl
		{
			set{ _menuurl=value;}
			get{return _menuurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sequence
		{
			set{ _sequence=value;}
			get{return _sequence;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IconCode
		{
			set{ _iconcode=value;}
			get{return _iconcode;}
		}
		#endregion Model

	}
}

