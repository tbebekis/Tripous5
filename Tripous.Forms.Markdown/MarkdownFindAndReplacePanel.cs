#nullable enable
namespace Tripous.Forms
{
    /// <summary>
    /// Find & Replace panel for FastColoredTextBox (no extra styles, drop-in).
    /// Create once on demand; re-attach to editors with AttachTo().
    /// </summary>
    public sealed class MarkdownFindReplacePanel : Panel
    {
        private FastColoredTextBox _tb;

        // UI
        private readonly TextBox _txtFind = new() { BorderStyle = BorderStyle.FixedSingle, Width = 260, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
        private readonly TextBox _txtReplace = new() { BorderStyle = BorderStyle.FixedSingle, Width = 260, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };

        private readonly CheckBox _chkCase = new() { Text = "Aa", AutoSize = true };
        private readonly CheckBox _chkWhole = new() { Text = "Whole", AutoSize = true };
        private readonly CheckBox _chkRegex = new() { Text = "Regex", AutoSize = true };
        private readonly CheckBox _chkSelOnly = new() { Text = "In selection", AutoSize = true };
        private readonly CheckBox _chkBackward = new() { Text = "Backward", AutoSize = true };
        private readonly CheckBox _chkWrap = new() { Text = "Wrap", AutoSize = true, Checked = true };

        private readonly Button _btnFind = new() { Text = "&Find", AutoSize = true, UseMnemonic = true };
        private readonly Button _btnReplace = new() { Text = "&Replace", AutoSize = true, UseMnemonic = true };
        private readonly Button _btnReplaceAll = new() { Text = "Replace &All", AutoSize = true, UseMnemonic = true };
        private readonly Button _btnClose = new() { Text = "&Close", AutoSize = true, UseMnemonic = true };

        private readonly Label _lblCount = new() { Text = "0", AutoSize = true };

        private readonly System.Windows.Forms.Timer _debounceTimer = new() { Interval = 140 };
        private readonly ToolTip _tips = new() { AutomaticDelay = 250, ReshowDelay = 100, InitialDelay = 250, AutoPopDelay = 8000, ShowAlways = true };

        public MarkdownFindReplacePanel(FastColoredTextBox tb)
        {
            _tb = tb ?? throw new ArgumentNullException(nameof(tb));

            // Panel styling
            Height = 74;
            Padding = new Padding(6);
            BackColor = SystemColors.ControlLightLight;
            BorderStyle = BorderStyle.FixedSingle;
            DoubleBuffered = true;

            // Layout rows
            var row1 = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 32, WrapContents = false, AutoSize = false };
            var row2 = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 32, WrapContents = false, AutoSize = false };

            row1.Controls.Add(new Label { Text = "Find:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            row1.Controls.Add(_txtFind);
            row1.Controls.Add(_chkCase);
            row1.Controls.Add(_chkWhole);
            row1.Controls.Add(_chkRegex);
            row1.Controls.Add(_chkSelOnly);
            row1.Controls.Add(_chkBackward);
            row1.Controls.Add(_chkWrap);
            row1.Controls.Add(new Label { Text = "Count:", AutoSize = true, Padding = new Padding(12, 6, 0, 0) });
            row1.Controls.Add(_lblCount);
            row1.Controls.Add(_btnClose);

            row2.Controls.Add(new Label { Text = "Replace:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            row2.Controls.Add(_txtReplace);
            row2.Controls.Add(_btnFind);
            row2.Controls.Add(_btnReplace);
            row2.Controls.Add(_btnReplaceAll);

            Controls.Add(row2);
            Controls.Add(row1);

            // Tooltips
            _tips.SetToolTip(_txtFind, "Find text (Enter = Find Next, Shift+Enter = Find Previous)");
            _tips.SetToolTip(_txtReplace, "Replace with (Ctrl+Enter = Replace All)");
            _tips.SetToolTip(_chkCase, "Match case (Aa)");
            _tips.SetToolTip(_chkWhole, "Whole word only");
            _tips.SetToolTip(_chkRegex, "Use .NET Regular Expressions");
            _tips.SetToolTip(_chkSelOnly, "Search only in current selection");
            _tips.SetToolTip(_chkBackward, "Search backward");
            _tips.SetToolTip(_chkWrap, "Wrap search at document edges");
            _tips.SetToolTip(_btnFind, "Find Next (Enter)");
            _tips.SetToolTip(_btnReplace, "Replace current and go to next");
            _tips.SetToolTip(_btnReplaceAll, "Replace all matches (Ctrl+Enter)");
            _tips.SetToolTip(_btnClose, "Close (Esc)");

            // Events
            _btnClose.Click += (_, __) => ClosePanel();
            _btnFind.Click += (_, __) => DoFindNext();
            _btnReplace.Click += (_, __) => DoReplaceCurrent();
            _btnReplaceAll.Click += (_, __) => DoReplaceAll();

            _txtFind.TextChanged += (_, __) => DebounceHighlight();
            _txtFind.KeyDown += FindBox_KeyDown;
            _txtReplace.KeyDown += ReplaceBox_KeyDown;

            _chkCase.CheckedChanged += (_, __) => DebounceHighlight();
            _chkWhole.CheckedChanged += (_, __) => DebounceHighlight();
            _chkRegex.CheckedChanged += (_, __) => DebounceHighlight();
            _chkSelOnly.CheckedChanged += (_, __) => DebounceHighlight();

            _debounceTimer.Tick += (_, __) =>
            {
                _debounceTimer.Stop();
                UpdateHighlights();
            };

            // ESC closes
            AttachEscClose(this);

            // Initial highlight
            UpdateHighlights();
        }

        // -------- Public API --------

        public void AttachTo(FastColoredTextBox tb)
        {
            _tb = tb ?? throw new ArgumentNullException(nameof(tb));
            UpdateHighlights();
            FocusFind();
        }

        public void FocusFind()
        {
            _txtFind.Focus();
            _txtFind.SelectAll();
        }

        public void FocusReplace()
        {
            _txtReplace.Focus();
            _txtReplace.SelectAll();
        }

        public enum PanelTheme { Light, Dark }

        public void ApplyTheme(PanelTheme theme)
        {
            if (theme == PanelTheme.Dark)
            {
                BackColor = Color.FromArgb(35, 38, 45);
                ForeColor = Color.FromArgb(230, 230, 235);
                SetRowColors(this, BackColor, ForeColor);
                // Inputs
                StyleInput(_txtFind, dark: true);
                StyleInput(_txtReplace, dark: true);
            }
            else
            {
                BackColor = SystemColors.ControlLightLight;
                ForeColor = SystemColors.ControlText;
                SetRowColors(this, BackColor, ForeColor);
                StyleInput(_txtFind, dark: false);
                StyleInput(_txtReplace, dark: false);
            }
            Invalidate(true);
        }

        /// <summary>Καθαρίζει τα search highlights (χρήσιμο όταν κρύβεται το panel).</summary>
        public void ClearHighlights()
        {
            var opt = BuildOptions();
            FastFindReplace.HighlightAll(_tb, "", opt);
        }

        // -------- Internals --------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _debounceTimer?.Dispose();
                _tips?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AttachEscClose(Control root)
        {
            root.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    ClosePanel();
                    e.Handled = true;
                }
                else if (e.Control && e.KeyCode == Keys.H)
                {
                    FocusReplace();
                    e.Handled = true;
                }
            };

            // propagate to children too
            foreach (Control c in root.Controls)
                AttachEscClose(c);
        }

        private void StyleInput(TextBox tb, bool dark)
        {
            if (dark)
            {
                tb.BackColor = Color.FromArgb(48, 52, 60);
                tb.ForeColor = Color.White;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                tb.BackColor = Color.White;
                tb.ForeColor = Color.Black;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void SetRowColors(Control root, Color back, Color fore)
        {
            foreach (Control c in root.Controls)
            {
                c.BackColor = back;
                c.ForeColor = fore;
                if (c.HasChildren) SetRowColors(c, back, fore);
            }
        }

        private void FindBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                DoFindNext();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && e.Shift)
            {
                DoFindPrev();
                e.Handled = true;
            }
        }

        private void ReplaceBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                DoReplaceAll();
                e.Handled = true;
            }
        }

