/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{

    /// <summary>
    /// A list of <see cref="CodeDescriptor"/> items.
    /// </summary>
    public class CodeDescriptors : ModelItems<CodeDescriptor>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeDescriptors()
        {
            UseSafeAdd = true;
        }

        /// <summary>
        /// Adds a code producer descriptor to the list.
        /// <para>It configures automatically the pivot and the prefix part.</para>
        /// <para>The pivot part mode is <see cref="CodePartMode.FieldName"/> and the prefix part mode is <see cref="CodePartMode.Literal"/></para>
        /// </summary>
        public CodeDescriptor Add(string Name, string PivotFieldName, string PivotFormat, string ProducerClassName, string PrefixLiteral)
        {
            CodeDescriptor Result = base.Add(Name);

            Result.TypeClassName = ProducerClassName;

            Result.Pivot.Mode = CodePartMode.FieldName;
            Result.Pivot.Text = PivotFieldName;
            Result.Pivot.Format = PivotFormat;

            Result.Prefix.Mode = CodePartMode.Literal;
            Result.Prefix.Text = PrefixLiteral;

            return Result;
        }
        /// <summary>
        /// Adds a code producer descriptor to the list.
        /// <para>It configures automatically the pivot and the prefix part.</para>
        /// <para>The pivot part mode is <see cref="CodePartMode.FieldName"/> and the prefix part mode is <see cref="CodePartMode.Literal"/></para>
        /// </summary>
        public CodeDescriptor Add(string Name, string PivotFieldName, string PivotFormat)
        {
            return Add(Name, PivotFieldName, PivotFormat, "", "");
        }
    }

}
