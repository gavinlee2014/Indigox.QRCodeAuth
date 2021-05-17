using Indigox.Common.Logging;
using System;
using System.Configuration;
using System.Net;
using System.Web;

namespace Indigox.QRCodeAuth.Application.Web
{
    public class AuthModule : IHttpModule
    {
        /// <summary>
        /// 您将需要在网站的 Web.config 文件中配置此模块
        /// 并向 IIS 注册它，然后才能使用它。有关详细信息，
        /// 请参阅以下链接: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule 成员

        public void Dispose()
        {
            //此处放置清除代码。
        }

        public void Init(HttpApplication context)
        {
            // 下面是如何处理 LogRequest 事件并为其 
            // 提供自定义日志记录实现的示例
            context.LogRequest += new EventHandler(OnLogRequest);
            context.AuthenticateRequest += Context_AuthenticateRequest;
        }

        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            
            string userName = "";
            if ((context.User != null) && (context.User.Identity != null))
            {
                userName = context.User.Identity.Name;
            }

            Log.Debug("get user name " + userName);
            if (!IsIgnorePage(context))
            {
                if (String.IsNullOrEmpty(userName))
                {
                    context.Response.Redirect("sso/login?ReturnUrl=" + WebUtility.UrlEncode(context.Request.RawUrl));
                }
            }

        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //可以在此处放置自定义日志记录逻辑
        }

        private bool IsIgnorePage(HttpContext context)
        {
            string requestUrl = context.Request.Url.PathAndQuery;

            string ignorePathes = ConfigurationManager.AppSettings["SSO_MODULE_IGNORE_PATH"];
            if (String.IsNullOrEmpty(ignorePathes))
            {
                return true;
            }

            string[] ignorePaths = ignorePathes.Split(';');

            foreach (var ignorePath in ignorePaths)
            {
                //Log.Debug("ignorePath:" + ignorePath + " compare to " + requestUrl + " result is " + requestUrl.StartsWith(ignorePath, StringComparison.CurrentCultureIgnoreCase));
                if (string.IsNullOrEmpty(ignorePath))
                {
                    continue;
                }
                if (requestUrl.StartsWith(ignorePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
