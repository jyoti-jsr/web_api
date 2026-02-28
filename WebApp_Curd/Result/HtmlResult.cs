
using System.Text;

namespace WebApp_Curd.Result
{
    public class HtmlResult : IResult
    {
        private readonly string html;

        public HtmlResult(string html)
        {
            this.html = html;
        }
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(html);

            await httpContext.Response.WriteAsync(html);
        }
    }
}
