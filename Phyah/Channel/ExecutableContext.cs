using Phyah.Chain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Channel
{
    public class ExecutableContext
    {
        public void Emit(IExecutable executable)
        {
            executable.Execute();
        }
        //public void Emit(IExecutable executable)
        //{

        //}
    }
}
