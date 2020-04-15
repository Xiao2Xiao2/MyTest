using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DHelper.Core
{
    /// <summary>
    /// Model解析类
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// Model名称
        /// </summary>
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// Model字段List
        /// </summary>
        public List<ColumInfo> Colums
        {
            get;
            set;
        }
        /// <summary>
        /// Model下公共字段特性List
        /// </summary>
        public PropertyInfo[] Props
        {
            get;
            set;
        }
    }
}
