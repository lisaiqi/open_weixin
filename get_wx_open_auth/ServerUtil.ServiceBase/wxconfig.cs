using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ServerUtil.ServiceBase
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class wxconfig
    {
        /// <summary>
        /// 请求超时设置（以毫秒为单位），默认为10秒。
        /// 说明：此处常量专为提供给方法的参数的默认值，不是方法内所有请求的默认超时时间。
        /// </summary>
        public const int TIME_OUT = 10000;

        #region 服务号相关配置
        public static string ServerToken
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerToken"];
            }
        }

        public static string ServerAppID
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerAppID"];
            }
        }

        public static string ServerAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerAppSecret"];
            }
        }
        public static string ServerEncodingAESKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerEncodingAESKey"];
            }
        }

        /// <summary>
        /// 服务号授权回调的URL
        /// </summary>
        public static string ServeAuthRedirectUri
        {
            get
            {
                return ConfigurationManager.AppSettings["ServeAuthRedirectUri"];
            }
        }
        #endregion

        /// <summary>
        /// 个人号授权回调的URL
        /// </summary>
        public static string AuthRedirectUri
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthRedirectUri"];
            }
        }
    }
}
