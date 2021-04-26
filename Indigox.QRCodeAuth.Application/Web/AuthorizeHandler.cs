using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Indigox.QRCodeAuth.Application.Web
{
    public class AuthorizeHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
