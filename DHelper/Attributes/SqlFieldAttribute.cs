using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SqlFieldAttribute : Attribute
    {
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool Ignore
        {
            get;
            set;
        }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool Key
        {
            get;
            set;
        }
        /// <summary>
        /// 是否自增长
        /// </summary>
        public bool Identity
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ignore">是否忽略</param>
        /// <param name="Key">是否主键</param>
        /// <param name="identity">是否自增长</param>
        public SqlFieldAttribute(bool Ignore = false, bool Key = false, bool identity = false)
        {
            this.Ignore = Ignore;
            this.Key = Key;
            this.Identity = identity;
        }
    }
}
