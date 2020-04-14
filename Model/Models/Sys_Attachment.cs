/**  版本信息模板在安装目录下，可自行修改。
* Sys_Attachment.cs
*
* 功 能： N/A
* 类 名： Sys_Attachment
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/4/14 16:52:39   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using DHelper.Attributes;

namespace Model
{
	/// <summary>
	/// Sys_Attachment:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Attachment
	{
		public Sys_Attachment()
		{}
		#region Model
		private string _guid;
		private string _filename;
		private string _extension;
		private string _serverpath;
		private int? _filesize;
		private string _description;
		private string _objectid;
		private int? _deleted=0;
		private string _attachmenttype;
		private string _userid;
		private string _createtime;
		/// <summary>
		/// 
		/// </summary>
        [SqlField(false,true)]
		public string GUID
		{
			set{ _guid=value;}
			get{return _guid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Extension
		{
			set{ _extension=value;}
			get{return _extension;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ServerPath
		{
			set{ _serverpath=value;}
			get{return _serverpath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FileSize
		{
			set{ _filesize=value;}
			get{return _filesize;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ObjectID
		{
			set{ _objectid=value;}
			get{return _objectid;}
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
		public string AttachmentType
		{
			set{ _attachmenttype=value;}
			get{return _attachmenttype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

