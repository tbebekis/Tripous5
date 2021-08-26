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
        static List<CodeProviderDef> Descriptors = new List<CodeProviderDef>();

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeProviderDef()
        {
        }

        /* static */
        /// <summary>
        /// Returns a descriptor by a specified name if any, else, null
        /// </summary>
        static public CodeProviderDef FindDescriptor(string Name)
        {
            return Descriptors.Find(item => item.Name.IsSameText(Name));
        }
        /// <summary>
        /// Returns true if a descriptor is already registered under a specified name.
        /// </summary>
        static public bool DescriptorExists(string Name)
        {
            return FindDescriptor(Name) != null;
        }
        /// <summary>
        /// Registers a descriptor.
        /// </summary>
        static public void RegisterDescriptor(string Name, string Text)
        {
            CodeProviderDef Des = FindDescriptor(Name);
            if (Des != null)
            {
                Des.Text = Text;
            }
            else
            {
                Descriptors.Add(new CodeProviderDef() { Name = Name, Text = Text });
            }
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
