using System;
using System.Collections.Generic;

namespace Common
{
    [System.AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RightCodeAttribute : Attribute
    {
        public RightCodeAttribute(params string[] codes)
        {
            if (_rightcode == null) _rightcode = new List<string>();
            foreach (string item in codes)
                _rightcode.Add(item);
        }
        private IList<string> _rightcode;

        public IList<string> RightCode
        {
            get { return _rightcode; }
        }
    }
}
