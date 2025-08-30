using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 

using Tripous;
using Tripous.Data;
using Tripous.Forms;

namespace Test.WinApp
{
    public partial class MainForm : Form
    {
        bool Executing;

        void AnyClick(object sender, EventArgs ea)
        { 
        }


        /* private */        
        void FormInitialize()
        {
            LogBox.Initialize(edtLog);
            App.Initialize(this);

            /*
            SettingTest.LoadSettings();
            BrokerTest.TestJoinTables();
            TestStringList();
            */
        }
        void Execute(Action Proc)
        {
            if (Executing)
            {
                LogBox.AppendLine("Cannot Execute(). Already executing!");
            }

            Executing = true;
            try
            {
                Proc();
            }
            catch (Exception e)
            {
                LogBox.AppendLine(e);
            }
            finally
            {
                Executing = false;
            }
        }

        void CodeProviderTest()
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
        void TestStringList()
        {
            List<string> InsertList = new List<string>();
            InsertList.Add("Id");
            InsertList.Add("Name");
            InsertList.Add("CountryId");

            string S = string.Join(", " + Environment.NewLine, InsertList.ToArray());
            LogBox.Clear();
            LogBox.AppendLine(S);
 
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
                LogBox.AppendLine("Can NOT close. Please STOP executing first." + Environment.NewLine);
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

    public class LogEntry
    {
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    }
}
