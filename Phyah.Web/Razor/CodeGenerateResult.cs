using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Razor
{
    public class CodeGenerateResult
    {
        public bool Success { set; get; }

        public List<string> Errors { set; get; }

    }
    public class CompileResult
    {
        public bool Success { set; get; }

        public List<string> Errors { set; get; }


    }
}
