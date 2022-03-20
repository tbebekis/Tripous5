using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tripous.Forms
{
    /// <summary>
    /// Displays a Please, wait... dialog
    /// </summary>
    static public class Wait
    {
        static private string lastText;
        /// <summary>
        /// Constant.
        /// </summary>
        public const string ForceCloseSign = "###";

        static private void Sys_UnhandledExceptionThrown(object sender, Exception e)
        {
            ForceClose();
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static Wait()
        {
            Ui.UnhandledExceptionThrown += Sys_UnhandledExceptionThrown;
        }

        /* methods */
        /// <summary>
        /// Displays the wait dialog box, passing it the Value string .
        /// </summary>
        static public void Show(string Value)
        {
            if (Environment.UserInteractive && Ui.InMainThread)
            {
                WaitDialog.Show(Value);
            }
        }
        /// <summary>
        /// Displays the wait box and freezes the application for WaitMSecs.
        /// <para>WARNING: The WaitBox will remain visible.</para>
        /// </summary>
        static public void Show(string Value, int WaitMSecs)
        {
            if (Environment.UserInteractive && Ui.InMainThread)
            {
                Show(Value);
                while (WaitMSecs > 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    WaitMSecs -= 100;
                }
            }
        }
        /// <summary>
        /// Displays a wait box while loading a form
        /// </summary>
        static public void LoadingForm()
        {
            Show("Loading form");
        }
        /// <summary>
        /// Displays a wait box while loading data
        /// </summary>
        static public void LoadingData()
        {
            Show("Loading data");
        }
        /// <summary>
        /// Displays a wait box while executing a SELECT 
        /// </summary>
        static public void SelectSql()
        {
            Show("Executing a SELECT statement...");
        }
        /// <summary>
        /// Displays a wait box while executing an INSERT, UPDATE or DELETE statement 
        /// </summary>
        static public void ExecSql()
        {
            Show("Executing a SQL statement...");
        }
        /// <summary>
        /// Shows the wait box
        /// </summary>
        static public void Executing()
        {
            Show("Executing");
        }
        /// <summary>
        /// Shows the wait box
        /// </summary>
        static public void Loading()
        {
            Show("Loading");
        }
        /// <summary>
        /// Shows the wait box
        /// </summary>
        static public void Saving()
        {
            Show("Saving");
        }
        /// <summary>
        /// Displays a wait box while executing a SELECT 
        /// </summary>
        static public void Commiting()
        {
            Show("Commiting");
        }

        /// <summary>
        /// Displays the wait box and freezes the application for WaitMSecs.
        /// <para>Then it instructs the wait box to decrease its internal close counter.</para>
        /// </summary>
        static public void Close(string Value, int WaitMSecs)
        {
            Show(Value, WaitMSecs);
            Close();
        }
        /// <summary>
        /// Displays the wait box and freezes the application for 1500 MSecs.
        /// <para>Then it instructs the wait box to decrease its internal close counter.</para>
        /// </summary>
        static public void Close(string Value)
        {
            Close(Value, 1500);
        }
        /// <summary>
        /// Instructs the wait box to decrease its internal close counter.
        /// </summary>
        static public void Close()
        {
            Show(string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        static public void ForceClose()
        {
            Show(ForceCloseSign);
        }

        /* properties */
        /// <summary>
        /// Gets or sets the text lastly displayed by the wait form.
        /// </summary>
        static public string LastText
        {
            get { return !string.IsNullOrEmpty(lastText) ? lastText : string.Empty; }
            set { lastText = value; }
        }

    }
}
