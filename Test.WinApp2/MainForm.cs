
namespace Test.WinApp2
{
    public partial class MainForm : Form
    {

        AppSettings Settings;

        void FormInitialize()
        {
            LogBox.Initialize(edtLog);
            
            Settings = new();


            btnTest1.Click += (s, e) => Test1();
            btnTest2.Click += (s, e) => Test2();

            string JsonText = Json.Serialize(Settings, true);
            LogBox.AppendLine(JsonText);
        }

        void Test1()
        {
            Settings.LastProject = "Sci-Fi";
            Settings.Save();

            AppSettings Settings2 = new();
            Settings2.Part.Name += "kai den les tipote";
            Settings2.List.Add("vale na xei");
            string JsonText = Json.Serialize(Settings2, true);
            LogBox.AppendLine(JsonText);
        }
        void Test2()
        {
            List<Part> Parts = new();

            Parts.Add(new Part() { Name = "Part1" });
            Parts.Add(new Part() { Name = "Part2" });
            Parts.Add(new Part() { Name = "Part3" });

            string JsonText = Json.Serialize(Parts, true);

            List<Part> Parts2 = new();
            Json.PopulateObject(Parts2, JsonText);

            JsonText = Json.Serialize(Parts2, true);
            LogBox.AppendLine(JsonText);
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
