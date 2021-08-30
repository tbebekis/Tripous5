using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Tripous.Data;

namespace Test.WinApp
{
    public partial class MainForm : Form
    {
        bool Executing;

        void AnyClick(object sender, EventArgs ea)
        { 
        }


        /* public */
        public void ClearLog()
        {
            edtLog.Clear();
            Application.DoEvents();
        }
        public void Append(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                edtLog.AppendText(Text);
                Application.DoEvents();
            }
        }
        public void AppendLine(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text = string.Empty;

            edtLog.AppendText(Text + Environment.NewLine);
            Application.DoEvents();
        }
        public void AppendLine()
        {
            AppendLine("-------------------------------------------------------------------");
        }
        public void AppendLineEmpty()
        {
            AppendLine(string.Empty);
        }
        public void Log(string Text = null)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                ClearLog();
            }
            else
            {
                AppendLine(Text);
            }
        }


        void Execute(Action Proc)
        {
            if (Executing)
            {
                AppendLine("Cannot Execute(). Already executing!");
            }

            Executing = true;
            try
            {
                Proc();
            }
            catch (Exception e)
            {
                AppendLine(e.ToString());
            }
            finally
            {
                Executing = false;
            }
        }

        void FormInitialize()
        {
            App.Initialize(this);

            SettingTest.LoadSettings();

            //Test();
            //BrokerTest.Test1();
        }
        void Test()
        {
            DataTable Table = new DataTable();
            Table.Columns.Add("Code");
            Table.Columns.Add("Name");

            Table.Rows.Add("000", "Teo");

            CodeProviderDef Def = new CodeProviderDef();
            Def.Name = "Def";
            Def.Text = "PO|select max(NumberField) from @TABLE_NAME;XXX-XXX";

            CodeProvider CP = new CodeProvider() { Descriptor = Def, TableName = "Customer" };
            //string Result = CP.Execute(Table.Rows[0], null, null);
        }
 
        /* overrides */
        protected override void OnShown(EventArgs e)
        {
            if (!DesignMode)
                FormInitialize();

            base.OnShown(e);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (Executing)
            {
                e.Cancel = true;
                AppendLine("Can NOT close. Please STOP executing first." + Environment.NewLine);
            }

        }

        /* construction */

        /// <summary>
        /// Construction
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

    }
}
