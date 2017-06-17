using System;
using System.Collections.Generic;
using Phyah.Interface;

namespace Phyah.Web
{
    internal class HttpUser:IUser
    {
        public string Token { get; protected set; }
        

        public List<string> Roles { get; protected set; } = new List<string>();

        public string AuthenticationType { get; protected set; }


        public string Name { get; protected set; }
        

        public HttpUser()
        {
        }


        public HttpUser(string token) :this(token,"")
        {
        }
        public HttpUser(string token, string name) 
        {
            this.Token = token;
            this.Name = name;
        }
        public object UserData { get; set; }
        public bool IsAuthenticated
        {
            get;
            protected set;
        }
        
    }
}