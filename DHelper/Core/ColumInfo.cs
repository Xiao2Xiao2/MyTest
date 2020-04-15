using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHelper.Core
{
    /// <summary>
    /// Model字段解析类
    /// </summary>
    public class ColumInfo
    {
        /// <summary>
        /// 是否自增长
        /// </summary>
        public bool Identity
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string ColName
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        public object ColValue
        {
            get;
            set;
        }
        /// <summary>
        /// 是否读取
        /// </summary>
        public bool ColCanWrite
        {
            get;
            set;
        }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool ColIsKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool ColIsIgnore
        {
            get;
            set;
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type ColType
        {
            get;
            set;
        }
    }
}
