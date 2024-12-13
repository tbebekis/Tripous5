namespace Tripous.Data
{


    /// <summary>
    /// Used internally by a <see cref="CodeProvider"/> in producing unique Codes.
    /// </summary>
    public class CodeProviderPart
    {
        /// <summary>
        /// Constructor.
        /// <para>Parses the specified part text and recognizes the <see cref="CodeProviderPartType"/> of this part. </para>
        /// </summary>
        public CodeProviderPart(string PartText)
        {
            PartText = PartText.Trim();
            string[] SubParts;

            if (PartText.StartsWith("SELECT ", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Type = CodeProviderPartType.NumericSelect;
                SubParts = PartText.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                this.Text = SubParts[0];
                if (SubParts.Length >= 2)
                    this.Format = SubParts[1];
            }
            else if (PartText.StartsWith("SEQUENCER", StringComparison.InvariantCultureIgnoreCase))
            {
                this.Type = CodeProviderPartType.Sequencer;
                SubParts = PartText.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                this.Text = SubParts[1];
                if (SubParts.Length >= 3)
                    this.Format = SubParts[2];
            }
            else
            {

                List<char> Letters = new List<char>();
                foreach (char C in PartText)
                {
                    if (char.IsLetter(C) && !Letters.Contains(char.ToUpperInvariant(C)))
                    {
                        Letters.Add(char.ToUpperInvariant(C));
                    }
                }

                this.Text = PartText;

                if (Letters.TrueForAll(c => c == CodeProvider.ValidCodeFormatDigit))
                {
                    this.Type = CodeProviderPartType.Pivot;
                    this.Format = PartText;
                }
                else if (Letters.TrueForAll(c => "YMD".IndexOf(c) != -1))
                {
                    this.Type = CodeProviderPartType.Date;
                }
                else
                {
                    this.Type = CodeProviderPartType.Literal;
                }


                // check valid separators
                if (this.Type == CodeProviderPartType.Pivot || this.Type == CodeProviderPartType.Date)
                {
                    foreach (char C in this.Text)
                    {
                        if (!char.IsLetter(C))
                        {
                            if (!CodeProvider.ValidSeparators.Contains(C))
                                Sys.Throw($@"'{C}' is not a valid separator character of a Code Part of a {nameof(CodeProvider)}");
                        }
                    }
                }

            }

        }

        /* properties */
        /// <summary>
        /// The type of the part.
        /// </summary>
        public CodeProviderPartType Type { get; }
        /// <summary>
        /// The part text.
        /// <para>The meaning of the Text is determined by the value of the <see cref="Type"/>.</para>
        /// <para>When the mode is <see cref="CodeProviderPartType.NumericSelect"/> the Text SHOULD contain
        /// the @TABLE_NAME placeholder which is then replaced by the value of the <see cref="CodeProvider"/> TableName</para>
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// The part format. Not all parts have a format.
        /// </summary>
        public string Format { get; }
    }
}
