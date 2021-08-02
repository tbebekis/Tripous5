using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.WinApp
{
    public partial class MainForm : Form
    {
        void AnyClick(object sender, EventArgs ea)
        {
            if (btnParseSql == sender)
            {
                ParseSql();
            }
        }
        void FormInitialize()
        {
            btnParseSql.Click += AnyClick;
        }
        void ParseSql()
        {
            string SqlText = edtSql.Text;
            SqlParserHelper.Parse(SqlText);
        }






        protected override void OnShown(EventArgs e)
        {
            if (!DesignMode)
                FormInitialize();
            base.OnShown(e);
        }

        /// <summary>
        /// Construction
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

    }
}