        private void DebounceHighlight()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void UpdateHighlights()
        {
            var count = FastFindReplace.HighlightAll(_tb, _txtFind.Text, BuildOptions());
            _lblCount.Text = count.ToString();
        }

        private FastFindReplace.FindOptions BuildOptions(bool? forwardOverride = null)
        {
            return new FastFindReplace.FindOptions
            {
                MatchCase = _chkCase.Checked,
                WholeWord = _chkWhole.Checked,
                UseRegex = _chkRegex.Checked,
                SearchSelectionOnly = _chkSelOnly.Checked,
                Wrap = _chkWrap.Checked,
                Forward = forwardOverride ?? !_chkBackward.Checked
            };
        }

        private FastColoredTextBoxNS.Range FastFindReplaceHighlightScope()
        {
            return _chkSelOnly.Checked && !_tb.Selection.IsEmpty ? _tb.Selection.Clone() : _tb.Range;
        }

        private void DoFindNext()
        {
            var ok = FastFindReplace.FindNext(_tb, _txtFind.Text, BuildOptions(true));
            if (!ok && !_chkWrap.Checked) System.Media.SystemSounds.Beep.Play();
        }

        private void DoFindPrev()
        {
            var ok = FastFindReplace.FindNext(_tb, _txtFind.Text, BuildOptions(false));
            if (!ok && !_chkWrap.Checked) System.Media.SystemSounds.Beep.Play();
        }

        private void DoReplaceCurrent()
        {
            var replaced = FastFindReplace.ReplaceCurrent(_tb, _txtFind.Text, _txtReplace.Text, BuildOptions());
            if (!replaced)
            {
                DoFindNext();
                replaced = FastFindReplace.ReplaceCurrent(_tb, _txtFind.Text, _txtReplace.Text, BuildOptions());
            }
            if (replaced)
            {
                DoFindNext();
                UpdateHighlights();
            }
        }

        private void DoReplaceAll()
        {
            var count = FastFindReplace.ReplaceAll(_tb, _txtFind.Text, _txtReplace.Text, BuildOptions());
            UpdateHighlights();
            _tb.Focus();

            var tt = new ToolTip { AutomaticDelay = 100, AutoPopDelay = 1500, ReshowDelay = 50, ShowAlways = true };
            tt.Show($"Replaced {count} occurrence(s).", this, Width - 180, Height - 30, 1500);
        }

        private void ClosePanel()
        {
            ClearHighlights();
            Visible = false;
            _tb?.Focus();
        }

        // μέσα στην κλάση MarkdownFindReplacePanel
        public void FindNextShortcut() => DoFindNext();
        public void FindPrevShortcut() => DoFindPrev();
 

    }
}
