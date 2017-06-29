

namespace Phyah.Huaxue
{
    using System;
    using Phyah.Web;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Phyah.Extensions;

    public class TypeContainerBehavior : RestBehavior
    {
        public override string Text => "TypeContainer";

        public async Task Get()
        {
            List<object> ls = new List<object>();
            var containers = TypeContainer.TypeContainer.Container.Fetch();
            foreach (var type in containers)
            {
                ls.Add(new
                {
                    Interface = type.Key.FullName,
                    description = type.Key.Description(),
                    Implemented = type.Value.FullName
                });
            }
            await Json(ls);
        }
        public async Task Post()
        {
            List<object> ls = new List<object>();
            string interfaceType = Request.Form["interface"];
            string impltemented = Request.Form["implemented"];
            Type it = Phyah.Reflection.Assembly.Type(interfaceType);
            Type imType = Phyah.Reflection.Assembly.Type(impltemented);
            TypeContainer.TypeContainer.Container.Inject(it, imType);
            await Status(200);
        }

        public async Task Head()
        {
            List<object> ls = new List<object>();
            string interfaceType = Request.Form["interface"];
            Type it = Phyah.Reflection.Assembly.Type(interfaceType);
            var implemented = TypeContainer.TypeContainer.Container.Fetch(it);

            await Json(new
            {
                baseType = it,
                description=it.Description(),
                implemented = implemented
            });
        }
    }
}
