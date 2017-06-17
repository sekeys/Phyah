using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web
{
    public static class MimeTypeUtils
    {
        private static Dictionary<string, string> MimeTypeMap = new Dictionary<string, string>();
        static MimeTypeUtils()
        {
            InitMap();
        }
        private static void InitMap()
        {
            //.tif
            MimeTypeMap.Add(".tif", "image/tiff");
            MimeTypeMap.Add(".asp", "text/asp");
            MimeTypeMap.Add(".asx", "video/x-ms-asf");
            MimeTypeMap.Add(".au", "audio/basic");
            MimeTypeMap.Add(".avi", "video/avi");
            MimeTypeMap.Add(".awf", "application/vnd.adobe.workflow");
            MimeTypeMap.Add(".biz", "text/xml");
            MimeTypeMap.Add(".bmp", "application/x-bmp");
            MimeTypeMap.Add(".bot", "application/x-bot");
            MimeTypeMap.Add(".cit", "application/x-cit");
            MimeTypeMap.Add(".class", "java/*");
            MimeTypeMap.Add(".csi", "application/x-csi");
            MimeTypeMap.Add(".css", "text/css");
            MimeTypeMap.Add(".dcd", "text/xml");
            MimeTypeMap.Add(".dcx", "application/x-dcx");
            MimeTypeMap.Add(".dib", "application/x-dib");
            MimeTypeMap.Add(".dll", "application/x-msdownload");
            MimeTypeMap.Add(".doc", "application/msword");
            MimeTypeMap.Add(".dot", "application/msword");
            MimeTypeMap.Add(".drw", "application/x-drw");
            MimeTypeMap.Add(".dtd", "text/xml");
            MimeTypeMap.Add(".dwf", "Model/vnd.dwf");
            MimeTypeMap.Add(".etd", "application/x-ebx");
            MimeTypeMap.Add(".exe", "application/x-msdownload");
            MimeTypeMap.Add(".gif", "image/gif");
            MimeTypeMap.Add(".gl2", "application/x-gl2");
            MimeTypeMap.Add(".hrf", "application/x-hrf");
            MimeTypeMap.Add(".hta", "application/hta");
            MimeTypeMap.Add(".htc", "text/x-component");
            MimeTypeMap.Add(".htm", "text/html");
            MimeTypeMap.Add(".html", "text/html");
            MimeTypeMap.Add(".htx", "text/html");
            MimeTypeMap.Add(".htt", "text/webviewhtml");
            MimeTypeMap.Add(".icb", "application/x-icb");
            MimeTypeMap.Add(".ico", "image/x-icon");
            MimeTypeMap.Add(".iff", "application/x-iff");
            MimeTypeMap.Add(".ig4", "application/x-g4");
            MimeTypeMap.Add(".igs", "application/x-igs");
            MimeTypeMap.Add(".iii", "application/x-iphone");
            MimeTypeMap.Add(".img", "application/x-img");
            MimeTypeMap.Add(".ins", "application/x-internet-signup");
            MimeTypeMap.Add(".java", "java/*");
            MimeTypeMap.Add(".jfif", "image/jpeg");
            MimeTypeMap.Add(".jpe", "image/jpeg");
            MimeTypeMap.Add(".jpeg", "image/jpeg");
            MimeTypeMap.Add(".jpg", "image/jpeg");
            MimeTypeMap.Add(".js", "application/x-javascript");
            MimeTypeMap.Add(".jsp", "text/html");
            MimeTypeMap.Add(".la1", "audio/x-liquid-file");
            MimeTypeMap.Add(".mdb", "application/msaccess");
            MimeTypeMap.Add(".mfp", "application/x-shockwave-flash");
            MimeTypeMap.Add(".mht", "message/rfc822");
            MimeTypeMap.Add(".mhtml", "message/rfc822");
            MimeTypeMap.Add(".mi", "application/x-mi");
            MimeTypeMap.Add(".mid", "audio/mid");
            MimeTypeMap.Add(".midi", "audio/mid");
            MimeTypeMap.Add(".mil", "application/x-mil");
            MimeTypeMap.Add(".mml", "text/xml");
            MimeTypeMap.Add(".mnd", "audio/x-musicnet-download");
            MimeTypeMap.Add(".mns", "audio/x-musicnet-stream");
            MimeTypeMap.Add(".mocha", "application/x-javascript");
            MimeTypeMap.Add(".movie", "video/x-sgi-movie");
            MimeTypeMap.Add(".mp1", "audio/mp1");
            MimeTypeMap.Add(".mp2", "audio/mp2");
            MimeTypeMap.Add(".mp3", "audio/mpeg");
            MimeTypeMap.Add(".mp2v", "audio/mp1");
            MimeTypeMap.Add(".mp4", "audio/mp4");
            MimeTypeMap.Add(".mpd", "application/vnd.ms-project");
            MimeTypeMap.Add(".mpe", "video/x-mpeg");
            MimeTypeMap.Add(".net", "image/pnetvue");
            MimeTypeMap.Add(".nrf", "application/x-nrf");
            MimeTypeMap.Add(".nws", "message/rfc822");
            MimeTypeMap.Add(".odc", "text/x-ms-odc");
            MimeTypeMap.Add(".pdf", "application/pdf");
            MimeTypeMap.Add(".png", "image/png");
            MimeTypeMap.Add(".ppt", "application/vnd.ms-powerpoint");
            MimeTypeMap.Add(".ppx", "application/vnd.ms-powerpoint");
            MimeTypeMap.Add(".ps", "application/postscript");
            MimeTypeMap.Add(".swf", "application/x-shockwave-flash");
            MimeTypeMap.Add(".tif", "image/tiff");
            MimeTypeMap.Add(".tiff", "image/tiff");
            MimeTypeMap.Add(".txt", "text/plain");
            MimeTypeMap.Add(".vdx", "application/vnd.visio");
            MimeTypeMap.Add(".wav", "audio/wav");
            MimeTypeMap.Add(".xls", "application/x-xls");
            MimeTypeMap.Add(".xhtml", "text/html");
            MimeTypeMap.Add(".xlw", "application/x-xlw");
            MimeTypeMap.Add(".xml", "text/xml");
            MimeTypeMap.Add(".xsd", "text/xml");
            MimeTypeMap.Add(".xsl", "text/xml");
            MimeTypeMap.Add(".xslt", "text/xml");
            MimeTypeMap.Add(".xwd", "text/xml");
            MimeTypeMap.Add(".apk", "application/vnd.android.package-archive");
            MimeTypeMap.Add(".xap", "application/x-silverlight-app");
            MimeTypeMap.Add(".ipa", "application/vnd.iphone");
            MimeTypeMap.Add(".vsx", "application/vnd.visio");
            MimeTypeMap.Add(".vtx", "application/vnd.visio");
            MimeTypeMap.Add(".vsw", "application/vnd.visio");
            MimeTypeMap.Add(".vst", "application/vnd.visio");
            MimeTypeMap.Add(".vss", "application/vnd.visio");
            //MimeTypeMap.Add("", "");
        }

        public static string ContentType(string key)
        {
            if (!key.StartsWith("."))
            {
                key = "." + key;
            }
            if (MimeTypeMap.ContainsKey(key))
            {
                return MimeTypeMap[key];
            }
            return "";
        }
    }
}
