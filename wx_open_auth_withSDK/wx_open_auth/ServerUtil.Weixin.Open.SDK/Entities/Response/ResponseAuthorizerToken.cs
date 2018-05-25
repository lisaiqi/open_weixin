using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    public class ResponseAuthorizerToken : WxJsonResult
    {
        /// <summary>
        /// 授权方令牌
        /// </summary>
        public string authorizer_access_token { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string authorizer_refresh_token { get; set; }
    }
}
