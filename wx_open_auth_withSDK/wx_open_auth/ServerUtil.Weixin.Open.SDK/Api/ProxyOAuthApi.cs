using Weixin.Open.SDK.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weixin.Open.SDK.HttpUtility;
namespace Weixin.Open.SDK.Api
{
    /// <summary>
    /// 代公众号发起网页授权API
    /// </summary>
    public  class ProxyOAuthApi
    {
        /// <summary>
        /// 获取授权连接
        /// </summary>
        /// <param name="appId">公众号的appid</param>
        /// <param name="redirectUrl">重定向地址，需要urlencode，这里填写的应是服务开发方的回调地址</param>
        /// <param name="scope">授权作用域，拥有多个作用域用逗号（,）分隔</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写任意参数值，最多128字节</param>
        /// <param name="component_appid">服务方的appid，在申请创建公众号服务成功后，可在公众号服务详情页找到</param>
        /// <param name="responseType">默认为填code</param>
        /// <returns>URL</returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl,  OAuthScope scope,string state, string component_appid,string responseType = "code")
        {
            var url =
                string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}&component_appid={5}#wechat_redirect",
                                appId, redirectUrl.UrlEncode(), responseType, scope, state,component_appid);
            return url;
        }
        /// <summary>
        /// 通过code换取access_token
        /// </summary>
        /// <param name="appId">公众号的appid</param>
        /// <param name="code">填写第一步获取的code参数</param>
        /// <param name="component_appid">服务开发方的appid</param>
        /// <param name="component_access_token">服务开发方的access_token</param>
        /// <returns></returns>
        public static string Access_token(string appId, string code,string component_appid,string component_access_token)
        {
            var url =
                string.Format("https://api.weixin.qq.com/sns/oauth2/component/access_token?appid={0}&code={1}&grant_type=authorization_code&component_appid={2}&component_access_token={3}",
                                appId, code, component_appid, component_access_token);
            return url;
        }
        /// <summary>
        /// 通过code换取access_token
        /// </summary>
        /// <param name="appId">公众号的appid</param>
        /// <param name="code">填写第一步获取的code参数</param>
        /// <param name="componentAppId">服务开发方的appid</param>
        /// <param name="componentAccessToken">服务开发方的access_token</param>
        /// <param name="grantType">填authorization_code</param>
        /// <returns></returns>
        public static ResponseOAuthOpenAccessToken GetOpenAccessToken(string appId, string code, string componentAppId, string componentAccessToken, string grantType = "authorization_code")
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/component/access_token?appid={0}&code={1}&grant_type={2}&component_appid={3}&component_access_token={4}",
                    appId, code, grantType, componentAppId, componentAccessToken);

            return Get.GetJson<ResponseOAuthOpenAccessToken>(url);
        }
        /// <summary>
        /// 刷新access_token
        /// 由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，refresh_token拥有较长的有效期（30天），当refresh_token失效的后，需要用户重新授权。
        /// </summary>
        /// <param name="appId">公众号的appid</param>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="componentAppId">服务开发商的appid</param>
        /// <param name="componentAccessToken">服务开发方的access_token</param>
        /// <param name="grantType">填refresh_token</param>
        /// <returns></returns>
        public static ResponseOAuthOpenAccessToken RefreshOpenToken(string appId, string refreshToken, string componentAppId, string componentAccessToken, string grantType = "refresh_token")
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/component/refresh_token?appid={0}&grant_type={1}&component_appid={2}&component_access_token={3}&refresh_token={4}",
                    appId, grantType, componentAppId, componentAccessToken, refreshToken);

            return Get.GetJson<ResponseOAuthOpenAccessToken>(url);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public static ResponseOAuthUserInfo GetUserInfo(string accessToken, string openId, Language lang = Language.zh_CN)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}", accessToken, openId, lang);
            return CommonJsonSend.Send<ResponseOAuthUserInfo>(null, url, null, CommonJsonSendType.GET);
        }
    }
}
