using Phyah.EventStore.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.EventStore.Transport.Http
{
    public class HttpClient
    {
        public const string CONTENTTYPE = "application/x-www-form-urlencoded";
        public static async Task<HttpMessage> SendAsync(string method, string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
        {
            var httpMessage = new HttpMessage();
            httpMessage.Verb = method;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = contentType;// CONTENTTYPE;
                                                  //request.Headers. = Encoding.UTF8.GetByteCount(postDataStr);
                request.CookieContainer = cookie;
                Stream myRequestStream = await request.GetRequestStreamAsync();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Dispose();

                var response = await request.GetResponseAsync();

                var cookievalue = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                httpMessage.Deserialize(myResponseStream);
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.Msg = ex.Message;
                return httpMessage;
            }
        }
        public static async Task<HttpMessage> PostAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("post", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> GetAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("get", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> PutAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("put", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> DeleteAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("delete", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> PatchAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("patch", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> OptionsAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("options", url, postDataStr, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> HeadAsync(string url, string postDataStr, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("head", url, postDataStr, contentType = CONTENTTYPE, cookie = null);


        public static async Task<HttpMessage> SendAsync(string method, string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
        {
            var httpMessage = new HttpMessage();
            httpMessage.Verb = method;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = contentType;// CONTENTTYPE;
                                                  //request.Headers. = Encoding.UTF8.GetByteCount(postDataStr);
                request.CookieContainer = cookie;
                Stream myRequestStream = await request.GetRequestStreamAsync();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(msg.Serialize());
                myStreamWriter.Dispose();

                var response = await request.GetResponseAsync();

                var cookievalue = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                httpMessage.Deserialize(myResponseStream);
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.Msg = ex.Message;
                return httpMessage;
            }
        }

        public static async Task<HttpMessage> PostAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("post", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> GetAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("get", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> PutAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("put", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> DeleteAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("delete", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> PatchAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("patch", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> OptionsAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("options", url, msg, contentType = CONTENTTYPE, cookie = null);
        public static async Task<HttpMessage> HeadAsync(string url, Message msg, string contentType = CONTENTTYPE, CookieContainer cookie = null)
            => await SendAsync("head", url, msg, contentType = CONTENTTYPE, cookie = null);
    }
}
