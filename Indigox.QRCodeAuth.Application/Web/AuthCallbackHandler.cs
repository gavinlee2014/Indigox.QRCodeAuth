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

        private readonly string successResponse = @"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <title>扫码成功</title>

    <style type='text/css'>
        body {
            font-size: 14px;
            font-family: PingFang-SC-Regular,Helvetica,sans-serif;
        }
        .content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%,-50%);
            width: 100%;
            text-align: center;
        }
        .content h1
        {
            font-size: 10em;
            color: #f4ad42;
        }
        .content p
        {
            font-size: 4em;
            color: #333;
        }
        .count_down
        {
            color: #f4ad42;
        }
    </style>
</head>
<body>
    <div class='content'>
         <h1>扫码成功</h1>
         <p>请关闭当前页以继续</p>
     </div>
 </body>
 </html>";


        public void ProcessRequest(HttpContext context)
        {

            string id = context.Request.Params.Get("code");
            Log.Debug("id " + id + " start access");

            string userName = context.User.Identity.Name;
            Log.Debug("id " + id + " get user name " + userName);

            _ = MessageSender.Instance.SendAsync("TRANS", id + " " + userName);
            context.Response.Write(successResponse);
            //context.Response.Write("<html><header><title></title><script type='text/javascript'>window.location.href='success.html'</script></header><body></body></html>");
        }

    }
}
