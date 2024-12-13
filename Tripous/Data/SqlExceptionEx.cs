namespace Tripous
{




    /// <summary>
    /// Tripous Sql exception
    /// </summary>
    public class SqlExceptionEx : ExceptionEx
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlExceptionEx(string Message, Exception InnerException, string CommandText = "")
            : base(Message, InnerException)
        {            
            if (!string.IsNullOrWhiteSpace(CommandText))
            {
                this.CommandText = CommandText;
                this.Data["CommandText"] = CommandText;
            }
        }


        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            string Result = base.ToString();

            if (!string.IsNullOrWhiteSpace(CommandText))
            {
                StringBuilder SB = new StringBuilder(Result);
                SB.AppendLine(" ");
                SB.AppendLine(CommandText);
                SB.AppendLine(" ");

                Result = SB.ToString();
            }

            return Result;
        }

        /* properties */
        /// <summary>
        /// Gets the command text of this exception
        /// </summary>
        public string CommandText { get; private set; } = "";
    }

}
