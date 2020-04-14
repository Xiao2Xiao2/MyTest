using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataMapFieldAttribute : Attribute
    {
        string columnName = "";
        public DataMapFieldAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
        /// <summary>
        /// 要绑定的列名
        /// </summary>
        public string ColumnName
        {
            get { return this.columnName; }
            private set { this.columnName = value; }
        }
    }
}
