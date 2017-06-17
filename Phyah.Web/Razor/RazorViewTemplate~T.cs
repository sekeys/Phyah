using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Razor
{
    public abstract class RazorViewTemplate<T> : RazorViewTemplate
    {
        public T Model { private set; get; }




        public override void SetModel(object model, DynamicDictionary viewbag = null)
        {
            this.Model = (T)model;
            base.SetModel(model, viewbag);
        }

    }
}
