#pragma warning disable WFO1000 // Missing code serialization configuration for property content

using Google.Protobuf.Reflection;
using System.Windows.Forms;
using Tripous.Forms.Properties;

namespace Tripous.Forms
{
    public partial class MarkdownEditor : UserControl
    {
        static public readonly Font DefaultEditorFont = new Font("Consolas", 10.85f, FontStyle.Regular, GraphicsUnit.Point, 0);

        // ● private        
        MarkdownWebView WV;

        // --- NEW: Find/Replace panel πεδίο
        private MarkdownFindReplacePanel _findPanel;
        private bool _darkMode = true; // κράτα sync με το highlighter theme που εφαρμόζεις

        // --- SCROLL SYNC: guards + settings
        bool _syncEnabled = true;
        bool _internalEditorScroll = false;
        DateTime _lastEditorSent = DateTime.MinValue;

        // ● private      
        void ControlInitialize()
        {
            ToolBar.Items.AddRange(new ToolStripItem[]{
                CreateButton("Bold", Resources.bold, (s,e)=> MarkdownBox.ToggleBold(), "Bold (Ctrl + B)"),
                CreateButton("Italic", Resources.italic, (s,e)=> MarkdownBox.ToggleItalic(), "Italic (Ctrl + I)"),
                new ToolStripSeparator(),
                CreateButton("Code", Resources.code, (s,e)=> MarkdownBox.ToggleInlineCode(), "Inline Code"),
                CreateButton("Link", Resources.link, (s,e)=> MarkdownBox.InsertLink("", ""),"Insert Link"),
                CreateButton("Img", Resources.image, (s,e)=> MarkdownBox.InsertImage("", ""),"Insert Image"),
                new ToolStripSeparator(),
                CreateButton("UL", Resources.list_bullet, (s,e)=> MarkdownBox.ToggleUnorderedList(),"Bullet List"),
                CreateButton("OL", Resources.list_number, (s,e)=> MarkdownBox.ToggleOrderedList(1),"Numbered List"),
                CreateButton("Task", Resources.list_task, (s,e)=> MarkdownBox.ToggleTaskList(),"Task List"),
                CreateButton("Quote", Resources.quote, (s,e)=> MarkdownBox.ToggleBlockquote(),"Blockquote"),
                new ToolStripSeparator(),
                CreateButton("Code Block", Resources.code_block, (s,e)=> MarkdownBox.InsertFencedCode(""),"Fenced Code Block"),
                CreateButton("HR", Resources.hr, (s,e)=> MarkdownBox.InsertHorizontalRule(),"Horizontal Rule"),
                CreateButton("Table", Resources.table, (s,e)=> MarkdownBox.InsertTable(3,2,true),"Insert Table (Ctrl + T)"),
                new ToolStripSeparator(),
                CreateButton("Reformat Table", Resources.table_reformat, (s,e)=> MarkdownBox.ReformatTableAtCaret(),"Reformat Table (Ctrl + Shift + T)"),
                CreateButton("Index", Resources.index, (s,e)=> MarkdownBox.BuildOrUpdateIndex(),"Build Or Update Index (Ctrl + Alt + I)"),
                //
                new ToolStripSeparator(),
                CreateButton("Find", Resources.find, (s,e)=> ShowFindPanel(),"Find / Replace"),
                CreateButton("Theme", Resources.theme, (s,e)=>
                {
                    var next = MarkdownBox.Theme == MarkdownTextBox.MarkdownTheme.Dark
                        ? MarkdownTextBox.MarkdownTheme.Light
                        : MarkdownTextBox.MarkdownTheme.Dark;
                    MarkdownBox.ApplyTheme(next);
                }, "Toggle Light/Dark"),
                CreateButton("Index", Resources.save, (s,e)=> DoSaveText(),"Save (Ctrl + S)"),
            });

            WV = new MarkdownWebView();
            Splitter.Panel2.Controls.Add(WV);
            WV.Dock = DockStyle.Fill;

            // Editor init
            MarkdownBox.WordWrap = true;
            MarkdownBox.Font = DefaultEditorFont;
            MarkdownBox.Language = FastColoredTextBoxNS.Language.Custom;

            var Highlighter = new MarkdownHighlighter(MarkdownBox);
            // Highlighter.ApplyLightTheme();
            Highlighter.ApplyDarkTheme();
            _darkMode = true;

            MarkdownBox.TextChanged += (s, e) => MarkdownTextChanged();

            // --- NEW: optional — αν θες το panel να υπάρχει από την αρχή αλλά κρυφό
            EnsureFindPanel();
            _findPanel.Visible = false;

            MarkdownBox.TextChanged += (s, e) => MarkdownTextChanged();

            // --- NEW: κάνε τον Form host να δίνει τα keyboard events εδώ
            var form = this.FindForm();
            if (form != null) form.KeyPreview = true;

            // --- SCROLL SYNC: wire up δύο κατευθύνσεις
            MarkdownBox.VisibleRangeChanged += Editor_VisibleRangeChanged;
            WV.EnableScrollSync(true);
            WV.ScrollPercentChanged += WebView_ScrollPercentChanged;
        }

        private ToolStripButton CreateButton(string text, Image img, EventHandler onClick, string tooltip)
        {
            var b = new ToolStripButton(text, img, onClick);
            b.DisplayStyle = ToolStripItemDisplayStyle.Image;
            b.ToolTipText = tooltip;
            return b;
        }

