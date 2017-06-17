
namespace Phyah.Authentication
{
    using System;
    using System.Reflection;
    public class AuthenticationAttribute:Attribute
    {
        public Type Type { get; private set; }
        public AuthenticationAttribute(Type type)
        {
            if (!type.GetTypeInfo().IsAssignableFrom(typeof(IAuthentication)))
            {
                throw new ArgumentException("type参数");
            }
            Type = type;
        }
    }
}
