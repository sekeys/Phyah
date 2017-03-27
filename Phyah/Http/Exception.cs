using Microsoft.AspNetCore.Http;
using Phyah.Exceptions;
using Phyah.Thread;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Phyah.Http
{
    public static class ExceptionExtends
    {
        public static void Output(this Exception ex)
        {
            if (typeof(StatusException).GetTypeInfo().IsAssignableFrom(ex.GetType()))
            {
                Accessor<HttpContext>.Current.Response.StatusCode=((StatusException)ex).Status;
            }
            else
            {
                Accessor<HttpContext>.Current.Response.StatusCode = 500;
            }
        }
    }
}
