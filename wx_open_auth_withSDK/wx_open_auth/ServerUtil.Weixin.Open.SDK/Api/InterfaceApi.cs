using Weixin.Open.SDK.Entities;
using Weixin.Open.SDK.Helpers;
using Weixin.Open.SDK.HttpUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Weixin.Open.SDK.Api
{
    /// <summary>
    /// 代公众号调用接口
    /// </summary>
    public class InterfaceApi
    {
        /// <summary>
        /// 接收component_verify_ticket协议
        /// </summary>
        /// <param name="postModel"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static ResponseAuthEventReceiveMSG Component_verify_ticket(PostModel postModel, Stream inputStream)
        {
            if (inputStream != null)
            {
                inputStream.Seek(0, SeekOrigin.Begin);//强制调整指针位置
                using (XmlReader xr = XmlReader.Create(inputStream))
                {
                    var postDataDocument = XDocument.Load(xr);

                    var result = Init(postDataDocument, postModel);

                    var resultMessage = new ResponseAuthEventReceiveMSG();
                    //xml to model
                    EntityHelper.FillEntityWithXml(resultMessage, result);
                    return resultMessage;
                }
            }
            return null;
            
        }

        private static XDocument Init(XDocument postDataDocument, PostModel _postModel)
        {
            //进行加密判断并处理

            var postDataStr = postDataDocument.ToString();
            XDocument decryptDoc = postDataDocument;
            if (_postModel != null && postDataDocument.Root.Element("Encrypt") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("Encrypt").Value))
            {
                //使用了加密

                string sToken =Config.ServerToken;
                string sAppID = Config.ServerAppID;
                string sEncodingAESKey = Config.ServerEncodingAESKey;

                Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                string msgXml = null;
                var result = wxcpt.DecryptMsg(_postModel.Msg_Signature, _postModel.Timestamp, _postModel.Nonce, postDataStr, ref msgXml);

                //判断result类型
                if (result != 0)
                {
                    //验证没有通过，取消执行

                    return null;
                }

                decryptDoc = XDocument.Parse(msgXml);//完成解密
            }
            return decryptDoc;
        }

        /// <summary>
        /// 获取第三方平台access_token
        /// </summary>
        /// <param name="component_appid"></param>
        /// <param name="component_appsecret"></param>
        /// <param name="component_verify_ticket"></param>
        /// <returns></returns>
        public static ResponseComponentToken Component_token(string component_verify_ticket)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/component/api_component_token";
            object data = null;
            data = new
            {
                component_appid = Config.ServerAppID,
                component_appsecret = Config.ServerAppSecret,   
                component_verify_ticket = component_verify_ticket
            };
            return CommonJsonSend.Send<ResponseComponentToken>("", urlFormat, data, timeOut: Config.TIME_OUT);
        }


        /// <summary>
        /// 用于获取预授权码。预授权码用于公众号授权时的第三方平台方安全验证
        /// </summary>
        /// <param name="component_verify_ticket"></param>
        /// <returns></returns>
        public static ResponseCreatePreauthCode Create_preauthcode(string component_access_token)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token={0}";
            object data = null;
            data = new
            {
                component_appid = Config.ServerAppID,
            };
            return CommonJsonSend.Send<ResponseCreatePreauthCode>(component_access_token, urlFormat, data, timeOut: Config.TIME_OUT);
        }

        /// <summary>
        /// 使用授权码换取公众号的授权信息
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <returns></returns>
        public static PublicWechatAuthorizerInfo Query_auth(string component_access_token, string auth_code_value)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token={0}";
            object data = null;
            data = new
            {
                component_appid = Config.ServerAppID,
                authorization_code = auth_code_value
            };
            return CommonJsonSend.Send<PublicWechatAuthorizerInfo>(component_access_token, urlFormat, data, timeOut: Config.TIME_OUT);
        }

        /// <summary>
        /// 获取授权方的账户信息
        /// </summary>
        /// <param name="component_access_token">第三方平台access_token</param>
        /// <param name="authorizer_appid">授权方appid</param>
        /// <returns></returns>
        public static PublicAuthorizerUserInfo Get_authorizer_info(string component_access_token, string authorizer_appid)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token={0}";
            object data = null;
            data = new
            {
                component_appid = Config.ServerAppID,
                authorizer_appid = authorizer_appid
            };
            return CommonJsonSend.Send<PublicAuthorizerUserInfo>(component_access_token, urlFormat, data, timeOut: Config.TIME_OUT);
        }


        /// <summary>
        /// 获取（刷新）授权公众号的令牌
        /// </summary>
        /// <param name="component_access_token">第三方平台appid</param>
        /// <param name="authorizerAppId">授权方appid</param>
        /// <param name="authorizer_refresh_token">授权方的刷新令牌，刷新令牌主要用于公众号第三方平台获取和刷新已授权用户的access_token，只会在授权时刻提供，请妥善保存。 一旦丢失，只能让用户重新授权，才能再次拿到新的刷新令牌</param>
        /// <returns></returns>
        public static ResponseAuthorizerToken Refresh_authorizer_token(string component_access_token, string authorizerAppId, string authorizer_refresh_token)
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token={0}",
                    component_access_token);
            var data = new
            {
                component_appid = Config.ServerAppID,
                authorizer_appid = authorizerAppId,
                authorizer_refresh_token = authorizer_refresh_token
            };
            return CommonJsonSend.Send<ResponseAuthorizerToken>(component_access_token, url, data, CommonJsonSendType.POST, timeOut: Config.TIME_OUT);
        }

       /// <summary>
        /// 获取授权方的选项设置信息
       /// </summary>
        /// <param name="component_access_token">第三方平台appid</param>
        /// <param name="authorizerAppId">授权方appid</param>
        /// <param name="optionName">选项值</param>
       /// <returns></returns>
        public static ResponseAuthorizerOption GetAuthorizerOption(string component_access_token, string authorizerAppId, OptionName optionName)
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/component/ api_get_authorizer_option?component_access_token={0}",
                    component_access_token);

            var data = new
            {
                component_appid = Config.ServerAppID,
                authorizer_appid = authorizerAppId,
                option_name = optionName
            };

            return CommonJsonSend.Send<ResponseAuthorizerOption>(component_access_token, url, data, CommonJsonSendType.POST, timeOut: Config.TIME_OUT);
        }

        /// <summary>
        /// 设置授权方的选项信息
        /// </summary>
        /// <param name="componentAccessToken">服务开发方的access_token</param>
        /// <param name="authorizerAppId">授权公众号appid</param>
        /// <param name="optionName">选项名称</param>
        /// <param name="optionValue">设置的选项值</param>
        /// <returns></returns>
        public static WxJsonResult SetAuthorizerOption(string component_access_token, string authorizerAppId, OptionName optionName, int optionValue)
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/component/ api_set_authorizer_option?component_access_token={0}",
                    component_access_token);

            var data = new
            {
                component_appid = Config.ServerAppID,
                authorizer_appid = authorizerAppId,
                option_name = optionName,
                option_value = optionValue
            };

            return CommonJsonSend.Send<WxJsonResult>(component_access_token, url, data, CommonJsonSendType.POST, timeOut: Config.TIME_OUT);
        }
    }
}
