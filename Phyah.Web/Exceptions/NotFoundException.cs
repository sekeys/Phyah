using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string msg) : base( msg)
        {

        }
    }
}
