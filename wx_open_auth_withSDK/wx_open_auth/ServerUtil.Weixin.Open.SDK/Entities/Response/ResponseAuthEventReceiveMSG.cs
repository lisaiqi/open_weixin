using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    /// <summary>
    /// 授权事件接收URL接收到的事件信息
    /// </summary>
    public class ResponseAuthEventReceiveMSG : WxJsonResult
    {
        /// <summary>
        /// 第三方平台appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public int CreateTime { get; set; }

        /// <summary>
        /// none，代表该消息推送给服务
        /// component_verify_ticket 推送component_verify_ticket协议
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// Ticket内容
        /// </summary>
        public string ComponentVerifyTicket { get; set; }

        /// <summary>
        /// 取消授权的公众号
        /// </summary>
        public string AuthorizerAppid { get; set; }

        
    }
}
