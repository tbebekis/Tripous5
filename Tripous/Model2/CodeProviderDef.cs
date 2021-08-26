using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Model2
{


    /// <summary>
    /// Describes the production of a unique Code.
    /// <para>See the <see cref="CodeProviderPartType"/> enum for more information. </para>
    /// </summary>
    public class CodeProviderDef
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeProviderDef()
        {
        }

        /* properties */
        /// <summary>
        /// A unique name for this instance
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The definition text.
        /// <para>See the <see cref="CodeProviderPartType"/> enum for more information. </para>
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The field name of the field to put the produced Code.
        /// </summary>
        public string CodeFieldName { get; set; } = "Code";
        /// <summary>
        /// A character that used in separating the parts of the produced Code.
        /// </summary>
        public char PartSeparator { get; set; } = '-';
    }




}
