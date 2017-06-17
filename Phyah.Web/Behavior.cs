

namespace Phyah.Web
{
    using Microsoft.AspNetCore.Http;
    using Phyah.Concurrency;
    using Phyah.Enumerable;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    public abstract class Behavior : IBehavior
    {
        public HttpContext HttpContext => AccessorContext.DefaultContext?.Get<HttpContext>();
        public IUser User => AccessorContext.DefaultContext?.Get<IUser>();

        public  Verbs Verb=> Processor.Current.Verb;
        public Behavior()
        {
        }
        public abstract string Text { get; }

        public abstract Task Invoke();

        protected async Task Json(string data) => await Json(data);
        protected async Task Json(object data, string contentType = "application/json")
        {
            await HttpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }

        protected async Task View()
        {

        }
        protected async Task Redirect(string location)
        {

            await Task.Run(() =>
            {
                var httpResp = HttpContext.Response;
                httpResp.Redirect(location);
            });
        }

        protected async Task Redirect(string location, bool permanent)
        {

            await Task.Run(() =>
            {
                var httpResp = HttpContext.Response;
                httpResp.Redirect(location, permanent);
            });
        }
        protected async Task Html(string html) => await Content(html, "text/html");

        protected async Task HtmlFile(string path) => await Content(System.IO.File.ReadAllText(path), "text/html");
        //protected async Task File()
        //{

        //}
        //protected async Task Html() =>await HtmlFile()
        public HttpResponse Response => HttpContext.Response;
        public HttpRequest Request => HttpContext.Request;
        protected async Task Content(string content, string contentType)
        {
            var httpResp = HttpContext.Response;

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                httpResp.ContentType = contentType;
            }
            if (!string.IsNullOrWhiteSpace(content))
            {
                await httpResp.WriteAsync(content);
            }
        }
        protected async Task File(string content, string contentType, Encoding encoding)
        {

            var httpResp = HttpContext.Response;
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                httpResp.ContentType = contentType;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            Stream outputStream = httpResp.Body;
            var contents = encoding.GetBytes(content);
            await outputStream.WriteAsync(contents, 0, content.Length);
        }
        protected async Task File(string content, Encoding encoding) => await File(content, null, encoding);
        protected async Task File(Stream file, string contentType)
        {
          
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                Response.ContentType = contentType;
            }
            Stream outputStream = Response.Body;
            using (file)
            {
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int count = file.Read(buffer, 0, 0x1000);
                    if (count == 0)
                    {
                        return;
                    }
                    await outputStream.WriteAsync(buffer, 0, count);
                }
            }
        }
        protected async Task FileStream(Stream file, string contentType) => await File(file, null);
        protected async Task File(string file) => await File(System.IO.File.OpenRead(file), null);
        protected async Task File(string file, string contentType) => await File(System.IO.File.OpenRead(file),contentType);

        protected async Task Status(int code)
        {
            HttpContext.Response.StatusCode = code;
        }
        protected async Task Status(int code, string message)
        {
            HttpContext.Response.StatusCode = code;
            await HttpContext.Response.WriteAsync(message);
        }
        protected async Task NotFound() => await Status(StatusCode.NOTFOUND);
        protected async Task Script(string script)
        {
            
            Response.ContentType = "application/x-javascript";
            await Response.WriteAsync(script);
        }
        protected async Task ScriptFile(string script) =>await File(script, "application/x-javascript");
    }
}
