using Microsoft.AspNetCore.Http;
using Phyah.Configuration;
using Phyah.Enumerable;
using Phyah.Exceptions;
using Phyah.Thread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Http
{
    public class HttpMiddlewareChannel : Channel.Channel
    {

        private RequestDelegate _next;
        public HttpMiddlewareChannel(RequestDelegate next)
        {
            _next = next;
        }
        protected override void CompletedCore()
        {
            Dispose();
        }

        protected override void OnErrorCore(Exception ex)
        {
            if (ex is StatusException)
            {
                ex.Output();
            }
        }

        protected override async void OnStartCore()
        {
            HttpContext context = Accessor<HttpContext>.Current;
            Input input = new Input();
            input.User = await IdentityService.Service.Get(context, (id) => { return null; });
            input.Verb = (Verbs)Enum.Parse(typeof(Verbs), context.Request.Method);
            input.FormHttp(context.Request);
            string requestPath = context.Request.Path.ToUriComponent().Trim(new char[] { '/', '\\' });
            if (string.IsNullOrWhiteSpace(requestPath))
            {
                requestPath = AppSetting.AppSettings["defaultweb"].ToString();//"home";
            }
            input.Body.Path = requestPath;
            //Chain.InvokeAsync();
            OnCompleted();
        }

        public async Task Invoke(HttpContext context)
        {
            Accessor<HttpContext>.Current = context;
            await Task.Run(() =>
            {
                OnStart();
            });
        }
    }
}