        // --- NEW: δημιουργεί/επιστρέφει το panel και το “δένει” στο Panel1
        private void EnsureFindPanel()
        {
            if (_findPanel == null)
            {
                _findPanel = new MarkdownFindReplacePanel(MarkdownBox)
                {
                    Visible = false
                };
                // Theme sync
                _findPanel.ApplyTheme(_darkMode
                    ? MarkdownFindReplacePanel.PanelTheme.Dark
                    : MarkdownFindReplacePanel.PanelTheme.Light);

                MarkdownBox.Parent = null;
                Application.DoEvents();
                Splitter.Panel1.Controls.Add(_findPanel);
                _findPanel.BringToFront();
                _findPanel.Dock = DockStyle.Top;
                Splitter.Panel1.Controls.Add(MarkdownBox);
                MarkdownBox.BringToFront();
            }
            else
            {
                _findPanel.AttachTo(MarkdownBox);
            }
        }

        // --- NEW: API για άνοιγμα Find/Replace από toolbar/μενού
        public void ShowFindPanel()
        {
            EnsureFindPanel();
            _findPanel.Visible = true;
            _findPanel.FocusFind();
        }

        public void ShowReplacePanel()
        {
            EnsureFindPanel();
            _findPanel.Visible = true;
            _findPanel.FocusReplace();
        }

        // --- NEW: αν αλλάζεις theme στο editor, κράτα sync το panel
        public void ApplyEditorTheme(bool dark)
        {
            _darkMode = dark;
            if (_findPanel != null)
            {
                _findPanel.ApplyTheme(dark
                    ? MarkdownFindReplacePanel.PanelTheme.Dark
                    : MarkdownFindReplacePanel.PanelTheme.Light);
            }
        }

        void MarkdownTextChanged()
        {
            string MarkdownText = MarkdownBox.Text;
            WV.MarkdownText = MarkdownText;
        }

        void DoSaveText()
        {
            SaveTextRequested?.Invoke(this, EventArgs.Empty);
        }

        // ● overrides  
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
                ControlInitialize();
        }

        // --- NEW: shortcuts μέσα από το ίδιο UserControl
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                DoSaveText();
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                ShowFindPanel();
                return true;
            }
            if (keyData == (Keys.Control | Keys.H))
            {
                ShowReplacePanel();
                return true;
            }
            if (keyData == Keys.F3)
            {
                EnsureFindPanel();
                _findPanel.Visible = true;
                _findPanel.FindNextShortcut();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.F3))
            {
                EnsureFindPanel();
                _findPanel.Visible = true;
                _findPanel.FindPrevShortcut();
                return true;
            }
            if (keyData == Keys.Escape && _findPanel != null && _findPanel.Visible)
            {
                _findPanel.ClearHighlights();
                _findPanel.Visible = false;
                MarkdownBox.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // -------------------------
        // --- SCROLL SYNC REGION
        // -------------------------

        // Editor -> WebView (όταν αλλάζει το ορατό range)
        void Editor_VisibleRangeChanged(object sender, EventArgs e)
        {
            if (!_syncEnabled || _internalEditorScroll) return;

            // throttle ~50ms
            if ((DateTime.Now - _lastEditorSent).TotalMilliseconds < 50)
                return;
            _lastEditorSent = DateTime.Now;

            double pct = GetEditorScrollPercent();
            _ = WV.ScrollToPercentAsync(pct);
        }

        double GetEditorScrollPercent()
        {
            int total = Math.Max(1, MarkdownBox.LinesCount - 1);
            int firstVisible = Math.Max(0, MarkdownBox.VisibleRange.Start.iLine);
            return total == 0 ? 0.0 : (double)firstVisible / total;
        }

        // WebView -> Editor
        void WebView_ScrollPercentChanged(object sender, double pct)
        {
            if (!_syncEnabled) return;

            int total = Math.Max(1, MarkdownBox.LinesCount - 1);
            int targetLine = (int)Math.Round(Math.Clamp(pct, 0.0, 1.0) * total);
            ScrollEditorToLine(targetLine);
        }

        void ScrollEditorToLine(int targetLine)
        {
            int line = Math.Clamp(targetLine, 0, Math.Max(0, MarkdownBox.LinesCount - 1));

            _internalEditorScroll = true;
            try
            {
                // Γενικός τρόπος που παίζει σε όλες τις εκδόσεις FCTB
                var r = new FastColoredTextBoxNS.Range(MarkdownBox, 0, line, 0, line);
                MarkdownBox.Selection = r;
                MarkdownBox.DoSelectionVisible();
            }
            finally
            {
                // δώσε 30ms να «σβήσουν» τα internal events και έπειτα ξεκλείδωσε
                _ = Task.Delay(30).ContinueWith(_ => _internalEditorScroll = false);
            }
        }

        /// <summary>Enable/disable scroll synchronization και στις δύο πλευρές.</summary>
        public void SetScrollSync(bool enable)
        {
            _syncEnabled = enable;
            WV.EnableScrollSync(enable);
        }

        // -------------------------
        // --- END SCROLL SYNC
        // -------------------------

        // ● construction
        public MarkdownEditor()
        {
            InitializeComponent();
        }

        // ● properties
        public MarkdownTextBox MarkdownBox => fMarkdownBox;
        public MarkdownWebView WebView => WV;
        public bool Modified
        {
            get => MarkdownBox.IsChanged;
            set
            {
                if (MarkdownBox.IsChanged != value)
                    MarkdownBox.IsChanged = value;
            }
        }
        public string MarkdownText
        {
            get => MarkdownBox.Text;
            set => MarkdownBox.Text = value;
        }

        public event EventHandler SaveTextRequested;
    }
}
