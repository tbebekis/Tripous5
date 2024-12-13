namespace Tripous
{
    /// <summary>
    /// An exception class used by this system.
    /// </summary>
    public class ExceptionEx : ApplicationException
    {

        /* construction */
        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx()
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message)
            : base(Message)
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message, int ErrorCode)
            : base(Message)
        {
            this.ErrorCode = ErrorCode;
        }
        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message, int ErrorCode, Exception InnerException)
            : base(Message, InnerException)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message, HttpStatusCode StatusCode)
            : base(Message)
        {
            this.StatusCode = StatusCode;
        }
        /// <summary>
        /// constructor
        /// </summary>
        public ExceptionEx(string Message, HttpStatusCode StatusCode, Exception InnerException)
            : base(Message, InnerException)
        {
            this.StatusCode = StatusCode;
        }
        
        /* public */
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();
 
            SB.Append(base.ToString());
            AddDataDictionaryTo(this, SB);

            return SB.ToString();
        }

        /* static */
        /// <summary>
        /// Adds E.Data dictionary information to SB
        /// </summary>
        static public void AddDataDictionaryTo(Exception E, StringBuilder SB)
        {
            if ((E != null) && (E.Data.Count > 0))
            {
                SB.AppendLine();

                foreach (object Key in E.Data.Keys)
                    SB.AppendLine(string.Format("{0}: {1}", Key, E.Data[Key]));

                SB.AppendLine();
            }

        }
        /// <summary>
        /// Returns a string containing all exception information,
        /// including the Data dictionary and the inner exceptions
        /// </summary>
        static public string GetExceptionText(Exception Ex)
        {
            StringBuilder SB = new StringBuilder();

            Action<Exception> Proc = null;
            Proc = delegate (Exception E)
            {
                if (E != null)
                {
                    SB.AppendLine(E.ToString());
                    AddDataDictionaryTo(E, SB);

                    if (E.InnerException != null)
                    {
                        SB.AppendLine(" ----------------------------------------------------------------");
                        SB.AppendLine(" ");

                        Proc(E.InnerException);
                    }
                }
            };

            //SB.AppendLine(" ================================================================");
            SB.AppendLine(" ");

            Proc(Ex);

            return SB.ToString();

        }
        

        /* properties */
        /// <summary>
        /// Error code
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    }
}
