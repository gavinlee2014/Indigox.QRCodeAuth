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
            //string content = "";
            //using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            //{
            //    content = reader.ReadToEnd();
            //}
            //AuthResultDTO result = JsonConvert.DeserializeObject<AuthResultDTO>(content);
            //WebSocket socket = SocketHolder.Instance.Get(result.code);

            //ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(result.AccountName + ":" + result.AccountPassword));
            //socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            string id = context.Request.Params.Get("code");
            //string userName = "indigox";
            //string pwd = "Aa123456";
            Log.Debug("id " + id + " start access");

            string userName = context.User.Identity.Name;
            Log.Debug("id " + id + " get user name " + userName);
            Task<bool> task = MessageSender.Instance.SendAsync("TRANS", id + " " + userName);
            if (task.Result)
            {
                context.Response.Write("<html><header><title></title><script type='text/javascript'>window.location.href='success.html'</script></header><body></body></html>");
            }
            else
            {
                context.Response.Write("<html><header><title></title><script type='text/javascript'>window.location.href='fail.html'</script></header><body></body></html>");
            }
            //task.Start();
            //_ = MessageSender.Instance.SendAsync("TRANS", id + " " + userName);
            //context.Response.Redirect("/success.html");
        }
            
    }

    //class AuthResultDTO
    //{
    //    public string ID;
    //    public string code;
    //    public string AccountName;
    //    public string AccountPassword;
    //}

}
