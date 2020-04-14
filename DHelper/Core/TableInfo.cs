using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DHelper.Core
{
    public class TableInfo
    {
        public string TableName
        {
            get;
            set;
        }

        public List<ColumInfo> Colums
        {
            get;
            set;
        }

        public PropertyInfo[] Props
        {
            get;
            set;
        }
    }
}
