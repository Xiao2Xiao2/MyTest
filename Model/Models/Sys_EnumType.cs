/**  版本信息模板在安装目录下，可自行修改。
* Sys_EnumType.cs
*
* 功 能： N/A
* 类 名： Sys_EnumType
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
	/// Sys_EnumType:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_EnumType
	{
		public Sys_EnumType()
		{}
		#region Model
		private string _guid;
		private string _tcode;
		private string _tname;
		private string _comments;
		private string _tparentid;
		private int? _sequence;
		private int? _deleted;
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
		public string TCode
		{
			set{ _tcode=value;}
			get{return _tcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TName
		{
			set{ _tname=value;}
			get{return _tname;}
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
		public string TParentID
		{
			set{ _tparentid=value;}
			get{return _tparentid;}
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
		public int? Deleted
		{
			set{ _deleted=value;}
			get{return _deleted;}
		}
		#endregion Model

	}
}

