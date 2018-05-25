using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    /// <summary>
    /// 预授权码
    /// </summary>
    public class ResponseCreatePreauthCode : WxJsonResult
    {
        public string pre_auth_code { get; set; }
        public int expires_in { get; set; }
        }
}
