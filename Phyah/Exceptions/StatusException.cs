using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Exceptions
{
    public class StatusException : System.Exception
    {
        private readonly int _status;
        public StatusException(int status, string message, Exception innerException) : base(message, innerException)
        {
            _status = status;
        }
        public StatusException(int status, string message) : this(status, message, null)
        {
        }
        public StatusException(int status) : this(status, "", null)
        {
        }
        public StatusException(string message) : this(500, message, null)
        {
        }
        public StatusException(Exception innerException) : this(500, innerException.Message, innerException)
        {
        }
        public int Status { get => _status; }
    }
}
