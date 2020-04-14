using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHelper.Core
{
    public class ColumInfo
    {
        public bool Identity
        {
            get;
            set;
        }

        public string ColName
        {
            get;
            set;
        }

        public object ColValue
        {
            get;
            set;
        }

        public bool ColCanWrite
        {
            get;
            set;
        }

        public bool ColIsKey
        {
            get;
            set;
        }

        public bool ColIsIgnore
        {
            get;
            set;
        }

        public Type ColType
        {
            get;
            set;
        }
    }
}
