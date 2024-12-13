namespace WinApp.Demos
{
    public partial class MainForm : Form
    {
        DataTable tblDemos;
        BindingSource bs;

        void AnyClick(object sender, EventArgs ea)
        {

        }
        void Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        void Grid_DoubleClick(object sender, EventArgs e)
        {
            ShowDemo();
        }

        void FormInitialize()
        {
            CollectDemos();
        }
        void CollectDemos()
        {
            tblDemos = new DataTable();
            tblDemos.Columns.Add("Title");
            tblDemos.Columns.Add("Description");
            tblDemos.Columns.Add("TYPE", typeof(object));

            bs = new BindingSource();
            bs.DataSource = tblDemos;

            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
 
            Grid.DataSource = bs;
            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            Grid.DataError += Grid_DataError;
            Grid.DoubleClick += Grid_DoubleClick; 

            List<Type> DemoTypeList = TypeFinder.FindImplementorClasses(typeof(IDemo), this.GetType().Assembly);
            IDemo Demo;
            DataRow Row;
            foreach (Type T in DemoTypeList)
            {
                Demo = T.Create() as IDemo;

                Row = tblDemos.NewRow();
                tblDemos.Rows.Add(Row);

                Row["Title"] = Demo.Title;
                Row["Description"] = Demo.Description;
                Row["TYPE"] = T;
            }
        }

        void ShowDemo()
        {
            Type T = null;

            var GridRow = Grid.CurrentRow;
            if (GridRow != null)
            {
                var RowView = GridRow.DataBoundItem as DataRowView;
                var Row = RowView.Row;
                if (Row != null)
                {
                    T = Row["TYPE"] as Type;
                }
            }

            if (T != null)
            {
                IDemo Demo = T.Create() as IDemo;

                if (Demo.Singleton)
                {
                    foreach (TabPage P in Pager.TabPages)
                    {
                        if ((P.Tag is IDemo) && ((P.Tag as IDemo).Title == Demo.Title))
                            return;
                    }
                }

                TabPage Page = new TabPage();
                Pager.TabPages.Add(Page);
                Demo.ShowUi(Page);
                Pager.SelectedTab = Page;
            }


        }


        protected override void OnShown(EventArgs e)
        {
            if (!DesignMode)
                FormInitialize();

            base.OnShown(e);
        }

 

        public MainForm()
        {
            InitializeComponent();
        }

    }
}
