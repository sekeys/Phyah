using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Phyah.EventStore.Messages;

namespace Phyah.EventStore.Transport.Http
{
    public class HttpTransport : Transport
    {

        public Endpoint Endpoint { get; set; }
        public new HttpMessage Message { get; set; }

        public override void Send()
        {
            var cookie = new System.Net.CookieContainer();
            cookie.Add(new Uri(Endpoint.IP),Message.Cookie);
            var task= HttpClient.SendAsync(Message.Verb, Endpoint.IP, Message, HttpClient.CONTENTTYPE,cookie );
            task.Wait();
            task.Result.Context = Message.Context;
        }
        public override void SendNoWait()
        {
            var cookie = new System.Net.CookieContainer();
            cookie.Add(new Uri(Endpoint.IP), Message.Cookie);
            HttpClient.SendAsync(Message.Verb, Endpoint.IP, Message, HttpClient.CONTENTTYPE, cookie);
        }

        public override void Send(Action<Message, EventContext> callback)
        {
            var cookie = new System.Net.CookieContainer();
            cookie.Add(new Uri(Endpoint.IP), Message.Cookie);
            var task = HttpClient.SendAsync(Message.Verb, Endpoint.IP, Message, HttpClient.CONTENTTYPE, cookie);
            task.Wait();
            task.Result.Context = Message.Context;
            callback(task.Result, Message.Context);

        }

        public async override Task<Message> SendAsync()
        {
            var cookie = new System.Net.CookieContainer();
            cookie.Add(new Uri(Endpoint.IP), Message.Cookie);
            return await HttpClient.SendAsync(Message.Verb, Endpoint.IP, Message, HttpClient.CONTENTTYPE, cookie).ContinueWith<Message>(m => {
                m.Result.Context = Message.Context;
                return m.Result;
            });
            
        }
    }
}
