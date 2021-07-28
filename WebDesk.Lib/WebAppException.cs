using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WebDesk
{
    /// <summary>
    /// Exception class to be used by this application
    /// </summary>
    public class WebAppException: Tripous.ExceptionEx
    { 
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException() : base()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message) : base(Message)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message, Exception InnerException) : base(Message, InnerException)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message, int ErrorCode) : base(Message, ErrorCode)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message, HttpStatusCode StatusCode) : base(Message, StatusCode)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message, int ErrorCode, Exception InnerException) : base(Message, ErrorCode, InnerException)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppException(string Message, HttpStatusCode StatusCode, Exception InnerException) : base(Message, StatusCode, InnerException)
        {
        }
    }
}
