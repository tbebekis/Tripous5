namespace Tripous.Forms
{
    /// <summary>
    /// A top most form used to display information to the user
    /// while processing a lengthy operation.
    /// </summary>
    public partial class WaitDialog : Form
    {
        private delegate void AddTextEventHandler(string Value);

        static private object syncLock = new LockObject();
        static private WaitDialog instance;

        /* private */
        private int closeCounter = 0;
        private string lastValue = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public WaitDialog()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                Ui.Waiting = true;
                Timer.Interval = 150;
                Timer.Enabled = true;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            lock (syncLock)
            {
                try
                {
                    if (instance != null)
                    {
                        Timer.Enabled = false;
                        Application.DoEvents();
                        Timer.Enabled = true;
                    }
                }
                catch
                {
                }
            }
        }
        void AddText(string Value)
        {
            lock (syncLock)
            {
                try
                {
                    if (instance != null)
                    {
                        if (Memo.InvokeRequired)
                            Memo.Invoke(new AddTextEventHandler(AddText), Value);
                        else
                        {

                            if ((Value != null) && (Value == Wait.ForceCloseSign))
                            {
                                this.Close();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(Value))
                                    closeCounter--;
                                else if (Value != lastValue)
                                {
                                    lastValue = Value;

                                    closeCounter++;
                                    Memo.Text = Value + Environment.NewLine + Memo.Text;
                                    Wait.LastText = Memo.Text;
                                }

                                if (closeCounter <= 0)
                                    this.Close();
                            }

                            Application.DoEvents();

                        }
                    }
                }
                catch
                {
                }
            }

        }

        /* overrides */
        /// <summary>
        /// Raises the System.Windows.Forms.Form.Closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            Wait.LastText = string.Empty;
            Ui.Waiting = false;
            Timer.Enabled = false;
            base.OnClosed(e);
            instance = null;
        }


        /// <summary>
        /// Shows the dialog
        /// </summary> 
        static public void Show(string Value)
        {
            if (Ui.MainForm != null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new WaitDialog();
                        instance.Show();
                    }

                    instance.AddText(Value);
                    Application.DoEvents();
                }
            }
 
        }
    }
}
