using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Controller.Common.Helpers
{
    public class AccessTokenHelper
    {
        public class ErrorInfo
        { //错误信息
            public string errcode { get; set; }
            public string errmsg { get; set; }
        }
        public class MediaInfo
        {//多媒体信息
            public string type { get; set; }
            public string media_id { get; set; }
            public string created_at { get; set; }
        }
        public class AccessTokenInfo
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
        }
        public class OAuthUserInfo
        {
            public string UserId { get; set; }
            public string DeviceId { get; set; }
        }

        public class DepartMentInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string parentid { get; set; }
            public string order { get; set; }

        }
        public class DepartMentResultInfo
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public List<DepartMentInfo> department { get; set; }
        }
        public class UpdateDepartMentResultInfo { 
            
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string id { get; set; }
     
        }
        /// <summary>
        /// 用户信息
        /// 参数说明见http://mp.weixin.qq.com/wiki/index.php?title=%E8%8E%B7%E5%8F%96%E7%94%A8%E6%88%B7%E5%9F%BA%E6%9C%AC%E4%BF%A1%E6%81%AF
        /// 正常情况下，微信会返回下述JSON数据包给公众号
        /// {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        public class UserInfo
        {
            public string subscribe { get; set; }//用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
            public string openid { get; set; }//普通用户的标识，对当前公众号唯一
            public string nickname { get; set; }//用户的昵称
            public int sex { get; set; }//用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
            public string language { get; set; }//用户的语言，简体中文为zh_CN
            public string city { get; set; }//用户所在城市
            public string province { get; set; }//用户所在省份
            public string country { get; set; }//用户所在国家
            public string headimgurl { get; set; }//用户头像
            public long subscribe_time { get; set; }//用户关注时间，为时间戳
        }
        /// <summary>
        /// 关注者
        /// </summary>
        public class Follower
        {
            public int total { get; set; }
            public int count { get; set; }
            public string[] data { get; set; }
            public string next_openid { get; set; }
        }
    }
}