using System.Web;

namespace Indigox.QRCodeAuth.Application.Web
{
    public class AwakeHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable => false;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            context.Response.Write("success");
        }
    }
}
