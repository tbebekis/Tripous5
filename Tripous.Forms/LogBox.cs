namespace Tripous.Forms
{

    /// <summary>
    /// Represents a thread-safe log edit box.
    /// <para>To be used by Form projects in order to display log entries in a TextBox or RichTextBox.</para>
    /// </summary>
    static public class LogBox
    {
        const string SLine = "-------------------------------------------------------------------";

        class LogBoxLogListener: LogListener
        {
            /// <summary>
            /// Called by the Logger to pass <see cref="LogEntry"/> to a log listener.
            ///<para>
            /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
            /// Thus Listeners should synchronize the ProcessLog() call. Controls need to check if InvokeRequired.
            /// </para>
            /// </summary>
            public override void ProcessLog(LogEntry Entry)
            {
                AppendLine(Entry.Text);
            }
        }


        static SynchronizationContext fSyncContext = AsyncOperationManager.SynchronizationContext; 
        static TextBoxBase Box;
        static LogBoxLogListener logListener;


        /* private */
        static void DoClear(object Fake)
        {
            if (Box != null)
            {
                Box.Clear();
                Application.DoEvents();
            }
        }
        static void DoLog(string Text)
        {
            if (Box != null)
            {
                Box.AppendText(Text);
                if (Box.Text.Length > 0)
                {
                    Box.SelectionStart = Box.Text.Length;
                    Box.ScrollToCaret();
                }
                Application.DoEvents();
            }
        }

        /* public */
        /// <summary>
        /// Initializes this class.
        /// </summary>
        static public void Initialize(TextBoxBase Box, bool UseLogListenerToo = true)
        {
            if (LogBox.Box == null)
            {
                LogBox.Box = Box;
                if (UseLogListenerToo)
                    logListener = new LogBoxLogListener();
            }
            
        }

        /// <summary>
        /// Clears the box
        /// </summary>
        static public void Clear()
        {
            fSyncContext.Post(o => DoClear(null), null);
        }
        /// <summary>
        /// Appends text in the box, in the last existing text line, if any.
        /// </summary>
        static public void Append(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
                fSyncContext.Post(o => DoLog(o as string), Text);
        }
        /// <summary>
        /// Appends a new text line in the box.
        /// </summary>
        static public void AppendLine(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text = Environment.NewLine;
            else if (Text == SLine)
                Text = Environment.NewLine + Text;
            else if (Text != SLine)
                Text = $"{Environment.NewLine}[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {Text} ";  
            fSyncContext.Post(o => DoLog(o as string), Text);
        }
        /// <summary>
        /// Appends a new empty text line in the box.
        /// </summary>
        static public void AppendLineEmpty()
        {
            AppendLine(string.Empty);
        }
        /// <summary>
        /// Appends a new text line in the box.
        /// </summary>
        static public void AppendLine(Exception Ex)
        {
            AppendLine(Ex.ToString());
        }
        /// <summary>
        /// Appends a line in the box, i.e. a <c>-------------------------------------------------------------------</c>
        /// </summary>
        static public void AppendLine()
        {
            AppendLine(SLine);
        }
 
    }
}
