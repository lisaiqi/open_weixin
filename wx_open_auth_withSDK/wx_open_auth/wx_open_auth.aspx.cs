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
using System.Net;
using System.Xml.Linq;
using Weixin.Open.SDK;
using Weixin.Open.SDK.Api;
using Weixin.Open.SDK.Entities;
using Weixin.Open.SDK.Helpers;
using WXSDK = Weixin.Open.SDK;

namespace wx_open_auth
{
    public partial class wx_open_auth : System.Web.UI.Page
    {
        public string _ServerAppID = "";
        public string _PreAuthCode = "";
        public string _RedirectUri = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Index();
        }

        #region 代公众号发起网页授权
        /// <summary>
        /// 获取个人微信号网页授权入口
        /// </summary>
        /// <returns></returns>
        public string Auth()
        {

            return UtilityAuth(WeixinInfo.authorizer_appid, "/Receive/TestResult", true);
        }
        /// <summary>
        /// 授权结果页
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string TestResult(string openid, string authorizer_appid)
        {
            string Openid = openid;//页面参数

            string Authorizer_appid = authorizer_appid;//页面参数
            return "";
        }

        /// <summary>
        /// 个人微信号授权
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="state">授权后跳转RUL</param>
        /// <param name="scope">是否显示授权信息</param>
        /// <returns></returns>
        public string UtilityAuth(string authorizer_appid, string state, bool isShow)
        {
            string url = ProxyOAuthApi.GetAuthorizeUrl(authorizer_appid, WXSDK.Config.AuthRedirectUri, isShow ? OAuthScope.snsapi_userinfo : OAuthScope.snsapi_base, state, WXSDK.Config.ServerAppID);

            return (url);

        }
        /// <summary>
        /// 个人微信号授权回调方法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public string AuthCallback(string code, string state, string appid)
        {
            //通过，用code换取access_token
            var result = ProxyOAuthApi.GetOpenAccessToken(appid, code, WXSDK.Config.ServerAppID, WeixinInfo.component_tokenInfo);
            if (result.errcode != ReturnCode.请求成功)
            {
                return ("错误：" + result.errmsg);
            }

            if (result.scope == OAuthScope.snsapi_userinfo.ToString())//获取用户信息
            {
                var userInfo = ProxyOAuthApi.GetUserInfo(result.access_token, result.openid);
                //todo:保存用户信息
            }
            return (GetParemeter(state, "openid=" + result.openid + "&authorizer_appid=" + appid));

        }
        /// <summary>
        /// 参数转换
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string GetParemeter(string url, string parameter)
        {
            if (url.IndexOf('?') > 0)
            {
                return url + "&" + parameter;
            }
            else
            {
                return url + "?" + parameter;
            }
        }
        #endregion

        #region 代公众号调用接口相关demo
        /// <summary>
        /// 一键授权入口
        /// </summary>
        /// <returns></returns>
        public void Index()
        {
            //try
            //{

            _ServerAppID = WXSDK.Config.ServerAppID;//页面参数
            _PreAuthCode = WeixinInfo.pre_auth_codeInfo;
            _RedirectUri = WXSDK.Config.ServeAuthRedirectUri;// 页面参数ServeAuthRedirectUri;

            string _authorizer_access_token = Request.QueryString["authorizer_access_token"];
            FileHelper.LogToTxt("_authorizer_access_token: " + _authorizer_access_token);
            string _expires_in = Request.QueryString["expires_in"];
            FileHelper.LogToTxt("_expires_in: " + _expires_in);
            Callback(Request.QueryString["authorizer_access_token"], Request.QueryString["expires_in"]);
            //return "";
            //}
            //catch (Exception ex)
            //{
            //    return (ex.Message + ex.StackTrace.ToString());
            //}

        }
        //授权回调事件 获取公众号信息
        // GET: /Receive/
        public string Callback(string auth_code, string expires_in)
        {
            try
            {
                FileHelper.LogToTxt("WeixinInfo.component_tokenInfo: " + WeixinInfo.component_tokenInfo);
                var authInfo = InterfaceApi.Query_auth(WeixinInfo.component_tokenInfo, auth_code);
                WeixinInfo.authorizer_appid = authInfo.authorization_info.authorizer_appid;
                FileHelper.LogToTxt("Callback authorizer_appid: " + authInfo.authorization_info.authorizer_appid);
                return GetAuth(authInfo.authorization_info.authorizer_appid);
            }
            catch (Exception ex)
            {
                return (ex.Message + ex.StackTrace.ToString());
            }

        }

        /// <summary>
        /// 获取授权帐号信息
        /// </summary>
        /// <returns></returns>
        public string GetAuth(string authorizerAppid)
        {
            try
            {
                var authInfo = InterfaceApi.Get_authorizer_info(WeixinInfo.component_tokenInfo, authorizerAppid);
                return (authInfo.authorization_info.appid);
            }
            catch (Exception ex)
            {
                return ("GetAuth:" + ex.Message + ex.ToString());
            }
        }

        /// <summary>
        /// 接收微信推送事件的消息方法
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public string Sysmessage(PostModel postModel)
        {
            try
            {
                var msg = InterfaceApi.Component_verify_ticket(postModel, Request.InputStream);
                if (msg.InfoType == ThirdPartyInfo.component_verify_ticket.ToString())
                {
                    FileHelper.LogToTxt("msg.ComponentVerifyTicket:" + msg.ComponentVerifyTicket);
                    //存储ticket
                    WeixinInfo.componentVerifyTicket = msg.ComponentVerifyTicket;
                }
                else if (msg.InfoType == ThirdPartyInfo.unauthorized.ToString())
                {
                    //取消事件 todo
                }
                return ("success");
            }
            catch (Exception ex)
            {
                FileHelper.LogToTxt(ex.Message + ex.StackTrace.ToString());
                return ("success");
            }

        }

        #endregion
    }

    /// <summary>
    /// 这个所有的数据应该持久化到数据库，不能每次在接口读，性能慢
    /// </summary>
    public static class WeixinInfo
    {
        /// <summary>
        /// 授权方Appid
        /// </summary>
        public static string authorizer_appid
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方平台access_token 
        /// </summary>
        public static string component_tokenInfo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(componentVerifyTicket))
                {
                    FileHelper.LogToTxt("componentVerifyTicket为空  10分钟后在试试吧");
                }
                return InterfaceApi.Component_token(componentVerifyTicket).component_access_token;
            }
        }
        /// <summary>
        /// Ticket内容
        /// </summary>
        public static string componentVerifyTicket
        {
            get;
            set;
        }
        /// <summary>
        /// 预授权码信息
        /// </summary>
        public static string pre_auth_codeInfo
        {
            get
            {
                return InterfaceApi.Create_preauthcode(component_tokenInfo).pre_auth_code;
            }
        }
    }
}