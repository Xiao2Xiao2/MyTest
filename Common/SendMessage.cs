using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Common
{
    public class SendMessage
    {
        private string _accesstoken = new WX_AccessToken().Accesstoken;
        public WX_Response sendMessage(JsonResponse model)
        {
            string text = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            HttpHelper httpHelper = new HttpHelper();
            HttpItem item = new HttpItem();
            item.URL = SystemExtends.format(text, new object[]
				{
					this._accesstoken
				});
            item.Encoding = Encoding.UTF8;
            item.PostEncoding = Encoding.UTF8;
            item.Method = "POST";
            item.Postdata = model.ToJson().Replace("\\\\","\\");
            HttpResult html = httpHelper.GetHtml(item);
            string html2 = html.Html;
            return SystemExtends.ToObject<WX_Response>(html2);
        }
    }

    public class CorpSendBase
    {
        /// <summary>  
        /// UserID列表（消息接收者，多个接收者用‘|’分隔）。特殊情况：指定为@all，则向关注该企业应用的全部成员发送  
        /// </summary>  
        public string touser { get; set; }

        /// <summary>  
        /// PartyID列表，多个接受者用‘|’分隔。当touser为@all时忽略本参数  
        /// </summary>  
        public string toparty { get; set; }

        /// <summary>  
        /// 消息类型  
        /// </summary>  
        public string msgtype { get; set; }

        public JsonText text { get; set; }

    } 
    public class WX_Response
    {
        public int errcode
        {
            get;
            set;
        }

        public string errmsg
        {
            get;
            set;
        }
    }
    public class JsonText
    {
        public string content
        {
            get;
            set;
        }
    }
    public class JsonImage
    {
        public string media_id
        {
            get;
            set;
        }
    }
    public class JsonVoice
    {
        public string media_id
        {
            get;
            set;
        }
    }
    public class JsonVideo
    {
        public string media_id
        {
            get;
            set;
        }

        public string thumb_media_id
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }
    }
    public class JsonMusic
    {
        public string hqmusicurl
        {
            get;
            set;
        }

        public string musicurl
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string thumb_media_id
        {
            get;
            set;
        }
    }
    public class WX_CustomsInfo
    {
        [JsonProperty(PropertyName = "kf_account", NullValueHandling = NullValueHandling.Ignore)]
        public string kf_account
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "nickname", NullValueHandling = NullValueHandling.Ignore)]
        public string nickname
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
        public string password
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "kf_headimgurl", NullValueHandling = NullValueHandling.Ignore)]
        public string kf_headimgurl
        {
            get;
            set;
        }
    }
    public class JsonResponse
    {
        public string touser
        {
            get;
            set;
        }

        public string msgtype
        {
            get;
            set;
        }
        public string url { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public JsonText text
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "image", NullValueHandling = NullValueHandling.Ignore)]
        public JsonImage image
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "voice", NullValueHandling = NullValueHandling.Ignore)]
        public JsonVoice voice
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "video", NullValueHandling = NullValueHandling.Ignore)]
        public JsonVideo video
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "music", NullValueHandling = NullValueHandling.Ignore)]
        public JsonMusic music
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "customservice", NullValueHandling = NullValueHandling.Ignore)]
        public WX_CustomsInfo customservice
        {
            get;
            set;
        }
        public JsonResponse()
        {
            this.customservice = null;
        }

        public JsonResponse(WX_CustomsInfo custom)
        {
            this.customservice = custom;
        }
    }
    public class WX_AccessToken
    {
        private const string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        private string _accesstoken = "";

        private object locker = new object();

        private string Appid = System.Configuration.ConfigurationSettings.AppSettings["Appid"].ToString();
        private string AppSecret = System.Configuration.ConfigurationSettings.AppSettings["AppSecret"].ToString();
        public string Accesstoken
        {
            get
            {
                string accesstoken;
                lock (this.locker)
                {
                    this._accesstoken = this.RefreshAccessToken();
                    accesstoken = this._accesstoken;
                }
                return accesstoken;
            }
        }
        private string RefreshAccessToken()
        {
            string text = "AccessToken-" +Appid ;
            string text2 = Cache.ReadCache(text).ToMyString();
            string result;
            if (text2 != "")
            {
                result = text2;
            }
            else
            {
                HttpHelper httpHelper = new HttpHelper();
                HttpResult html = httpHelper.GetHtml(new HttpItem
                {
                    URL = SystemExtends.format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", new object[]
					{
						this.Appid,
						this.AppSecret
					}),
                    Method = "GET"
                });
                string html2 = html.Html;
                accessTokenResult accessTokenResult = SystemExtends.ToObject<accessTokenResult>(html2);
                Cache.AddCache(text, accessTokenResult.access_token, DateTime.Now.AddMilliseconds((double)(accessTokenResult.expires_in - 600)));
                result = accessTokenResult.access_token;
            }
            return result;
        }
    }
    public class accessTokenResult
    {
        public string access_token
        {
            get;
            set;
        }

        public int expires_in
        {
            get;
            set;
        }
    }
}
