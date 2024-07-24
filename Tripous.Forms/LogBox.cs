using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Tripous.Forms
{

    /// <summary>
    /// Represents a thread-safe log edit box.
    /// <para>To be used by Form projects in order to display log entries in a TextBox or RichTextBox.</para>
    /// </summary>
    static public class LogBox
    {
        static SynchronizationContext fSyncContext = AsyncOperationManager.SynchronizationContext; 
        static TextBoxBase Box;
 
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
                Application.DoEvents();
            }
        }

        /* public */
        /// <summary>
        /// Initializes this class.
        /// </summary>
        static public void Initialize(TextBoxBase Box)
        {
            LogBox.Box = Box;
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
        static public void AppendLine(string Text = "")
        {
            Text = string.IsNullOrWhiteSpace(Text)? Environment.NewLine: Text + Environment.NewLine;
            fSyncContext.Post(o => DoLog(o as string), Text);
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
            string Text = "-------------------------------------------------------------------";
            fSyncContext.Post(o => DoLog(o as string), Text);
        }
 
    }
}
