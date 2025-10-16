 

namespace Tripous.Forms
{
    public static class FastFindReplace
    {
        // --- Options ---
        public sealed class FindOptions
        {
            public bool MatchCase { get; set; }
            public bool WholeWord { get; set; }
            public bool UseRegex { get; set; }
            public bool SearchSelectionOnly { get; set; }
            public bool Wrap { get; set; } = true;
            public bool Forward { get; set; } = true;
        }

        private const string SearchStyleKey = "__SW_SEARCH_STYLE__";

        private static MarkerStyle EnsureSearchStyle(FastColoredTextBox tb)
        {
            if (tb.Tag is not IDictionary tag)
            {
                tag = new Hashtable();
                tb.Tag = tag;
            }
            if (tag[SearchStyleKey] is MarkerStyle s) return s;

            // Ημιδιάφανο γαλάζιο highlight
            //var style = new MarkerStyle(new SolidBrush(Color.FromArgb(70, 180, 220, 255)));
            var style = new MarkerStyle(new SolidBrush(Color.FromArgb(90, 255, 255, 0))); // bright yellow highlight
            //var style = new MarkerStyle(new SolidBrush(Color.FromArgb(100, 255, 230, 100))); // warm light-yellow
            //var style = new MarkerStyle(new SolidBrush(Color.Yellow));

            tag[SearchStyleKey] = style;
            return style;
        }

        private static Regex BuildRegex(string query, FindOptions opt)
        {
            if (string.IsNullOrEmpty(query))
                return new Regex("(?!x)x", RegexOptions.Compiled); // no-op

            string pattern;
            if (opt.UseRegex)
            {
                pattern = query;
            }
            else
            {
                pattern = Regex.Escape(query);
                if (opt.WholeWord)
                    pattern = $@"\b{pattern}\b";
            }

            var ro = RegexOptions.Compiled;
            if (!opt.MatchCase) ro |= RegexOptions.IgnoreCase;

            return new Regex(pattern, ro);
        }

        private static FastColoredTextBoxNS.Range GetScope(FastColoredTextBox tb, bool selectionOnly)
        {
            return selectionOnly && !tb.Selection.IsEmpty ? tb.Selection.Clone() : tb.Range;
        }

        /// <summary>Χρωματίζει όλα τα matches (στο scope) και επιστρέφει πόσα είναι.</summary>
        public static int HighlightAll(FastColoredTextBox tb, string query, FindOptions opt)
        {
            var style = EnsureSearchStyle(tb);
            var scope = GetScope(tb, opt.SearchSelectionOnly);

            // Καθάρισε παλιό highlight
            scope.ClearStyle(style);

            if (string.IsNullOrEmpty(query))
                return 0;

            var rx = BuildRegex(query, opt);

            int count = 0;
            foreach (var r in scope.GetRanges(rx))
            {
                r.SetStyle(style);
                count++;
            }
            return count;
        }

        /// <summary>Βρίσκει το επόμενο (ή προηγούμενο) match. Επιστρέφει true αν το επέλεξε.</summary>
        public static bool FindNext(FastColoredTextBox tb, string query, FindOptions opt)
        {
            if (string.IsNullOrEmpty(query))
                return false;

            var rx = BuildRegex(query, opt);
            var scope = GetScope(tb, opt.SearchSelectionOnly);

            // Υπολογισμοί σε απόλυτες θέσεις
            int startPos = tb.PlaceToPosition(tb.Selection.Start);
            int scopeStart = tb.PlaceToPosition(scope.Start);
            int scopeEnd = tb.PlaceToPosition(scope.End);

            if (opt.Forward)
            {
                if (TryFindInRange(tb, rx, Math.Max(startPos, scopeStart), scopeEnd, out var r1))
                { tb.Selection = r1; tb.DoSelectionVisible(); return true; }

                if (opt.Wrap && TryFindInRange(tb, rx, scopeStart, Math.Min(startPos, scopeEnd), out var r2))
                { tb.Selection = r2; tb.DoSelectionVisible(); return true; }
            }
            else
            {
                if (TryFindLastInRange(tb, rx, scopeStart, Math.Min(startPos, scopeEnd), out var r1))
                { tb.Selection = r1; tb.DoSelectionVisible(); return true; }

                if (opt.Wrap && TryFindLastInRange(tb, rx, Math.Max(startPos, scopeStart), scopeEnd, out var r2))
                { tb.Selection = r2; tb.DoSelectionVisible(); return true; }
            }

            return false;
        }

