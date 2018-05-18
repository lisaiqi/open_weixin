using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ServerUtil.ServiceBase;
using ServerUtil.LogHelper;
using System.Net;
using System.Xml.Linq;

namespace wx_open_auth_host
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReceiveRequestData postModel = new ReceiveRequestData();
            postModel.signature = Request["signature"];
            postModel.timestamp = Request["timestamp"];
            postModel.nonce = Request["nonce"];
            postModel.encrypt_type = Request["encrypt_type"];
            postModel.msg_signature = Request["msg_signature"];


            //using (var streamReader = new StreamReader(Request.InputStream))
            //{
            //    string stringInput1 = streamReader.ReadToEnd();
            //    Log4NetHelper.logText("stream", stringInput1);
            //}
            ResponseAuthEventReceiveMsg response = Component_verify_ticket(postModel, Request.InputStream);
            if (response != null)
            {
                Log4NetHelper.logText("ComponentVerifyTicket", response.ComponentVerifyTicket);
            }




        }

        public class ReceiveRequestData
        {
            /// <summary>  
            /// 微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。  
            /// </summary>  
            public string signature { get; set; }


            /// <summary>  
            /// 时间戳  
            /// </summary>  
            public string timestamp { get; set; }


            /// <summary>  
            /// 随机数  
            /// </summary>  
            public string nonce { get; set; }


            /// <summary>  
            /// 加密类型
            /// </summary>  
            public string encrypt_type { get; set; }

            /// <summary>  
            /// 消息体签名
            /// </summary>  
            public string msg_signature { get; set; }
        }

        public class ResponseAuthEventReceiveMsg
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

        /// <summary>  
        /// 接收component_verify_ticket协议  
        /// </summary>  
        /// <param name="postModel"></param>  
        /// <param name="inputStream"></param>  
        /// <returns></returns>  
        public ResponseAuthEventReceiveMsg Component_verify_ticket(ReceiveRequestData postModel, Stream inputStream)
        {
            if (inputStream != null)
            {
                inputStream.Seek(0, SeekOrigin.Begin);//强制调整指针位置  
                using (XmlReader xr = XmlReader.Create(inputStream))
                {
                    var postDataDocument = XDocument.Load(xr);
                    var result = Init(postDataDocument, postModel);
                    var resultMessage = new ResponseAuthEventReceiveMsg();
                    //xml to model  
                    EntityHelper.FillEntityWithXml(resultMessage, result);
                    return resultMessage;
                }
            }
            //else
            //{
            //Log4NetHelper.logText("timestamp", "时间戳未获取到");
            return null;
            //}
        }

        private XDocument Init(XDocument postDataDocument, ReceiveRequestData postModel)
        {
            //进行加密判断并处理  

            var postDataStr = postDataDocument.ToString();
            XDocument decryptDoc = postDataDocument;

            if (postModel != null && postDataDocument.Root.Element("Encrypt") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("Encrypt").Value))
            {
                //使用了加密  
                string sToken = wxconfig.ServerToken;//消息校验Token
                string sAppId = wxconfig.ServerAppID;//申请到的第三方平台APPID
                string sEncodingAesKey = wxconfig.ServerEncodingAESKey;//消息加解密Key
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAesKey, sAppId);
                string msgXml = null;
                var result = wxcpt.DecryptMsg(postModel.msg_signature, postModel.timestamp, postModel.nonce, postDataStr, ref msgXml);
                //判断result类型  
                if (result != 0)
                {
                    Log4NetHelper.logText("Init", "未解密成功");
                    //验证没有通过，取消执行  
                    return null;
                }

                decryptDoc = XDocument.Parse(msgXml);//完成解密  
            }
            return decryptDoc;
        }
    }
}