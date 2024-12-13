namespace Tripous.Forms
{
    /// <summary>
    /// Connection string dialog
    /// </summary>
    public partial class ConnectionStringDialog : Form
    {
        SqlConnectionInfo ConInfo;

        void AnyClick(object sender, EventArgs ea)
        {
            if (btnOK == sender)
            {
                PassResultBack();
            }
        }

        void PassResultBack()
        {
            if (!string.IsNullOrWhiteSpace(edtConnectionString.Text))
            {
                ConInfo.ConnectionString = edtConnectionString.Text;
                this.DialogResult = DialogResult.OK;
            }                
        }
        void FormInitialize()
        {
            if (Ui.MainForm != null)
            {
                this.Owner = Ui.MainForm;
                this.Icon = Ui.MainForm.Icon;
            }
            btnOK.Click += AnyClick;

            this.ActiveControl = edtConnectionString;
        }

        /// <summary>
        /// Override
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            FormInitialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionStringDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displays the dialog
        /// </summary>
        static public bool ShowDialog(SqlConnectionInfo ConInfo)
        {
            using (var F = new ConnectionStringDialog())
            {
                F.ConInfo = ConInfo;
                F.edtConnectionString.Text = ConInfo.ConnectionString;
                return F.ShowDialog() == DialogResult.OK; 
            }
 
        }
    }
}
