using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Open.SDK.Entities
{

    /// <summary>
    /// 授权方的账户信息
    /// </summary>
    public class PublicAuthorizerUserInfo
    {
        public PublicAuthorizer_Userinfo authorizer_info { get; set; }
       
        /// <summary>
        /// 授权信息
        /// </summary>
        public PublicAuthorization_Userinfo authorization_info { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicAuthorizer_Userinfo
    {
        /// <summary>
        /// 授权方昵称
        /// </summary>
        public string nick_name { get; set; }
        /// <summary>
        /// 授权方头像
        /// </summary>
        public string head_img { get; set; }

        /// <summary>
        /// 授权方公众号类型，0代表订阅号，1代表由历史老帐号升级后的订阅号，2代表服务号
        /// </summary>
        public service_type_info service_type_info { get; set; }

        /// <summary>
        /// 授权方认证类型，-1代表未认证，0代表微信认证，1代表新浪微博认证，2代表腾讯微博认证，3代表已资质认证通过但还未通过名称认证，4代表已资质认证通过、还未通过名称认证，但通过了新浪微博认证，5代表已资质认证通过、还未通过名称认证，但通过了腾讯微博认证
        /// </summary>
        public verify_type_info verify_type_info { get; set; }

        /// <summary>
        /// 授权方公众号的原始ID
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 授权方公众号所设置的微信号，可能为空
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// 二维码图片的URL，开发者最好自行也进行保存
        /// </summary>
        public string qrcode_url { get; set; }
    }


    /// <summary>
    /// 授权信息
    /// </summary>
    public class PublicAuthorization_Userinfo
    {
        /// <summary>
        /// 授权方appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 公众号授权给开发者的权限集列表（请注意，当出现用户已经将消息与菜单权限集授权给了某个第三方，再授权给另一个第三方时，由于该权限集是互斥的，后一个第三方的授权将去除此权限集，开发者可以在返回的func_info信息中验证这一点，避免信息遗漏），1到9分别代表：
        ///消息与菜单权限集
        ///用户管理权限集
        ///帐号管理权限集
        ///网页授权权限集
        ///微信小店权限集
        ///多客服权限集
        ///业务通知权限集
        ///微信卡券权限集
        ///微信扫一扫权限集
        /// </summary>
        public List<funcscope_categorys> func_info { get; set; }
      

    }

    /// <summary>
    /// 授权方公众号类型，0代表订阅号，1代表由历史老帐号升级后的订阅号，2代表服务号
    /// </summary>
    public class service_type_info
    {
        public int id { get; set; }
    }
    /// <summary>
    /// 授权方认证类型，-1代表未认证，0代表微信认证，1代表新浪微博认证，2代表腾讯微博认证，3代表已资质认证通过但还未通过名称认证，4代表已资质认证通过、还未通过名称认证，但通过了新浪微博认证，5代表已资质认证通过、还未通过名称认证，但通过了腾讯微博认证
    /// </summary>
    public class verify_type_info
    {
        public int id { get; set; }
    }

   
}
