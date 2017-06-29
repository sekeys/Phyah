using Microsoft.AspNetCore.WebUtilities;
using Phyah.Configuration;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class UploadBehavior : Behavior
    {
        public override string Text => "upload";
        //        HttpContext context = enchyma["context"] as HttpContext;
        //        var req = enchyma.TypeOf<IEnchyma>("forms");
        //        string p = Immuno.Security.Base64Encoder.DecodeBase64(req.StringEx("parent"));
        //        FileinfoModel parent = null;
        //        var dbcontext = new DbContext<FileinfoModel>().Connect("Vitae", "Editor_FileInfo");
        //            if (!string.IsNullOrWhiteSpace(p))
        //            {
        //                parent = dbcontext.FindOne(x => x.UId == p);
        //            }
        //            foreach (var item in context.Request.Form.Files)
        //            {

        //                string type = req.StringEx("type");
        //    string file = req.StringEx("file");

        //    var path = parent == null ? "root\\" + item.FileName : parent.Path + "\\" + item.FileName;




        //                using (FileStream fs = new FileStream(dir, FileMode.CreateNew))
        //                {
        //                    item.CopyTo(fs);
        //                };
        //dbcontext.Insert(model);
        //            }
        //return Response(200);

        const int chunkSize = 1024000;
        public override async Task Invoke()
        {
            try
            {
                string folder = System.IO.Path.Combine(AppSetting.AppSettings["hostdir"].ToString(), "wwwroot", "images");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                //List<string> paths = new List<string>();
                //foreach (var item in HttpContext.Request.Form.Files)
                //{
                //    var guid = Guid.NewGuid().ToString();
                //    var path = System.IO.Path.Combine(folder, guid + item.FileName);
                //    using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                //    {
                //        item.CopyTo(fs);
                //    };
                //    paths.Add($"wwwroot/images/{guid}{item.FileName}");
                //}
                string path = "";
                var boundary = GetBoundary(HttpContext.Request.ContentType);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                var section = await reader.ReadNextSectionAsync();
                while (section != null)
                {
                    // process each image
                    var buffer = new byte[chunkSize];
                    var bytesRead = 0;
                    var fileName = GetFileName(section.ContentDisposition);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        section = await reader.ReadNextSectionAsync();
                        continue;
                    }
                    var guid = Guid.NewGuid().ToString();
                    path = $"wwwroot/images/{guid}{fileName}";
                    using (var stream = new FileStream(path, FileMode.CreateNew))
                    {
                        bytesRead = await section.Body.ReadAsync(buffer, 0, buffer.Length);
                        while (bytesRead > 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                            bytesRead = await section.Body.ReadAsync(buffer, 0, buffer.Length);
                        };
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                await Json(new { result = true, urls = new List<string>() { path } });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }


        private static bool IsMultipartContentType(string contentType)
        {
            return
                !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string GetBoundary(string contentType)
        {
            var elements = contentType.Split(' ');
            var element = elements.Where(entry => entry.StartsWith("boundary=")).First();
            var boundary = element.Substring("boundary=".Length);
            // Remove quotes
            if (boundary.Length >= 2 && boundary[0] == '"' &&
                boundary[boundary.Length - 1] == '"')
            {
                boundary = boundary.Substring(1, boundary.Length - 2);
            }
            return boundary;
        }

        private string GetFileName(string contentDisposition)
        {
            return contentDisposition
                .Split(';')
                .SingleOrDefault(part => part.Contains("filename"))
                ?.Split('=')
                .Last()
                .Trim('"');
        }
    }
}
