using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{


    /// <summary>
    /// Describes the production of a unique Code.
    /// <para>See the <see cref="CodeProviderPartType"/> enum for more information. </para>
    /// </summary>
    [TypeStoreItem]
    public class CodeProviderDef
    {
        /// <summary>
        /// A code producer that produces a code of the form: XXXX
        /// </summary>
        public const string Simple4 = "SIMPLE XXXX";
        /// <summary>
        /// A code producer that produces a code of the form: XXXXXX
        /// </summary>
        public const string Simple6 = "SIMPLE XXXXXX";

        /// <summary>
        /// A code producer that produces a code of the form: XX-XX
        /// </summary>
        public const string Simple4_2 = "SIMPLE XX-XX";
        /// <summary>
        /// A code producer that produces a code of the form: XXX-XXX
        /// </summary>
        public const string Simple6_3 = "SIMPLE XXX-XXX";


        static List<CodeProviderDef> Descriptors = new List<CodeProviderDef>();

        static void RegisterSysCodeProviders()
        {
            RegisterDescriptor(Simple4, "XXXX");
            RegisterDescriptor(Simple6, "XXXXXX");

            RegisterDescriptor(Simple4_2, "XX-XX");
            RegisterDescriptor(Simple6_3, "XXX-XXX");
        }

        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static CodeProviderDef()
        {

        }
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
        /// Registers a descriptor. If it finds a descriptor returns the already registered descriptor.
        /// </summary>
        static public CodeProviderDef RegisterDescriptor(string Name, string Text)
        {
            CodeProviderDef Result = FindDescriptor(Name);
            
            if (Result == null)
            {
                Result = new CodeProviderDef() { Name = Name, Text = Text };
                Descriptors.Add(Result);
            }

            return Result;
        }

        /// <summary>
        /// Creates and returns an instance of a <see cref="CodeProvider"/> based on a specified descriptor.
        /// </summary>
        static public CodeProvider Create(string DescriptorName, string TableName)
        {
            return Create(FindDescriptor(DescriptorName), TableName);
        }
        /// <summary>
        /// Creates and returns an instance of a <see cref="CodeProvider"/> based on a specified descriptor.
        /// </summary>
        static public CodeProvider Create(CodeProviderDef Descriptor, string TableName)
        {
            if (Descriptor == null)
                Sys.Throw($"Cannot create a {nameof(CodeProvider)}. Descriptor is null.");

            CodeProvider Result = TypeStore.Create(Descriptor.TypeClassName) as CodeProvider;
            Result.Descriptor = Descriptor;
            Result.TableName = TableName;
            return Result;
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

        /// <summary>
        /// Gets or sets the class name of the <see cref="System.Type"/> this descriptor describes.
        /// <para>NOTE: The valus of this property may be a string returned by the <see cref="Type.AssemblyQualifiedName"/> property of the type. </para>
        /// <para>In that case, it consists of the type name, including its namespace, followed by a comma, followed by the display name of the assembly
        /// the type belongs to. It might looks like the following</para>
        /// <para><c>Tripous.Forms.BaseDataEntryForm, Tripous, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</c></para>
        /// <para></para>
        /// <para>Otherwise it must be a type name registered to the <see cref="TypeStore"/> either directly or
        /// just by using the <see cref="TypeStoreItemAttribute"/> attribute.</para>
        /// <para>In the case of a type registered with the TypeStore, a safe way is to use a Namespace.TypeName combination
        /// both, when registering and when retreiving a type.</para>
        /// <para></para>
        /// <para>Regarding types belonging to the various Tripous namespaces, using just the TypeName is enough.
        /// Most of the Tripous types are already registered to the TypeStore with just their TypeName.</para>
        /// </summary>
        public string TypeClassName { get; set; } = typeof(CodeProvider).Name;
 
    }




}