        /// <summary>Αντικαθιστά το τρέχον selection, αν ταιριάζει με το query (σύμφωνα με options).</summary>
        public static bool ReplaceCurrent(FastColoredTextBox tb, string query, string replacement, FindOptions opt)
        {
            if (tb.Selection.IsEmpty) return false;

            var rx = BuildRegex(query, opt);
            var selText = tb.Selection.Text;
            if (!rx.IsMatch(selText)) return false;

            string newText = opt.UseRegex ? rx.Replace(selText, replacement) : replacement;
            tb.InsertText(newText);
            return true;
        }

        /// <summary>Replace All στο scope (ολόκληρο doc ή μόνο selection).</summary>
        public static int ReplaceAll(FastColoredTextBox tb, string query, string replacement, FindOptions opt)
        {
            if (string.IsNullOrEmpty(query)) return 0;

            var rx = BuildRegex(query, opt);
            var scope = GetScope(tb, opt.SearchSelectionOnly);

            // Συλλέγουμε πρώτα όλα τα ranges
            var matches = scope.GetRanges(rx);
            var list = new System.Collections.Generic.List<FastColoredTextBoxNS.Range>(matches);

            // Αντικατάσταση από το τέλος προς την αρχή για να μην αλλάζουν offsets
            list.Sort((a, b) =>
            {
                int pa = tb.PlaceToPosition(a.Start);
                int pb = tb.PlaceToPosition(b.Start);
                return pb.CompareTo(pa);
            });

            int counter = 0;
            foreach (var r in list)
            {
                tb.Selection = r;
                var text = r.Text;
                string newText = opt.UseRegex ? rx.Replace(text, replacement) : replacement;
                tb.InsertText(newText);
                counter++;
            }

            return counter;
        }

        // -------- Helpers --------

        private static bool TryFindInRange(FastColoredTextBox tb, Regex rx, int absStart, int absEnd, out FastColoredTextBoxNS.Range found)
        {
            found = default!;
            if (absStart > absEnd) return false;

            string text = tb.Text;
            if (absEnd > text.Length) absEnd = text.Length;
            if (absStart < 0) absStart = 0;

            var segment = text.Substring(absStart, absEnd - absStart);
            var m = rx.Match(segment);
            if (!m.Success) return false;

            var sp = tb.PositionToPlace(absStart + m.Index);
            var ep = tb.PositionToPlace(absStart + m.Index + m.Length);
            found = new FastColoredTextBoxNS.Range(tb, sp, ep);
            return true;
        }

        private static bool TryFindLastInRange(FastColoredTextBox tb, Regex rx, int absStart, int absEnd, out FastColoredTextBoxNS.Range found)
        {
            found = default!;
            if (absStart > absEnd) return false;

            string text = tb.Text;
            if (absEnd > text.Length) absEnd = text.Length;
            if (absStart < 0) absStart = 0;

            var segment = text.Substring(absStart, absEnd - absStart);
            Match last = null!;
            for (var m = rx.Match(segment); m.Success; m = m.NextMatch())
                last = m;

            if (last == null) return false;

            var sp = tb.PositionToPlace(absStart + last.Index);
            var ep = tb.PositionToPlace(absStart + last.Index + last.Length);
            found = new FastColoredTextBoxNS.Range(tb, sp, ep);
            return true;
        }
    }
}
