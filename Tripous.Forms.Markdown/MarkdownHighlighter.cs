namespace Tripous.Forms
{

    /// <summary>
    /// Compact Markdown highlighter για FastColoredTextBox με ελεγχόμενο πλήθος styles.
    /// Καλύπτει: ATX/Setext headings, emphasis, code (inline/fenced/indented),
    /// links/images, blockquotes, lists, HR, tables. Δουλεύει σε VisibleRange.
    /// </summary>
    public sealed class MarkdownHighlighter : IDisposable
    {
        private readonly FastColoredTextBox _tb;
        private bool _disposed;
        

        // ---- Style pool (shared per-control) ----
        private sealed class Pool
        {
            public TextStyle Heading = null!;
            public TextStyle SetextHeading = null!;
            public TextStyle Bold = null!;
            public TextStyle Italic = null!;
            public TextStyle BoldItalic = null!;
            public TextStyle Code = null!;
            public TextStyle Blockquote = null!;
            public TextStyle ListMarker = null!;
            public TextStyle LinkText = null!;
            public TextStyle LinkUrl = null!;
            public TextStyle HorizontalRule = null!;
            public TextStyle TableSep = null!;
        }

        private const string PoolKey = "__SW_MD_POOL__";
        private const string AttachedKey = "__SW_MD_ATTACHED__";
        private Pool _pool;

        // ---- Regexes (compiled) ----
        private static readonly Regex ReHeadingAtx =
            new(@"^#{1,6}\s+[^\r\n#].*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReHeadingSetext =
            new(@"^(?<t>[^\r\n]+)\r?\n(?<u>={2,}|-{2,})\s*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReBoldItalic =
            new(@"(\*\*\*|___)(?=\S)(.+?)(?<=\S)\1", RegexOptions.Compiled);

        private static readonly Regex ReBold =
            new(@"(\*\*|__)(?=\S)(.+?)(?<=\S)\1", RegexOptions.Compiled);

        private static readonly Regex ReItalic =
            new(@"(?<!\*)\*(?!\*)(?=\S)(.+?)(?<=\S)\*|(?<!_)_(?!_)(?=\S)(.+?)(?<=\S)_", RegexOptions.Compiled);

        private static readonly Regex ReInlineCode =
            new(@"`[^`\r\n]+`", RegexOptions.Compiled);

        private static readonly Regex ReFencedCode =
            new(@"^```[^\r\n]*\r?\n[\s\S]*?\r?\n```[ \t]*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReIndentedCode =
            new(@"^(?: {4}|\t).*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReBlockquote =
            new(@"^\s*>\s.*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReListMarker =
            new(@"^\s*(?:[-+*]|\d+\.)\s+", RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex ReHr =
            new(@"^\s*([-*_])(?:\s*\1){2,}\s*$", RegexOptions.Compiled | RegexOptions.Multiline); // fixed

        private static readonly Regex ReLink =
            new(@"\[(?<text>[^\]]+)\]\((?<url>[^\s\)]+)(?:\s+""[^""]*"")?\)", RegexOptions.Compiled);

        private static readonly Regex ReImage =
            new(@"!\[(?<alt>[^\]]*)\]\((?<url>[^\s\)]+)(?:\s+""[^""]*"")?\)", RegexOptions.Compiled);

        private static readonly Regex ReTableSep =
            new(@"^\s*\|?(?:\s*:?-+:?\s*\|)+\s*:?-+:?\s*\|?\s*$", RegexOptions.Compiled | RegexOptions.Multiline);

        // fenced code: κάνε style μόνο το περιεχόμενο, όχι τις τριπλές backticks
        private static readonly Regex ReFencedCodeContent =
            new(@"^```[^\r\n]*\r?\n(?<code>[\s\S]*?)(?:\r?\n```[ \t]*$|\Z)",
                RegexOptions.Compiled | RegexOptions.Multiline);


        public MarkdownHighlighter(FastColoredTextBox tb)
        {
            _tb = tb ?? throw new ArgumentNullException(nameof(tb));

            // Σιγουρεύουμε Tag ως IDictionary για να κρατάμε state/pool
            if (_tb.Tag is not IDictionary tag)
            {
                tag = new Hashtable();
                _tb.Tag = tag;
            }

            // Ασφάλεια: αν είναι ήδη attached, μην ξαναδέσεις events/ξαναδημιουργήσεις styles
            if (tag.Contains(AttachedKey))
                return;
            tag[AttachedKey] = true;

            // Μηδενίζουμε οτιδήποτε θα πρόσθετε δικά του styles
            _tb.SyntaxHighlighter = null;
            _tb.BracketsStyle = null;

            // Basic editor prefs για markdown
            _tb.Language = FastColoredTextBoxNS.Language.Custom;
            _tb.AutoIndent = true;
            _tb.WordWrap = true;
            _tb.DelayedTextChangedInterval = 120;

            // Δημιουργία ή επαναχρήση pool
            if (tag.Contains(PoolKey) && tag[PoolKey] is Pool existing)
            {
                _pool = existing;
            }
            else
            {
                _pool = CreatePool();
                tag[PoolKey] = _pool;
            }

            // Events
            _tb.TextChangedDelayed += OnTextChangedDelayed;
            _tb.VisibleRangeChanged += OnVisibleRangeChanged;

            // Initial pass
            Highlight(_tb.VisibleRange);
        }

        private static Pool CreatePool()
        {
            // ~12 styles σύνολο (μακριά από το default όριο των 32)
            return new Pool
            {
                Heading = new TextStyle(new SolidBrush(Color.FromArgb(255, 189, 99)), null, FontStyle.Bold),
                SetextHeading = new TextStyle(new SolidBrush(Color.FromArgb(220, 160, 80)), null, FontStyle.Bold),
                Bold = new TextStyle(new SolidBrush(Color.FromArgb(230, 230, 230)), null, FontStyle.Bold),
                Italic = new TextStyle(new SolidBrush(Color.FromArgb(230, 230, 230)), null, FontStyle.Italic),
                BoldItalic = new TextStyle(new SolidBrush(Color.FromArgb(230, 230, 230)), null, FontStyle.Bold | FontStyle.Italic),
                Code = new TextStyle(new SolidBrush(Color.FromArgb(198, 200, 120)), null, FontStyle.Regular),
                Blockquote = new TextStyle(new SolidBrush(Color.FromArgb(120, 170, 220)), null, FontStyle.Italic),
                ListMarker = new TextStyle(new SolidBrush(Color.FromArgb(120, 200, 180)), null, FontStyle.Bold),
                LinkText = new TextStyle(new SolidBrush(Color.FromArgb(120, 180, 240)), null, FontStyle.Underline),
                LinkUrl = new TextStyle(new SolidBrush(Color.FromArgb(140, 200, 255)), null, FontStyle.Regular),
                HorizontalRule = new TextStyle(new SolidBrush(Color.FromArgb(110, 120, 130)), null, FontStyle.Regular),
                TableSep = new TextStyle(new SolidBrush(Color.FromArgb(120, 180, 200)), null, FontStyle.Bold),
            };
        }

        private void OnVisibleRangeChanged(object sender, EventArgs e)
        {
            if (_disposed) return;
            Highlight(_tb.VisibleRange);
        }

        private void OnTextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            if (_disposed) return;
            Highlight(_tb.VisibleRange);
        }

        private void Highlight2(FastColoredTextBoxNS.Range range)
        {
            if (range is null) return;

            // Καθάρισε ΜΟΝΟ τα δικά μας styles (όχι άλλων features)
            range.ClearStyle(
                _pool.Heading, _pool.SetextHeading,
                _pool.BoldItalic, _pool.Bold, _pool.Italic,
                _pool.Code, _pool.Blockquote, _pool.ListMarker,
                _pool.LinkText, _pool.LinkUrl, _pool.HorizontalRule, _pool.TableSep
            );

            // 1) Code blocks πρώτα
            range.SetStyle(_pool.Code, ReFencedCode);
            range.SetStyle(_pool.Code, ReIndentedCode);

            // 2) ATX headings (όλα στο ίδιο style για να μειωθούν τα layers)
            range.SetStyle(_pool.Heading, ReHeadingAtx);

            // 3) Setext headings (βάφουμε μόνο την πρώτη γραμμή)
            foreach (Match m in ReHeadingSetext.Matches(range.Text))
                ApplyGroup(range, m, "t", _pool.SetextHeading);

            // 4) Blockquotes, HR, Lists, Tables
            range.SetStyle(_pool.Blockquote, ReBlockquote);
            range.SetStyle(_pool.HorizontalRule, ReHr);
            range.SetStyle(_pool.ListMarker, ReListMarker);
            range.SetStyle(_pool.TableSep, ReTableSep);

            // 5) Images (βάφονται ως links για οικονομία)
            range.SetStyle(_pool.LinkText, ReImage);

            // 6) Links: text + url
            foreach (Match m in ReLink.Matches(range.Text))
            {
                ApplyGroup(range, m, "text", _pool.LinkText);
                ApplyGroup(range, m, "url", _pool.LinkUrl);
            }

            // 7) Inline code
            range.SetStyle(_pool.Code, ReInlineCode);

            // 8) Emphasis (σειρά έχει σημασία)
            range.SetStyle(_pool.BoldItalic, ReBoldItalic);
            range.SetStyle(_pool.Bold, ReBold);
            range.SetStyle(_pool.Italic, ReItalic);
        }

        private void Highlight(FastColoredTextBoxNS.Range range)
        {
            if (range is null) return;

            // Καθάρισε ΜΟΝΟ τα δικά μας styles στο visible range
            range.ClearStyle(
                _pool.Heading, _pool.SetextHeading,
                _pool.BoldItalic, _pool.Bold, _pool.Italic,
                _pool.Code, _pool.Blockquote, _pool.ListMarker,
                _pool.LinkText, _pool.LinkUrl, _pool.HorizontalRule, _pool.TableSep
            );

            // 1) Fenced code (στυλάρουμε ΜΟΝΟ το περιεχόμενο, και το κάνουμε σε όλο το document)
            var doc = _tb.Range; // ολόκληρο κείμενο
            foreach (Match m in ReFencedCodeContent.Matches(_tb.Text))
                ApplyGroup(doc, m, "code", _pool.Code);

            // 2) Indented code μέσα στο visible range (4 spaces/tab)
            range.SetStyle(_pool.Code, ReIndentedCode);

            // 3) ATX headings (visible range)
            range.SetStyle(_pool.Heading, ReHeadingAtx);

            // 4) Setext headings (βάφουμε μόνο την πρώτη γραμμή τίτλου)
            foreach (Match m in ReHeadingSetext.Matches(range.Text))
                ApplyGroup(range, m, "t", _pool.SetextHeading);

            // 5) Blockquotes, HR, Lists, Tables
            range.SetStyle(_pool.Blockquote, ReBlockquote);
            range.SetStyle(_pool.HorizontalRule, ReHr);
            range.SetStyle(_pool.ListMarker, ReListMarker);
            range.SetStyle(_pool.TableSep, ReTableSep);

            // 6) Images (ως link look)
            range.SetStyle(_pool.LinkText, ReImage);

            // 7) Links: text + url
            foreach (Match m in ReLink.Matches(range.Text))
            {
                ApplyGroup(range, m, "text", _pool.LinkText);
                ApplyGroup(range, m, "url", _pool.LinkUrl);
            }

            // 8) Inline code
            range.SetStyle(_pool.Code, ReInlineCode);

            // 9) Emphasis (σειρά έχει σημασία)
            range.SetStyle(_pool.BoldItalic, ReBoldItalic);
            range.SetStyle(_pool.Bold, ReBold);
            range.SetStyle(_pool.Italic, ReItalic);
        }


        private static void ApplyGroup(FastColoredTextBoxNS.Range baseRange, Match m, string groupName, Style style)
        {
            if (m is null) return;
            var g = m.Groups[groupName];
            if (!g.Success) return;

            int baseStartPos = baseRange.tb.PlaceToPosition(baseRange.Start);
            int absStart = baseStartPos + g.Index;
            int absEnd = absStart + g.Length;

            var startPlace = baseRange.tb.PositionToPlace(absStart);
            var endPlace = baseRange.tb.PositionToPlace(absEnd);

            var sub = new FastColoredTextBoxNS.Range(baseRange.tb, startPlace, endPlace);
            sub.SetStyle(style);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            _tb.TextChangedDelayed -= OnTextChangedDelayed;
            _tb.VisibleRangeChanged -= OnVisibleRangeChanged;

            if (_tb.Tag is IDictionary tag)
            {
                // αφήνω το Pool για επαναχρησιμοποίηση, μόνο το "attached" φεύγει
                tag.Remove(AttachedKey);
            }


        }

        private void EnsurePool()
        {
            // Reuse from Tag, or create exactly once
            if (_pool != null)
                return;

            if (_tb.Tag is not IDictionary tag)
            {
                tag = new Hashtable();
                _tb.Tag = tag;
            }

            if (tag.Contains(PoolKey) && tag[PoolKey] is Pool existing)
            {
                _pool = existing;
            }
            else
            {
                _pool = CreatePool();      // ΜΟΝΟ εδώ επιτρέπεται
                tag[PoolKey] = _pool;
            }
        }




        // μέσα στην κλάση MarkdownHighlighter
        public void ApplyLightTheme()
        {
            // Γενικό κείμενο
            _tb.BackColor = Color.FromArgb(253, 253, 254);
            _tb.ForeColor = Color.FromArgb(32, 35, 37);
            _tb.LineNumberColor = Color.FromArgb(140, 150, 160);
            _tb.ServiceLinesColor = Color.FromArgb(225, 230, 238);
            _tb.CurrentLineColor = Color.FromArgb(245, 247, 250);
            _tb.SelectionColor = Color.FromArgb(170, 205, 255);
            _tb.CaretColor = Color.Black;

            EnsurePool();

            // Ενημέρωσε τα foreground brushes των styles (πιο «γερά» για λευκό)
            SetBrush(_pool.Heading, Color.FromArgb(145, 60, 0));   // headings (#)
            SetBrush(_pool.SetextHeading, Color.FromArgb(160, 70, 0));   // setext
            SetBrush(_pool.Bold, Color.FromArgb(25, 25, 25));
            SetBrush(_pool.Italic, Color.FromArgb(35, 35, 35));
            SetBrush(_pool.BoldItalic, Color.FromArgb(20, 20, 20));
            SetBrush(_pool.Code, Color.FromArgb(90, 88, 50));  // inline/fenced
            SetBrush(_pool.Blockquote, Color.FromArgb(55, 100, 160));
            SetBrush(_pool.ListMarker, Color.FromArgb(25, 120, 100));
            SetBrush(_pool.LinkText, Color.FromArgb(0, 102, 204));
            SetBrush(_pool.LinkUrl, Color.FromArgb(30, 140, 220));
            SetBrush(_pool.HorizontalRule, Color.FromArgb(120, 130, 140));
            SetBrush(_pool.TableSep, Color.FromArgb(70, 120, 150));

            // Repaint
            Highlight(_tb.VisibleRange);
            _tb.Invalidate();
        }

        private static void SetBrush(TextStyle style, Color c)
        {
            // αντικατάσταση του ForeBrush με νέο SolidBrush
            if (style.ForeBrush is IDisposable d) d.Dispose();
            style.ForeBrush = new SolidBrush(c);
        }

        public void ApplyDarkTheme()
        {
            // Editor look
            _tb.BackColor = Color.FromArgb(30, 30, 30);
            _tb.ForeColor = Color.FromArgb(230, 230, 230);
            _tb.LineNumberColor = Color.FromArgb(130, 140, 150);
            _tb.ServiceLinesColor = Color.FromArgb(55, 65, 80);
            _tb.CurrentLineColor = Color.FromArgb(40, 45, 55);
            _tb.SelectionColor = Color.FromArgb(70, 110, 170);
            _tb.CaretColor = Color.White;
            _tb.IndentBackColor = Color.Transparent;
            _tb.WordWrap = true;
            _tb.WordWrapIndent = 2;

            // Darker text rendering
            //_tb.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            EnsurePool();

            // Update brush colors (ίδιες αποχρώσεις με του WebView2 dark CSS)
            SetBrush(_pool.Heading, Color.FromArgb(255, 189, 99));   // headings
            SetBrush(_pool.SetextHeading, Color.FromArgb(220, 160, 80));   // setext
            SetBrush(_pool.Bold, Color.FromArgb(235, 235, 235));
            SetBrush(_pool.Italic, Color.FromArgb(235, 235, 235));
            SetBrush(_pool.BoldItalic, Color.FromArgb(235, 235, 235));
            SetBrush(_pool.Code, Color.FromArgb(198, 200, 120));  // code
            SetBrush(_pool.Blockquote, Color.FromArgb(120, 170, 220));
            SetBrush(_pool.ListMarker, Color.FromArgb(120, 200, 180));
            SetBrush(_pool.LinkText, Color.FromArgb(120, 180, 240));
            SetBrush(_pool.LinkUrl, Color.FromArgb(140, 200, 255));
            SetBrush(_pool.HorizontalRule, Color.FromArgb(100, 110, 120));
            SetBrush(_pool.TableSep, Color.FromArgb(120, 180, 200));

            Highlight(_tb.VisibleRange);
            _tb.Invalidate();
        }


    }

}
