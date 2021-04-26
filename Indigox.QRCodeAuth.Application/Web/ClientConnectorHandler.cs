using Indigox.Common.Logging;
using Indigox.QRCodeAuth.Application.Biz;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Indigox.QRCodeAuth.Application.Web
{
    public class ClientConnectorHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable => false;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(DoTalking);
            }
        }

        public async Task DoTalking(AspNetWebSocketContext context)
        {
            Log.Debug("connection of " + context.RequestUri);
            string code = context.QueryString.Get("code");
            WebSocket socket = context.WebSocket;
            SocketHolder.Instance.Put(code, socket);

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                if (socket.State == WebSocketState.Open)
                {
                    //if(String.IsNullOrEmpty(code))
                    //{
                    //    code = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    //}
                    
                    //buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));

                    string url = "http://test-uum.szprl.com/DingLoginHandler.ashx?code="+ code;
                    byte[] qrcode = QRCodeUtil.GenerateQRCodeByte(url);
                    ArraySegment<byte> codeBuffer = new ArraySegment<byte>(qrcode);
                    await socket.SendAsync(codeBuffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    
                }
                else
                {
                    break;
                }
            }
        }
    }
}
