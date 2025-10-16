#pragma warning disable 1591

namespace Tripous.Forms
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class MarkdownTextBox : FastColoredTextBoxNS.FastColoredTextBox
    {
        // ====== Enums / Config ======

        public enum MarkdownTheme
        {
            Light = 0,
            Dark = 1
        }

        public class MarkdownPolicy
        {
            public bool UseAsteriskForItalic = true;      // *text*  (false => _text_)
            public bool UseAsteriskForBold = true;        // **text** (false => __text__)

            public bool UseDashForBullets = true;         // "- " (false => "* ")
            public bool RenumberOrderedLists = false;

            public bool AllowEmphasisInsideCode = false;
            public int FenceTicks = 3;                    // 3+

            public bool TableHeader = true;
            public string TableHeaderAlignment = "left";  // left|center|right

            public int MaxHeadingLevel = 6;               // 1..6

            public string NewLine = "\n";

            public MarkdownTheme DefaultTheme = MarkdownTheme.Dark;
        }

        // ====== Events ======

        public event System.EventHandler CommandExecuted;
        public event System.EventHandler MarkdownChanged;

        // ====== Fields ======

        private MarkdownPolicy _policy = new MarkdownPolicy();
        private MarkdownTheme _theme = MarkdownTheme.Dark;
        private MarkdownHighlighter _highlighter;

        // ====== Ctor / Init ======

        public MarkdownTextBox()
        {
            this.Language = FastColoredTextBoxNS.Language.Custom;
            this.AutoIndent = true;
            this.WordWrap = true;

            this.TextChangedDelayed += Editor_TextChangedDelayed;

            // attach highlighter στον ίδιο τον editor
            _highlighter = new MarkdownHighlighter(this);

            // αρχικό theme sync
            if (_theme == MarkdownTheme.Dark)
                _highlighter.ApplyDarkTheme();
            else
                _highlighter.ApplyLightTheme();
        }

        // ====== Public Properties ======

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public MarkdownPolicy Policy
        {
            get { return _policy; }
            set { _policy = value == null ? new MarkdownPolicy() : value; }
        }

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(MarkdownTheme.Dark)]
        public MarkdownTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        // ====== Theme API ======

        public virtual void ApplyTheme(MarkdownTheme theme)
        {
            _theme = theme;
            if (_highlighter != null)
            {
                if (theme == MarkdownTheme.Dark)
                    _highlighter.ApplyDarkTheme();
                else
                    _highlighter.ApplyLightTheme();
            }
        }

        // ====== High-level Markdown Commands ======

        // Inline
        public virtual void ToggleBold()
        {
            if (!Policy.AllowEmphasisInsideCode && (IsInsideInlineCode() || IsInsideFencedCode()))
                return;
            string t = Policy.UseAsteriskForBold ? "**" : "__";
            InlineWrapOrUnwrap(t, t);
            RaiseCommandExecuted("ToggleBold");
        }

        public virtual void ToggleItalic()
        {
            if (!Policy.AllowEmphasisInsideCode && (IsInsideInlineCode() || IsInsideFencedCode()))
                return;
            string t = Policy.UseAsteriskForItalic ? "*" : "_";
            InlineWrapOrUnwrap(t, t);
            RaiseCommandExecuted("ToggleItalic");
        }

        public virtual void ToggleBoldItalic()
        {
            if (!Policy.AllowEmphasisInsideCode && (IsInsideInlineCode() || IsInsideFencedCode()))
                return;
            string baseChar = Policy.UseAsteriskForBold ? "*" : "_";
            string t = baseChar + baseChar + baseChar;
            InlineWrapOrUnwrap(t, t);
            RaiseCommandExecuted("ToggleBoldItalic");
        }

        public virtual void ToggleInlineCode()
        {
            InlineWrapOrUnwrap("`", "`");
            RaiseCommandExecuted("ToggleInlineCode");
        }

        // Links / Images
        public virtual void InsertLink(string text, string url)
        {
            if (!Policy.AllowEmphasisInsideCode && (IsInsideInlineCode() || IsInsideFencedCode()))
                return;

            string sel = GetSelectedText();
            string linkText = sel.Length > 0 ? sel : text;
            if (linkText == null || linkText.Length == 0) linkText = "text";
            if (url == null) url = "";

            string nl = Policy.NewLine;

            BeginUndoGroup();
            try
            {
                if (sel.Length == 0 && (url == null || url.Length == 0))
                {
                    // []() και caret στα []
                    ReplaceSelection("[]()");
                    // τοποθέτησε caret μέσα στα []
                    int pos = this.PlaceToPosition(this.Selection.Start) - 3; // πριν το ")"
                    int openPos = pos - 2; // θέση του '['
                    FastColoredTextBoxNS.Place pA = this.PositionToPlace(openPos + 1);
                    FastColoredTextBoxNS.Place pB = pA;
                    this.Selection = new FastColoredTextBoxNS.Range(this, pA, pB);
                }
                else
                {
                    ReplaceSelection("[" + linkText + "](" + url + ")");
                }
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("InsertLink");
        }

        public virtual void InsertImage(string alt, string url)
        {
            if (!Policy.AllowEmphasisInsideCode && (IsInsideInlineCode() || IsInsideFencedCode()))
                return;

            string sel = GetSelectedText();
            string altText = sel.Length > 0 ? sel : alt;
            if (altText == null || altText.Length == 0) altText = "alt";
            if (url == null) url = "";

            BeginUndoGroup();
            try
            {
                if (sel.Length == 0 && (url == null || url.Length == 0))
                {
                    // ![]() και caret στα []
                    ReplaceSelection("![]()");
                    int pos = this.PlaceToPosition(this.Selection.Start) - 3;
                    int openPos = pos - 2;
                    FastColoredTextBoxNS.Place pA = this.PositionToPlace(openPos + 1);
                    this.Selection = new FastColoredTextBoxNS.Range(this, pA, pA);
                }
                else
                {
                    ReplaceSelection("![" + altText + "](" + url + ")");
                }
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("InsertImage");
        }

        // Headings
        public virtual void SetHeading(int level)
        {
            if (level < 0) level = 0;
            if (level > Policy.MaxHeadingLevel) level = Policy.MaxHeadingLevel;

            BeginUndoGroup();
            try
            {
                int startLine, endLine;
                GetSelectedLines(out startLine, out endLine);

                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Length == 0) continue;

                    int indentLen = LeadingWhitespaceLen(line);
                    string indent = line.Substring(0, indentLen);
                    string rest = line.Substring(indentLen);

                    // strip existing ATX hashes
                    int hashCount = 0;
                    int j = 0;
                    while (j < rest.Length && rest[j] == '#') { hashCount++; j++; }
                    if (hashCount > 0)
                    {
                        if (j < rest.Length && rest[j] == ' ') j++;
                        rest = rest.Substring(j);
                    }

                    if (level == 0)
                    {
                        SetLineText(i, indent + rest);
                    }
                    else
                    {
                        string hashes = Repeat('#', level);
                        SetLineText(i, indent + hashes + " " + rest);
                    }
                }

                ReselectLines(startLine, endLine);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("SetHeading");
        }

        public virtual void ClearHeading()
        {
            SetHeading(0);
            RaiseCommandExecuted("ClearHeading");
        }

        // Lists / Quotes
        public virtual void ToggleUnorderedList()
        {
            BeginUndoGroup();
            try
            {
                int startLine, endLine;
                GetSelectedLines(out startLine, out endLine);

                bool allHave = true;
                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Trim().Length == 0) continue;
                    if (!IsUnorderedLine(line)) { allHave = false; break; }
                }

                string bullet = Policy.UseDashForBullets ? "- " : "* ";

                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Length == 0) continue;

                    int indentLen = LeadingWhitespaceLen(line);
                    string indent = line.Substring(0, indentLen);
                    string rest = line.Substring(indentLen);

                    if (rest.Length == 0)
                    {
                        SetLineText(i, indent + (allHave ? "" : bullet));
                        continue;
                    }

                    if (allHave)
                    {
                        if (rest.Length >= 2 && (rest[0] == '-' || rest[0] == '*') && rest[1] == ' ')
                            rest = rest.Substring(2);
                        SetLineText(i, indent + rest);
                    }
                    else
                    {
                        SetLineText(i, indent + bullet + rest);
                    }
                }

                ReselectLines(startLine, endLine);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("ToggleUnorderedList");
        }

        public virtual void ToggleOrderedList(int startAt)
        {
            if (startAt < 1) startAt = 1;

            BeginUndoGroup();
            try
            {
                int startLine, endLine;
                GetSelectedLines(out startLine, out endLine);

                bool allHave = true;
                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Trim().Length == 0) continue;
                    if (!IsOrderedLine(line)) { allHave = false; break; }
                }

                int number = startAt;
                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Length == 0) continue;

                    int indentLen = LeadingWhitespaceLen(line);
                    string indent = line.Substring(0, indentLen);
                    string rest = line.Substring(indentLen);

                    if (rest.Length == 0)
                    {
                        if (allHave)
                        {
                            SetLineText(i, indent);
                        }
                        else
                        {
                            SetLineText(i, indent + number.ToString() + ". ");
                            number++;
                        }
                        continue;
                    }

                    if (allHave)
                    {
                        int k = 0;
                        while (k < rest.Length && rest[k] >= '0' && rest[k] <= '9') k++;
                        if (k < rest.Length && rest[k] == '.' && k + 1 < rest.Length && rest[k + 1] == ' ')
                            rest = rest.Substring(k + 2);
                        SetLineText(i, indent + rest);
                    }
                    else
                    {
                        string content = rest;
                        int k = 0;
                        while (k < content.Length && content[k] >= '0' && content[k] <= '9') k++;
                        if (k < content.Length && content[k] == '.' && k + 1 < content.Length && content[k + 1] == ' ')
                            content = content.Substring(k + 2);

                        SetLineText(i, indent + number.ToString() + ". " + content);
                        number++;
                    }
                }

                if (Policy.RenumberOrderedLists && allHave)
                {
                    int num = startAt;
                    for (int i = startLine; i <= endLine; i++)
                    {
                        string line = SafeGetLineText(i);
                        if (line.Trim().Length == 0) continue;

                        int indentLen = LeadingWhitespaceLen(line);
                        string indent = line.Substring(0, indentLen);
                        string rest = line.Substring(indentLen);

                        int k = 0;
                        while (k < rest.Length && rest[k] >= '0' && rest[k] <= '9') k++;
                        if (k < rest.Length && rest[k] == '.' && k + 1 < rest.Length && rest[k + 1] == ' ')
                        {
                            string content = rest.Substring(k + 2);
                            SetLineText(i, indent + num.ToString() + ". " + content);
                            num++;
                        }
                    }
                }

                ReselectLines(startLine, endLine);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("ToggleOrderedList");
        }

        public virtual void ToggleTaskList()
        {
            // Προσθαφαιρεί task marker "- [ ] " ή "* [ ] " με βάση Policy.UseDashForBullets.
            BeginUndoGroup();
            try
            {
                int startLine, endLine;
                GetSelectedLines(out startLine, out endLine);

                bool allHave = true;
                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Trim().Length == 0) continue;
                    if (!IsTaskLine(line)) { allHave = false; break; }
                }

                string bullet = Policy.UseDashForBullets ? "- " : "* ";

                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    int indentLen = LeadingWhitespaceLen(line);
                    string indent = line.Substring(0, indentLen);
                    string rest = line.Substring(indentLen);

                    if (rest.Length == 0)
                    {
                        if (!allHave) SetLineText(i, indent + bullet + "[ ] ");
                        else SetLineText(i, indent);
                        continue;
                    }

                    if (allHave)
                    {
                        // Αφαίρεση task marker
                        if (StartsWithTask(rest))
                        {
                            string after = rest.Substring(TaskPrefixLen(rest));
                            SetLineText(i, indent + after);
                        }
                        else
                        {
                            SetLineText(i, indent + rest);
                        }
                    }
                    else
                    {
                        // Αν έχει ήδη απλό bullet, αντικατάστησέ το με task bullet
                        if (IsUnorderedRest(rest))
                        {
                            string content = rest.Substring(2);
                            SetLineText(i, indent + bullet + "[ ] " + content);
                        }
                        else if (StartsWithTask(rest))
                        {
                            // ήδη task: άστο ως έχει
                            SetLineText(i, indent + rest);
                        }
                        else
                        {
                            SetLineText(i, indent + bullet + "[ ] " + rest);
                        }
                    }
                }

                ReselectLines(startLine, endLine);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("ToggleTaskList");
        }

        public virtual void ToggleBlockquote()
        {
            BeginUndoGroup();
            try
            {
                int startLine, endLine;
                GetSelectedLines(out startLine, out endLine);

                bool allHave = true;
                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    if (line.Trim().Length == 0) continue;
                    if (!IsBlockquoteLine(line)) { allHave = false; break; }
                }

                for (int i = startLine; i <= endLine; i++)
                {
                    string line = SafeGetLineText(i);
                    int indentLen = LeadingWhitespaceLen(line);
                    string indent = line.Substring(0, indentLen);
                    string rest = line.Substring(indentLen);

                    if (rest.Length == 0)
                    {
                        if (!allHave) SetLineText(i, indent + "> ");
                        else SetLineText(i, indent);
                        continue;
                    }

                    if (allHave)
                    {
                        if (rest.StartsWith("> ", System.StringComparison.Ordinal))
                            rest = rest.Substring(2);
                        else if (rest.StartsWith(">", System.StringComparison.Ordinal))
                            rest = rest.Substring(1);
                        SetLineText(i, indent + rest);
                    }
                    else
                    {
                        SetLineText(i, indent + "> " + rest);
                    }
                }

                ReselectLines(startLine, endLine);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("ToggleBlockquote");
        }

        // Code fences
        public virtual void InsertFencedCode(string language)
        {
            if (language == null) language = "";
            string nl = Policy.NewLine;
            int ticks = Policy.FenceTicks < 3 ? 3 : Policy.FenceTicks;
            string fence = Repeat('`', ticks);

            BeginUndoGroup();
            try
            {
                FastColoredTextBoxNS.Place startPlace = this.Selection.Start;
                int startAbs = this.PlaceToPosition(startPlace);

                string sel = GetSelectedText();

                if (sel.Length == 0)
                {
                    string header = fence + (language.Length > 0 ? language : "");
                    string toInsert = header + nl + nl + fence + nl;
                    ReplaceSelection(toInsert);

                    int insideAbs = startAbs + header.Length + nl.Length;
                    FastColoredTextBoxNS.Place insidePlace = this.PositionToPlace(insideAbs);
                    this.Selection = new FastColoredTextBoxNS.Range(this, insidePlace, insidePlace);
                }
                else
                {
                    string header = fence + (language.Length > 0 ? language : "");
                    string toInsert = header + nl + sel + nl + fence + nl;
                    ReplaceSelection(toInsert);
                }
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("InsertFencedCode");
        }

        // Horizontal rule
        public virtual void InsertHorizontalRule()
        {
            string nl = Policy.NewLine;
            string hr = "---";
            BeginUndoGroup();
            try
            {
                // εξασφάλισε να μπει σε δική του γραμμή
                int iChar = this.Selection.Start.iChar;
                if (iChar > 0) ReplaceSelection(nl + hr + nl);
                else ReplaceSelection(hr + nl);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("InsertHorizontalRule");
        }

        // Tables
        public virtual void InsertTable(int columns, int rows, bool withHeader)
        {
            if (columns < 1) columns = 1;
            if (rows < 1) rows = 1;

            string nl = Policy.NewLine;

            string[] headerCells = new string[columns];
            for (int c = 0; c < columns; c++) headerCells[c] = "Header " + (c + 1).ToString();

            string alignToken = "---";
            if (Policy.TableHeaderAlignment == "center") alignToken = ":---:";
            else if (Policy.TableHeaderAlignment == "right") alignToken = "---:";
            else if (Policy.TableHeaderAlignment == "left") alignToken = ":---";

            string[] alignCells = new string[columns];
            for (int c = 0; c < columns; c++) alignCells[c] = alignToken;

            string[] bodyRow = new string[columns];
            for (int c = 0; c < columns; c++) bodyRow[c] = " ";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (withHeader)
            {
                sb.Append(RowLine(headerCells)).Append(nl);
                sb.Append(RowLine(alignCells)).Append(nl);
            }

            for (int r = 0; r < rows; r++)
                sb.Append(RowLine(bodyRow)).Append(nl);

            string tableText = sb.ToString();

            BeginUndoGroup();
            try
            {
                bool atLineStart = this.Selection.Start.iChar == 0;
                if (!atLineStart) tableText = nl + tableText;

                ReplaceSelection(tableText);

                int insertedLenBeforeBody = 0;
                if (withHeader)
                {
                    insertedLenBeforeBody += RowLine(headerCells).Length + nl.Length;
                    insertedLenBeforeBody += RowLine(alignCells).Length + nl.Length;
                }
                if (!atLineStart) insertedLenBeforeBody += nl.Length;

                int absStart = this.PlaceToPosition(this.Selection.Start) - tableText.Length;
                int caretAbs = absStart + insertedLenBeforeBody + 2; // "| "

                FastColoredTextBoxNS.Place caretPlace = this.PositionToPlace(caretAbs);
                this.Selection = new FastColoredTextBoxNS.Range(this, caretPlace, caretPlace);
            }
            finally { EndUndoGroup(); }
            RaiseCommandExecuted("InsertTable");
        }

        public virtual void AddTableRow() { }
        public virtual void AddTableColumn() { }

        // ====== Helpers ======

        protected virtual void BeginUndoGroup()
        {
            this.BeginUpdate();
        }

        protected virtual void EndUndoGroup()
        {
            this.EndUpdate();
            RaiseMarkdownChanged();
        }

        protected virtual string GetSelectedText()
        {
            return this.Selection.Text;
        }

        protected virtual void ReplaceSelection(string newText)
        {
            this.InsertText(newText);
        }

        protected virtual void ExpandSelectionToWord()
        {
            if (!this.Selection.IsEmpty)
                return;
            FastColoredTextBoxNS.Range word = this.Selection.GetFragment(@"\w");
            if (!word.IsEmpty)
                this.Selection = word;
        }

        protected virtual bool IsInsideInlineCode()
        {
            int lineIndex = this.Selection.Start.iLine;
            if (lineIndex < 0 || lineIndex >= this.LinesCount) return false;

            string line = this.Lines[lineIndex];
            int caretX = this.Selection.Start.iChar;
            if (caretX > line.Length) caretX = line.Length;

            int left = CountChar(line, '`', 0, caretX);
            int right = CountChar(line, '`', caretX, line.Length - caretX);
            return (left % 2 == 1) && (right > 0);
        }

        protected virtual bool IsInsideFencedCode()
        {
            int caretAbs = this.PlaceToPosition(this.Selection.Start);
            string text = this.Text;
            if (caretAbs < 0) caretAbs = 0;
            if (caretAbs > text.Length) caretAbs = text.Length;

            int count = 0;
            int idx = 0;
            int ticks = Policy.FenceTicks < 3 ? 3 : Policy.FenceTicks;
            string fence = new string('`', ticks);

            while (idx <= caretAbs - ticks)
            {
                int pos = text.IndexOf(fence, idx, caretAbs - idx, System.StringComparison.Ordinal);
                if (pos < 0) break;
                count++;
                idx = pos + ticks;
            }
            return (count % 2) == 1;
        }

        protected virtual void RaiseCommandExecuted(string commandName)
        {
            System.EventHandler h = CommandExecuted;
            if (h != null) h(this, System.EventArgs.Empty);
        }

        protected virtual void RaiseMarkdownChanged()
        {
            System.EventHandler h = MarkdownChanged;
            if (h != null) h(this, System.EventArgs.Empty);
        }

        private bool TryExpandSelectionToWrapped(string leftToken, string rightToken)
        {
            if (!this.Selection.IsEmpty)
                return false;

            int lineIndex = this.Selection.Start.iLine;
            if (lineIndex < 0 || lineIndex >= this.LinesCount)
                return false;

            string line = this.Lines[lineIndex];
            int caretX = this.Selection.Start.iChar;
            if (caretX < 0) caretX = 0;
            if (caretX > line.Length) caretX = line.Length;

            // Απόσπασμα λέξης στο caret
            FastColoredTextBoxNS.Range frag = this.Selection.GetFragment(@"\w");
            if (frag.IsEmpty)
                return false;

            int startX = frag.Start.iChar;
            int endX = frag.End.iChar;

            // Υπάρχουν tokens ακριβώς πριν/μετά το fragment;
            if (startX >= leftToken.Length && endX + rightToken.Length <= line.Length)
            {
                string left = line.Substring(startX - leftToken.Length, leftToken.Length);
                string right = line.Substring(endX, rightToken.Length);

                if (string.Compare(left, leftToken, System.StringComparison.Ordinal) == 0 &&
                    string.Compare(right, rightToken, System.StringComparison.Ordinal) == 0)
                {
                    FastColoredTextBoxNS.Place a = new FastColoredTextBoxNS.Place(startX - leftToken.Length, lineIndex);
                    FastColoredTextBoxNS.Place b = new FastColoredTextBoxNS.Place(endX + rightToken.Length, lineIndex);
                    this.Selection = new FastColoredTextBoxNS.Range(this, a, b);
                    return true;
                }
            }

            return false;
        }


        private void Editor_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            RaiseMarkdownChanged();
        }

        // ---- Inline core ----

        private void InlineWrapOrUnwrap(string leftToken, string rightToken)
        {
            BeginUndoGroup();
            try
            {
                if (this.Selection.IsEmpty)
                {
                    // Αν είμαστε ήδη μέσα σε **word** ή *word* κ.λπ., πιάσε ΚΑΙ τα tokens.
                    if (!TryExpandSelectionToWrapped(leftToken, rightToken))
                        ExpandSelectionToWord();
                }

                string sel = GetSelectedText();

                if (sel.Length == 0)
                {
                    ReplaceSelection(leftToken + rightToken);
                    int startPos = this.PlaceToPosition(this.Selection.Start) - rightToken.Length;
                    FastColoredTextBoxNS.Place p = this.PositionToPlace(startPos);
                    this.Selection = new FastColoredTextBoxNS.Range(this, p, p);
                    return;
                }

                if (HasWrapping(sel, leftToken, rightToken))
                {
                    string inner = sel.Substring(leftToken.Length, sel.Length - leftToken.Length - rightToken.Length);
                    ReplaceSelection(inner);
                }
                else
                {
                    string trimmed = sel.Trim();
                    int leading = sel.Length - sel.TrimStart().Length;
                    int trailing = sel.Length - sel.TrimEnd().Length;

                    string result = sel.Substring(0, leading) + leftToken + trimmed + rightToken + sel.Substring(sel.Length - trailing, trailing);
                    ReplaceSelection(result);
                }
            }
            finally { EndUndoGroup(); }
        }

        private static bool HasWrapping(string text, string leftToken, string rightToken)
        {
            if (text.Length < leftToken.Length + rightToken.Length) return false;
            if (!text.StartsWith(leftToken, System.StringComparison.Ordinal)) return false;
            if (!text.EndsWith(rightToken, System.StringComparison.Ordinal)) return false;
            return true;
        }

        private static int CountChar(string s, char ch, int start, int count)
        {
            int end = start + count;
            if (start < 0) start = 0;
            if (end > s.Length) end = s.Length;
            int c = 0;
            for (int i = start; i < end; i++)
                if (s[i] == ch) c++;
            return c;
        }

        // ---- Line helpers for block commands ----

        private void GetSelectedLines(out int startLine, out int endLine)
        {
            startLine = this.Selection.Start.iLine;
            endLine = this.Selection.End.iLine;

            if (this.Selection.End.iChar == 0 && endLine > startLine)
                endLine--;
        }

        private string SafeGetLineText(int lineIndex)
        {

            if (lineIndex < 0) lineIndex = 0;
            if (lineIndex >= this.LinesCount) return "";
            return this.Lines[lineIndex];
        }

        private void SetLineText(int lineIndex, string newText)
        {
            FastColoredTextBoxNS.Place a = new FastColoredTextBoxNS.Place(0, lineIndex);
            int len = 0;
            if (lineIndex >= 0 && lineIndex < this.LinesCount)
                len = this.Lines[lineIndex].Length;
            FastColoredTextBoxNS.Place b = new FastColoredTextBoxNS.Place(len, lineIndex);
            FastColoredTextBoxNS.Range r = new FastColoredTextBoxNS.Range(this, a, b);
            this.Selection = r;
            this.InsertText(newText);
        }

        private void ReselectLines(int startLine, int endLine)
        {
            FastColoredTextBoxNS.Place a = new FastColoredTextBoxNS.Place(0, startLine);
            int endLen = 0;
            if (endLine >= 0 && endLine < this.LinesCount)
                endLen = this.Lines[endLine].Length;
            FastColoredTextBoxNS.Place b = new FastColoredTextBoxNS.Place(endLen, endLine);
            this.Selection = new FastColoredTextBoxNS.Range(this, a, b);
        }

        private static int LeadingWhitespaceLen(string s)
        {
            int i = 0;
            while (i < s.Length && (s[i] == ' ' || s[i] == '\t')) i++;
            return i;
        }

        private static bool IsUnorderedLine(string line)
        {
            int i = LeadingWhitespaceLen(line);
            if (i + 1 >= line.Length) return false;
            char ch = line[i];
            if ((ch == '-' || ch == '*') && i + 1 < line.Length && line[i + 1] == ' ') return true;
            return false;
        }

        private static bool IsUnorderedRest(string rest)
        {
            if (rest.Length < 2) return false;
            if ((rest[0] == '-' || rest[0] == '*') && rest[1] == ' ') return true;
            return false;
        }

        private static bool IsOrderedLine(string line)
        {
            int i = LeadingWhitespaceLen(line);
            int j = i;
            while (j < line.Length && line[j] >= '0' && line[j] <= '9') j++;
            if (j == i) return false;
            if (j < line.Length && line[j] == '.' && j + 1 < line.Length && line[j + 1] == ' ') return true;
            return false;
        }

        private static bool IsBlockquoteLine(string line)
        {
            int i = LeadingWhitespaceLen(line);
            if (i >= line.Length) return false;
            if (line[i] == '>') return true;
            return false;
        }

        private static bool IsTaskLine(string line)
        {
            int i = LeadingWhitespaceLen(line);
            if (i >= line.Length) return false;
            // "- [ ] ..." ή "- [x] ..." ή "* [ ] ..." ή "* [x] ..."
            if ((line[i] == '-' || line[i] == '*') &&
                i + 3 < line.Length &&
                line[i + 1] == ' ' &&
                line[i + 2] == '[' &&
                (line[i + 3] == ' ' || line[i + 3] == 'x' || line[i + 3] == 'X') &&
                i + 4 < line.Length &&
                line[i + 4] == ']' &&
                i + 5 < line.Length &&
                line[i + 5] == ' ')
            {
                return true;
            }
            return false;
        }

        private static bool StartsWithTask(string rest)
        {
            if (rest.Length < 6) return false;
            if ((rest[0] == '-' || rest[0] == '*') &&
                rest[1] == ' ' &&
                rest[2] == '[' &&
                (rest[3] == ' ' || rest[3] == 'x' || rest[3] == 'X') &&
                rest[4] == ']' &&
                rest[5] == ' ')
                return true;
            return false;
        }

        private static int TaskPrefixLen(string rest)
        {
            // "- [ ] " ή "- [x] " => 6
            return 6;
        }

        private static string Repeat(char ch, int count)
        {
            if (count < 0) count = 0;
            char[] arr = new char[count];
            for (int i = 0; i < count; i++) arr[i] = ch;
            return new string(arr);
        }

        private static string RowLine(string[] cells)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("|");
            for (int i = 0; i < cells.Length; i++)
            {
                sb.Append(' ').Append(cells[i]).Append(' ').Append('|');
            }
            return sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_highlighter != null)
                {
                    _highlighter.Dispose();
                    _highlighter = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.B))
            {
                ToggleBold();
                return true; // handled
            }

            if (keyData == (Keys.Control | Keys.I))
            {
                ToggleItalic();
                return true; // handled
            }

            if (keyData == (Keys.Control | Keys.T))
            {
                InsertTable(3, 2, true);
                return true; // handled
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                ReformatTableAtCaret();
                return true; // handled
            }


            if (keyData == (Keys.Control | Keys.Alt | Keys.I))
            {
                BuildOrUpdateIndex();
                return true; // handled
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        // table re-format
        public void ReformatTableAtCaret()
        {
            int line = this.Selection.Start.iLine;
            if (line < 0 || line >= this.LinesCount) return;

            int top, bottom;
            if (!FindTableBlockAroundLine(line, out top, out bottom))
                return;

            System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();
            for (int i = top; i <= bottom; i++)
                lines.Add(SafeGetLineText(i));

            // Parse rows into cells
            System.Collections.Generic.List<string[]> rows = new System.Collections.Generic.List<string[]>();
            int maxCols = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                string s = lines[i].Trim();
                if (s.Length == 0) continue;

                // Ensure starting and ending pipe for consistent split
                if (s[0] != '|') s = "|" + s;
                if (s[s.Length - 1] != '|') s = s + "|";

                string[] raw = s.Split('|');
                System.Collections.Generic.List<string> cells = new System.Collections.Generic.List<string>();
                for (int k = 1; k < raw.Length - 1; k++)
                    cells.Add(raw[k].Trim());

                rows.Add(cells.ToArray());
                if (cells.Count > maxCols) maxCols = cells.Count;
            }
            if (rows.Count == 0 || maxCols == 0) return;

            // Detect if second row is alignment
            int headerIdx = -1;
            if (rows.Count >= 2 && IsAlignmentRow(rows[1]))
                headerIdx = 0;

            // If no alignment and policy wants header, synthesize
            if (headerIdx == -1 && Policy.TableHeader)
            {
                // Insert default header (if first row not empty)
                string[] header = new string[maxCols];
                for (int c = 0; c < maxCols; c++)
                    header[c] = rows[0].Length > c ? rows[0][c] : "";
                rows.Insert(1, CreateAlignmentRow(maxCols, Policy.TableHeaderAlignment));
                headerIdx = 0;
            }
            else if (headerIdx != -1)
            {
                // Normalize alignment row to policy
                rows[1] = CreateAlignmentRow(maxCols, Policy.TableHeaderAlignment);
            }

            // Normalize column counts and compute widths (excluding alignment row)
            int[] widths = new int[maxCols];
            for (int r = 0; r < rows.Count; r++)
            {
                if (headerIdx != -1 && r == 1) continue; // skip alignment
                string[] row = rows[r];
                if (row.Length != maxCols)
                {
                    string[] newRow = new string[maxCols];
                    int copy = row.Length < maxCols ? row.Length : maxCols;
                    for (int c = 0; c < copy; c++) newRow[c] = row[c];
                    for (int c = copy; c < maxCols; c++) newRow[c] = "";
                    rows[r] = newRow;
                    row = newRow;
                }
                for (int c = 0; c < maxCols; c++)
                {
                    int len = row[c] == null ? 0 : row[c].Length;
                    if (len > widths[c]) widths[c] = len;
                }
            }

            // Ensure alignment widths at least 3
            if (headerIdx != -1)
            {
                for (int c = 0; c < maxCols; c++)
                    if (widths[c] < 3) widths[c] = 3;
            }

            // Rebuild lines
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string nl = Policy.NewLine;
            for (int r = 0; r < rows.Count; r++)
            {
                if (headerIdx != -1 && r == 1)
                {
                    // alignment row from policy, already created with minimal tokens; pad to widths
                    string[] align = rows[r];
                    // stretch tokens to match widths
                    string[] stretched = new string[maxCols];
                    for (int c = 0; c < maxCols; c++)
                        stretched[c] = StretchAlignmentToken(align[c], widths[c]);
                    sb.Append(BuildRow(stretched)).Append(nl);
                }
                else
                {
                    string[] row = rows[r];
                    string[] padded = new string[maxCols];
                    for (int c = 0; c < maxCols; c++)
                        padded[c] = PadCell(row[c], widths[c]);
                    sb.Append(BuildRow(padded)).Append(nl);
                }
            }
            string newBlock = sb.ToString();

            // Replace document block
            BeginUndoGroup();
            try
            {
                ReplaceLines(top, bottom, newBlock);
                // Reselect the table block nicely
                ReselectLines(top, top + (newBlock.Length == 0 ? 0 : CountLines(newBlock) - 1));
            }
            finally { EndUndoGroup(); }
        }

        private bool FindTableBlockAroundLine(int line, out int top, out int bottom)
        {
            top = line;
            bottom = line;
            // expand upwards
            while (top > 0 && LineLooksLikeTable(this.Lines[top - 1])) top--;
            // expand downwards
            while (bottom + 1 < this.LinesCount && LineLooksLikeTable(this.Lines[bottom + 1])) bottom++;
            // at least one line must be table-like
            return LineLooksLikeTable(this.Lines[line]);
        }

        private bool LineLooksLikeTable(string s)
        {
            if (s == null) return false;
            if (s.IndexOf('|') >= 0) return true;
            return false;
        }



        private void ReplaceLines(int startLine, int endLine, string newTextWithNl)
        {
            FastColoredTextBoxNS.Place a = new FastColoredTextBoxNS.Place(0, startLine);
            int endLen = 0;
            if (endLine >= 0 && endLine < this.LinesCount)
                endLen = this.Lines[endLine].Length;
            FastColoredTextBoxNS.Place b = new FastColoredTextBoxNS.Place(endLen, endLine);
            FastColoredTextBoxNS.Range r = new FastColoredTextBoxNS.Range(this, a, b);
            this.Selection = r;
            this.InsertText(newTextWithNl);
        }

        private static bool IsAlignmentRow(string[] cells)
        {
            if (cells == null || cells.Length == 0) return false;
            for (int i = 0; i < cells.Length; i++)
            {
                string t = cells[i].Trim();
                if (t.Length < 3) return false;
                // valid patterns: ---  :---  ---:  :---:
                bool left = t.Length >= 4 && t[0] == ':' && AllDashes(t, 1, t.Length - 1);
                bool right = t.Length >= 4 && t[t.Length - 1] == ':' && AllDashes(t, 0, t.Length - 1);
                bool both = t.Length >= 5 && t[0] == ':' && t[t.Length - 1] == ':' && AllDashes(t, 1, t.Length - 2);
                bool plain = AllDashes(t, 0, t.Length);
                if (!(left || right || both || plain)) return false;
            }
            return true;
        }

        private static bool AllDashes(string s, int start, int len)
        {
            int end = start + len;
            if (start < 0) start = 0;
            if (end > s.Length) end = s.Length;
            for (int i = start; i < end; i++)
                if (s[i] != '-') return false;
            return len >= 3;
        }

        private string[] CreateAlignmentRow(int cols, string align)
        {
            string token;
            if (align == "center") token = ":---:";
            else if (align == "right") token = "---:";
            else if (align == "left") token = ":---";
            else token = "---";
            string[] row = new string[cols];
            for (int c = 0; c < cols; c++) row[c] = token;
            return row;
        }

        private string StretchAlignmentToken(string token, int width)
        {
            // make the number of dashes at least width (but keep colons at ends)
            if (token == null || token.Length == 0) token = "---";
            bool left = token[0] == ':';
            bool right = token[token.Length - 1] == ':';
            int core = width;
            if (left) core--;
            if (right) core--;
            if (core < 3) core = 3;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (left) sb.Append(':');
            for (int i = 0; i < core; i++) sb.Append('-');
            if (right) sb.Append(':');
            return sb.ToString();
        }

        private string PadCell(string s, int width)
        {
            if (s == null) s = "";
            // απλό right padding για σταθερό πλάτος
            int pad = width - s.Length;
            if (pad < 0) pad = 0;
            return s + new string(' ', pad);
        }

        private string BuildRow(string[] cells)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("|");
            for (int i = 0; i < cells.Length; i++)
            {
                string cell = cells[i] == null ? "" : cells[i];
                sb.Append(' ').Append(cell).Append(' ').Append('|');
            }
            return sb.ToString();
        }

        private int CountLines(string text)
        {
            if (text == null || text.Length == 0) return 0;
            int n = 0;
            for (int i = 0; i < text.Length; i++) if (text[i] == '\n') n++;
            return n;
        }



        public void BuildOrUpdateIndex()
        {
            // Συλλογή headings
            System.Collections.Generic.List<System.Tuple<int, string>> items = new System.Collections.Generic.List<System.Tuple<int, string>>();
            for (int i = 0; i < this.LinesCount; i++)
            {
                string line = this.Lines[i];
                int hashes = CountLeadingHashes(line);
                if (hashes >= 1 && hashes <= Policy.MaxHeadingLevel)
                {
                    string title = line.Substring(hashes).TrimStart();
                    if (title.Length > 0)
                        items.Add(new System.Tuple<int, string>(hashes, title));
                }
            }
            if (items.Count == 0) return;

            // φτιάξε ToC σε markdown
            System.Text.StringBuilder toc = new System.Text.StringBuilder();
            string nl = Policy.NewLine;
            toc.Append("<!-- INDEX START -->").Append(nl);
            toc.Append("# Περιεχόμενα").Append(nl).Append(nl);

            for (int i = 0; i < items.Count; i++)
            {
                int level = items[i].Item1;
                string text = items[i].Item2;
                string slug = Slugify(text);

                // indentation ανά επίπεδο (π.χ. 2 spaces per level)
                int indent = (level - 1) * 2;
                for (int k = 0; k < indent; k++) toc.Append(' ');
                toc.Append("- [").Append(text).Append("](#").Append(slug).Append(")").Append(nl);
            }
            toc.Append(nl).Append("<!-- INDEX END -->").Append(nl).Append(nl);

            // Αντικατάσταση αν υπάρχει ήδη μπλοκ INDEX, αλλιώς εισαγωγή στην αρχή
            int startLine, endLine;
            if (FindIndexBlock(out startLine, out endLine))
            {
                BeginUndoGroup();
                try
                {
                    ReplaceLines(startLine, endLine, toc.ToString());
                    ReselectLines(startLine, startLine + CountLines(toc.ToString()) - 1);
                }
                finally { EndUndoGroup(); }
            }
            else
            {
                BeginUndoGroup();
                try
                {
                    // εισαγωγή στην αρχή του εγγράφου
                    FastColoredTextBoxNS.Place a = new FastColoredTextBoxNS.Place(0, 0);
                    FastColoredTextBoxNS.Range r = new FastColoredTextBoxNS.Range(this, a, a);
                    this.Selection = r;
                    this.InsertText(toc.ToString());
                }
                finally { EndUndoGroup(); }
            }
        }

        private int CountLeadingHashes(string s)
        {
            int c = 0;
            while (c < s.Length && s[c] == '#') c++;
            if (c < s.Length && s[c] == ' ')
                return c;
            return 0;
        }

        private bool FindIndexBlock(out int startLine, out int endLine)
        {
            startLine = -1;
            endLine = -1;
            for (int i = 0; i < this.LinesCount; i++)
            {
                string line = this.Lines[i].Trim();
                if (string.Compare(line, "<!-- INDEX START -->", System.StringComparison.Ordinal) == 0)
                {
                    startLine = i;
                    break;
                }
            }
            if (startLine == -1) return false;

            for (int j = startLine + 1; j < this.LinesCount; j++)
            {
                string line = this.Lines[j].Trim();
                if (string.Compare(line, "<!-- INDEX END -->", System.StringComparison.Ordinal) == 0)
                {
                    endLine = j;
                    return true;
                }
            }
            return false;
        }

        private string Slugify(string s)
        {
            // απλό slug: lower, αντικατάσταση κενών με '-', αφαίρεση μη-alnum (εκτός '-')
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (ch >= 'A' && ch <= 'Z') ch = (char)(ch - 'A' + 'a');
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                    sb.Append(ch);
                else if (ch == ' ' || ch == '-' || ch == '_')
                    sb.Append('-');
                // αλλιώς αγνόησε
            }
            // συμπίεσε πολλαπλά '-'
            string slug = sb.ToString();
            System.Text.StringBuilder outp = new System.Text.StringBuilder();
            bool lastDash = false;
            for (int i = 0; i < slug.Length; i++)
            {
                char ch = slug[i];
                if (ch == '-')
                {
                    if (!lastDash) { outp.Append('-'); lastDash = true; }
                }
                else { outp.Append(ch); lastDash = false; }
            }
            // trim dashes
            string res = outp.ToString();
            int a = 0; while (a < res.Length && res[a] == '-') a++;
            int b = res.Length - 1; while (b >= a && res[b] == '-') b--;
            if (a > 0 || b < res.Length - 1)
                res = res.Substring(a, b - a + 1);
            if (res.Length == 0) res = "section";
            return res;
        }


        public void LoadCheatSheet()
        {
            string FilePath = Path.Combine(System.AppContext.BaseDirectory, "CheatSheet.md");
            LoadFromFile(FilePath);
        }
        public void LoadFromFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                string MarkdownText = File.ReadAllText(FilePath);
                this.Text = MarkdownText;
            }
        }
    }
}
