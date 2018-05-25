using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    /// <summary>
    /// JSON返回结果（用于菜单接口等）
    /// </summary>
    public class WxJsonResult
    {
        public ReturnCode errcode { get; set; }
        public string errmsg { get; set; }
         
    }
}
