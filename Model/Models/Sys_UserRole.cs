/**  版本信息模板在安装目录下，可自行修改。
* Sys_UserRole.cs
*
* 功 能： N/A
* 类 名： Sys_UserRole
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/4/14 16:52:44   N/A    初版
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
	/// Sys_UserRole:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_UserRole
	{
		public Sys_UserRole()
		{}
		#region Model
		private string _guid;
		private string _useraccid;
		private string _roleid;
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
		public string UserAccID
		{
			set{ _useraccid=value;}
			get{return _useraccid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
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

