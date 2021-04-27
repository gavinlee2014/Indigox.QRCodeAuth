using Indigox.Common.Logging;
using Indigox.QRCodeAuth.Application.Biz;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Indigox.QRCodeAuth.Application.Web
{
    public class AuthCallbackHandler : IHttpHandler
    {
        public bool IsReusable => true;


        public void ProcessRequest(HttpContext context)
        {

            string id = context.Request.Params.Get("code");
            Log.Debug("id " + id + " start access");

            string userName = context.User.Identity.Name;
            Log.Debug("id " + id + " get user name " + userName);

            _ = MessageSender.Instance.SendAsync("TRANS", id + " " + userName);
            context.Response.Write("<html><header><title></title><script type='text/javascript'>window.location.href='success.html'</script></header><body></body></html>");
        }

    }
}
