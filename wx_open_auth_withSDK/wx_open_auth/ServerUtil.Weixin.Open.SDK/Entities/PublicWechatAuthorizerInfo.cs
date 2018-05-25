﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{
    /// <summary>
    /// 公众号的授权信息
    /// </summary>
    public class PublicWechatAuthorizerInfo
    {
        public PublicAuthorization_info authorization_info { get; set; }

       
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class PublicAuthorization_info
    {
        /// <summary>
        /// 授权方appid
        /// </summary>
        public string authorizer_appid { get; set; }

        /// <summary>
        /// 授权方令牌（在授权的公众号具备API权限时，才有此返回值）
        /// </summary>
        public string authorizer_access_token { get; set; }

        /// <summary>
        /// 有效期（在授权的公众号具备API权限时，才有此返回值）
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 刷新令牌（在授权的公众号具备API权限时，才有此返回值），刷新令牌主要用于公众号第三方平台获取和刷新已授权用户的access_token，只会在授权时刻提供，请妥善保存。 一旦丢失，只能让用户重新授权，才能再次拿到新的刷新令牌
        /// </summary>
        public string authorizer_refresh_token { get; set; }
        public List<funcscope_categorys> func_info { get; set; }
       
    }

    /// <summary>
    /// 公众号授权给开发者的权限集列表（请注意，当出现用户已经将消息与菜单权限集授权给了某个第三方，再授权给另一个第三方时，由于该权限集是互斥的，后一个第三方的授权将去除此权限集，开发者可以在返回的func_info信息中验证这一点，避免信息遗漏），1到8分别代表：
    ///消息与菜单权限集
    ///用户管理权限集
    ///帐号管理权限集
    ///网页授权权限集
    ///微信小店权限集
    ///多客服权限集
    ///业务通知权限集
    ///微信卡券权限集
    /// </summary>
    public class funcscope_categorys
    {
        public category funcscope_category { get; set; }
    }

    public class category
    {
        public int id { get; set; }
    }


 
     
}