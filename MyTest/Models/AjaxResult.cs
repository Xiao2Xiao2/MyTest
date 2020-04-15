using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTest.Controllers
{
    /// <summary>
    /// 请求返回结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 成功：true;失败:false
        /// </summary>
        [JsonProperty("Result")]
        public bool Result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
    }

    /// <summary>
    /// 列表返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AjaxListResult<T> : AjaxResult
    {
        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty("count")]
        public int Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonProperty("data")]
        public List<T> List
        {
            get { return _List; }
            set { _List = value; }
        }

        private List<T> _List = new List<T>();

    }
    /// <summary>
    /// 列表返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AjaxListResult1<T> : AjaxResult
    {
        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty("count")]
        public int Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonProperty("data")]
        public T List
        {
            get { return _List; }
            set { _List = value; }
        }

        private T _List;

    }
}