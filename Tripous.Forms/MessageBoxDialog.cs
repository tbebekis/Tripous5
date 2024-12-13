namespace Tripous.Forms
{


    /// <summary>
    /// A message dialog box
    /// </summary>
    public partial class MessageBoxDialog : Form
    {
        string memoText;
        MessageBoxType boxType;
        string memoFonts;



        /// <summary>
        /// Initializes the dialog box
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (Ui.MainForm != null)
            {
                this.Owner = Ui.MainForm;
                this.Icon = Ui.MainForm.Icon;
            }

            int ERROR_ICO = 0;
            //int EXCLAMATION_ICO = 1;
            int INFORMATION_ICO = 2;
            int QUESTION_ICO = 3;

            this.MinimumSize = new System.Drawing.Size(360, 220);
            string Title = "";

            switch (boxType)
            {
                case MessageBoxType.Info:
                    Title = "Information";
                    picBox.Image = imageList.Images[INFORMATION_ICO];
                    break;
                case MessageBoxType.Error:
                    Title = "Error";
                    picBox.Image = imageList.Images[ERROR_ICO];
                    break;

                case MessageBoxType.YesNo:                                      // Yes | No
                    Title = "Question";
                    picBox.Image = imageList.Images[QUESTION_ICO];
                    break;
                case MessageBoxType.YesNoCancel:                                // Yes | No | Cancel 
                    Title = "Question";
                    picBox.Image = imageList.Images[QUESTION_ICO];
                    break;
            }


            this.Text = string.Format(" {0} - [{1}]", Application.ProductName, Title);
            edtTitle.Text = Title;


            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Text = "Cancel";
            this.CancelButton = btnCancel;

            btnNo.DialogResult = DialogResult.No;
            btnNo.Text = "No";

            btnYes.DialogResult = DialogResult.Yes;
            btnYes.Text = "Yes";
            this.AcceptButton = btnYes;

            switch (boxType)
            {
                case MessageBoxType.Info:
                case MessageBoxType.Error:                          //  OK                         
                    btnYes.Left = btnNo.Left;
                    btnYes.DialogResult = DialogResult.OK;
                    btnYes.Text = "OK";
                    break;
                case MessageBoxType.YesNo:                          // Yes | No
                    btnYes.Left = btnNo.Left;
                    btnNo.Left = btnCancel.Left;
                    this.CancelButton = btnNo;
                    break;
                case MessageBoxType.YesNoCancel:                    // Yes | No | Cancel                  
                    break;
            }


            //btnNo          .Visible = CSet.In(Type, MsgType.Question | MsgType.YesNoCancel);
            btnNo.Visible = ((int)boxType & (int)(MessageBoxType.YesNo | MessageBoxType.YesNoCancel)) == (int)boxType;
            btnCancel.Visible = (boxType != MessageBoxType.YesNo);

            this.ActiveControl = btnYes;

            if (memoFonts == "Courier New")
                Memo.Font = new System.Drawing.Font(memoFonts, 9);

            Memo.Text = memoText;

            this.SizeGripStyle = SizeGripStyle.Show;
        }



        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageBoxDialog()
        {
            InitializeComponent();
        }






        /* public */
        /// <summary>
        /// Displays a message dialog box according to Type
        /// </summary>
        static public DialogResult Show(string Text, MessageBoxType Type, string MemoFonts)
        {
            try
            {
                using (MessageBoxDialog F = new MessageBoxDialog())
                {
                    F.memoText = Text;
                    F.boxType = Type;
                    F.memoFonts = MemoFonts;

                    return F.ShowDialog();
                }
            }
            finally
            {
                Application.DoEvents();
            }
        }







    }
}
