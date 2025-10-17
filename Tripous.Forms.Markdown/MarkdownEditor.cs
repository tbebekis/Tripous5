#pragma warning disable WFO1000 // Missing code serialization configuration for property content

using System;
using System.Drawing;
using System.Windows.Forms;
using Tripous.Forms.Properties;

namespace Tripous.Forms
{
    public partial class MarkdownEditor : UserControl
    {
        public static readonly Font DefaultEditorFont = new Font("Consolas", 10.85f, FontStyle.Regular, GraphicsUnit.Point, 0);

        // ● private        
        MarkdownWebView WV;
        FctbSyncScrollAdapter fctbAdapter;
        WebViewSyncScrollAdapter webAdapter;
        ScrollSynchronizer ScrollSync;

        // --- Find/Replace panel
        private MarkdownFindReplacePanel _findPanel;
        private bool _darkMode = true; // sync με highlighter theme

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
            Highlighter.ApplyDarkTheme();
            _darkMode = true;

            // Render → Preview (debounced μέσα στο WebView)
            MarkdownBox.TextChanged += (s, e) =>
            {
                MarkdownTextChanged();
            };

            // Προαιρετικά: φτιάξε το Find panel από την αρχή αλλά κρυφό
            EnsureFindPanel();
            _findPanel.Visible = false;

            // Ο host form να δίνει keyboard events εδώ
            var form = this.FindForm();
            if (form != null) 
                form.KeyPreview = true;


            fctbAdapter   = new FctbSyncScrollAdapter(MarkdownBox);
            webAdapter = new WebViewSyncScrollAdapter(WV);

            ScrollSync = new ScrollSynchronizer(fctbAdapter, webAdapter, debounceMs: 15);
            ScrollSync.Start();


            this.Disposed += (s, e) => 
            {
                ScrollSync.Dispose();
                fctbAdapter.Dispose();
                webAdapter.Dispose();
            };
        }

        private ToolStripButton CreateButton(string text, Image img, EventHandler onClick, string tooltip)
        {
            var b = new ToolStripButton(text, img, onClick);
            b.DisplayStyle = ToolStripItemDisplayStyle.Image;
            b.ToolTipText = tooltip;
            return b;
        }

        // --- δημιουργεί/επιστρέφει το panel και το “δένει” στο Panel1
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

        // --- API για άνοιγμα Find/Replace από toolbar/μενού
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

        // --- theme sync
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

        // --- shortcuts μέσα από το ίδιο UserControl
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
