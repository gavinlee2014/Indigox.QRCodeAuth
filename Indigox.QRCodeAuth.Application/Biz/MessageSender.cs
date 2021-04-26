using Indigox.Common.Logging;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Indigox.QRCodeAuth.Application.Biz
{
    class MessageSender
    {
        private static MessageSender instance = new MessageSender();
        public static MessageSender Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageSender();
                }
                return instance;
            }
        }

        private EasyClient client;
        private MessageSender()
        {
            client = new EasyClient();
            client.Initialize(new DefaultReceiveFilter(), DispatchMessage);
            client.Connected += Client_Connected;
            client.Error += Client_Error;
            client.Closed += Client_Closed;
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            Log.Debug("connection closed");

        }

        private void Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Log.Error("connection error " + e.Exception.ToString());
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            Log.Debug("connected");
        }

        private async Task<bool> ConnectAsync()
        {
            string endPoint = ConfigurationManager.AppSettings.Get("TRANSFER_ENDPOINT");
            string ip = "";
            int port = 2021;

            if (endPoint.IndexOf(":") > 0)
            {
                ip = endPoint.Substring(0, endPoint.IndexOf(":"));
                port = Convert.ToInt32(endPoint.Substring(endPoint.IndexOf(":") + 1));
            }

            return await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        private void DispatchMessage(StringPackageInfo packageInfo)
        {
        }
        public async Task<bool> SendAsync(string command, string message)
        {
            bool connected = client.IsConnected;
            if (!connected)
            {
                connected = await ConnectAsync();
            }
            Log.Debug("send command: " + command + " " + message);
            if (connected)
            {
                try
                {
                    client.Send(Encoding.UTF8.GetBytes(command + " " + message + "\r\n"));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
    class DefaultReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        private const string terminator = "\r\n";
        public DefaultReceiveFilter() : base(Encoding.ASCII.GetBytes(terminator))
        {

        }
        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            string content = bufferStream.ReadString((int)bufferStream.Length, Encoding.UTF8);
            if (content.IndexOf(terminator) > -1)
            {
                content = content.Substring(0, content.IndexOf(terminator));
            }

            return new StringPackageInfo(content.Substring(0, content.IndexOf("")), content, new string[0]);
        }
    }
}
