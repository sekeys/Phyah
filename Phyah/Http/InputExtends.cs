

namespace Phyah.Http
{
    using Microsoft.AspNetCore.Http;
    using Phyah.Enumerable;
    using Phyah.Interface;
    using Phyah.Thread;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class InputExtends
    {
        public static QueryString QueryString(this Input input)
        {
           return input.Request().QueryString;
        }
        public static IQueryCollection Query(this Input input)
        {
            return input.Request().Query;
        }
        public static IRequestCookieCollection Cookies(this Input input)
        {
            return input.Request().Cookies;
        }
        public static IHeaderDictionary Header(this Input input)
        {
            return input.Request().Headers;
        }
        public static IFormCollection Form(this Input input)
        {
            return input.Request().Form;
        }

        public static void FormHttp(this Input input, HttpRequest request)
        {
            Require.NotNull(request);
            input.FromStream(request.Body);
            input.Protocol = Enumerable.Protocols.Http;
            input.Verb =(Verbs) Enum.Parse(typeof(Verbs), request.Method,true);
        }
        public static void FormHttp(this Input input, HttpContext context) => input.FormHttp(context.Request);
        public static void FormHttp(this Input input) => input.FormHttp(input.HttpContext());
        public static HttpContext HttpContext(this Input input) => Accessor<HttpContext>.Current;
        public static HttpRequest Request(this Input input) => Accessor<HttpContext>.Current.Request;
        public static HttpResponse Response(this Input input) =>Accessor<HttpContext>.Current.Response;

        public static void GetOutput(this Input input)
        {
            var output = Output.FromInput(input);
            
        }

        public static HttpContext HttpContext(this Output onput) => Accessor<HttpContext>.Current;
        public static HttpRequest Request(this Output onput) => Accessor<HttpContext>.Current.Request;
        public static HttpResponse Response(this Output onput) => Accessor<HttpContext>.Current.Response;
        
        public static IResponseCookies Cookies(this Output output)
        {
            return output.Response().Cookies;
        }
        public static void Cookies(this Output output, string key, string content)
        {
            output.Response().Cookies.Append(key, new Microsoft.Extensions.Primitives.StringValues(content));
        }
        public static IHeaderDictionary Header(this Output output)
        {
            return output.Response().Headers;
        }
        public static void Header(this Output output,string header,string content)
        {
             output.Response().Headers.Append(header,new Microsoft.Extensions.Primitives.StringValues(content));
        }
        public static async Task WriteAsync(this Output output,string content)
        {
           await output.Response().WriteAsync(content);
        }
        public static async Task WriteAsync(this Output output, string content,Encoding encoding)
        {
            await output.Response().WriteAsync(content,encoding);
        }

        public static async Task SendFileAsync(this Output output,string file)
        {
            await output.Response().SendFileAsync(file);
        }
        
        public static async void Content(this Output output,string content)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            if (!string.IsNullOrWhiteSpace(content))
            {
                await httpResp.WriteAsync(content);
            }
        }
        public static async void Content(this Output output, string content,string contentType)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            httpResp.ContentType = contentType;
            if (!string.IsNullOrWhiteSpace(content))
            {
                await httpResp.WriteAsync(content);
            }
        }
        
        public static async void Content(this Output response,string content,string contentType,Encoding contentEncoding)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                httpResp.ContentType = contentType;
            }
            if (contentEncoding == null)
            {
                contentEncoding = Encoding.UTF8;
            }
            Stream outputStream = httpResp.Body;
            var contents = contentEncoding.GetBytes(content);
            await outputStream.WriteAsync(contents, 0, content.Length);
        }
        public static async void FileStream(this Output response, FileStream filestream)
        {

            var httpResp = Accessor<HttpContext>.Current.Response;
            Stream outputStream = httpResp.Body;
            using (filestream)
            {
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int count = filestream.Read(buffer, 0, 0x1000);
                    if (count == 0)
                    {
                        return;
                    }
                    await outputStream.WriteAsync(buffer, 0, count);
                }
            }
        }
        public static async void FileStream(this Output response,FileStream filestream,string contentType)
        {

            var httpResp = Accessor<HttpContext>.Current.Response;
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                httpResp.ContentType = contentType;
            }
            Stream outputStream = httpResp.Body;
            using (filestream)
            {
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int count = filestream.Read(buffer, 0, 0x1000);
                    if (count == 0)
                    {
                        return;
                    }
                    await outputStream.WriteAsync(buffer, 0, count);
                }
            }
        }
        public static async void File(this Output response,string filename)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            await httpResp.SendFileAsync(filename);
        }

        public static async void Script(this Output response,string script)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            httpResp.ContentType = "application/x-javascript";
            await httpResp.WriteAsync(script);

        }

        public static async void Json(this Output response,object model)
        {
            var httpResp = Accessor<HttpContext>.Current.Response;
            httpResp.ContentType = "application/json";
            await httpResp.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }

        public static async void Status(this Output response)
        {
            await Task.Run(() =>
            {
                var httpResp = Accessor<HttpContext>.Current.Response;
                httpResp.StatusCode = response.Status;
            });
        }

        public static async void Redirect(this Output response,string uri,bool permanent=false)
        {
            await Task.Run(() =>
            {
                var httpResp = Accessor<HttpContext>.Current.Response;
                httpResp.Redirect(uri, permanent);
            });
        }
    }
}
