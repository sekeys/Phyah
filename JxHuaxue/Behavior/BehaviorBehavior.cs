using Phyah.Extensions;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class BehaviorBehavior : RestBehavior
    {
        public override string Text => "behavior";

        public async Task Get()
        {
            List<object> ls = new List<object>();
            var containers = Phyah.Web.BehaviorFactory.Factory.Caches();
            Type typeInfo = null;
            foreach (var type in containers)
            {
                typeInfo = type.Value.GetType();
                ls.Add(new
                {
                    text = type.Key,
                    description = typeInfo.Description(),
                    implemented = typeInfo.FullName
                });
            }
            await Json(ls);
        }
        public async Task Post()
        {
            List<object> ls = new List<object>();
            string impltemented = Request.Form["type"];
            Type imType = Phyah.Reflection.Assembly.Type(impltemented);
            BehaviorFactory.Factory.Cache(imType);
            await Status(200);
        }

        public async Task Head()
        {
            string text = Request.Form["text"];
            Type imType = Phyah.Web.BehaviorFactory.Factory.Cache(text)?.GetType();
            await Json(new
            {
                text = text,
                description = imType.Description(),
                implemented = imType?.FullName
            });
        }
    }
}
