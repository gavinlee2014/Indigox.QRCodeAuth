using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Indigox.QRCodeAuth.Application.Biz
{
    public class SocketHolder
    {
        private static SocketHolder instance = new SocketHolder();
        private SocketHolder() { }

        public static SocketHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SocketHolder();
                }
                return instance;
            }
        }

        private IDictionary<string, WebSocket> activeSocket = new Dictionary<string, WebSocket>();

        public void Put(string id, WebSocket socket)
        {
            if (activeSocket.ContainsKey(id))
            {
                activeSocket[id] = socket;
            }
            else
            {
                activeSocket.Add(id, socket);
            }
        }

        public WebSocket Get(string id)
        {
            return activeSocket[id];
        }
    }
}
