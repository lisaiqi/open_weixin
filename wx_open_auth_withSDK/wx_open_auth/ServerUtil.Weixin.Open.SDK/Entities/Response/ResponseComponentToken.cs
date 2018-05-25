using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    public class ResponseComponentToken : WxJsonResult
    {
        public string component_access_token
        {
            get;
            set;
        }

        public string expires_in
        {
            get;
            set;
        }
    }
}
