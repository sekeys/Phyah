using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventHub.Upgrate
{
    public interface IRegister
    {
        void Register(string name,string receptorType);
    }
}
