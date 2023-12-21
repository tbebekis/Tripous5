using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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


        static List<CodeProviderDef> RegistryList = new List<CodeProviderDef>();

        static void RegisterSysCodeProviders()
        {
            Register(Simple4, "XXXX");
            Register(Simple6, "XXXXXX");

            Register(Simple4_2, "XX-XX");
            Register(Simple6_3, "XXX-XXX");
        }

        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static CodeProviderDef()
        {
            RegisterSysCodeProviders();
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
        static public CodeProviderDef Find(string Name)
        {
            return RegistryList.Find(item => item.Name.IsSameText(Name));
        }
        /// <summary>
        /// Returns true if a descriptor is already registered under a specified name.
        /// </summary>
        static public bool Contains(string Name)
        {
            return Find(Name) != null;
        }

        /// <summary>
        /// Registers a descriptor. If it finds a descriptor returns the already registered descriptor.
        /// <para>The <see cref="CodeProviderDef"/> decriptor provides information regarding the parts that make up a Code, passing a definition text to a <see cref="CodeProvider"/>.</para>
        /// <para>The text that is passed to a <see cref="CodeProvider"/> by its <see cref="CodeProviderDef"/> descriptor comprises of parts, separated by the pipe | character.</para>     
        /// <para>This <see cref="CodeProviderPartType"/> enum type indicates how to handle a part of that text.</para>
        /// <para>CAUTION: The last part could be a <see cref="CodeProviderPartType.NumericSelect"/>, a <see cref="CodeProviderPartType.Sequencer"/> or a <see cref="CodeProviderPartType.Pivot"/> part. </para>
        /// <para>CAUTION: The <see cref="CodeProviderPartType.Pivot"/> part value is incremented by the code producer. </para>
        /// <para>CAUTION: No more than a single <see cref="CodeProviderPartType.Pivot"/> part is allowed. </para>
        /// <para>Examples of text:</para>
        /// <list type="bullet" >
        /// <item><term>SO|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Sales Order) and <see cref="CodeProviderPartType.Pivot"/></description></item>
        /// <item><term>SO|select Code from Country where Id = :CountryId|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/>, with <see cref="CodeProviderPartType.NumericSelect"/> and <see cref="CodeProviderPartType.Pivot"/>. 
        /// The <c>:CountryId</c> parameter comes from the <see cref="DataRow"/> passed to code producer.</description></item>
        /// <item><term>SI|Sequencer;MySequencer;XXX-XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Sales Invoice) and <see cref="CodeProviderPartType.Sequencer"/> with format.</description></item>
        /// <item><term>PO|YYYY-MM-DD|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Purchases Order), <see cref="CodeProviderPartType.Date"/> and <see cref="CodeProviderPartType.Pivot"/></description></item>
        /// </list>
        /// </summary>
        static public CodeProviderDef Register(string Name, string Text)
        {
            CodeProviderDef Result = Find(Name);
            
            if (Result == null)
            {
                Result = new CodeProviderDef() { Name = Name, Text = Text };
                RegistryList.Add(Result);
            }

            return Result;
        }

        /// <summary>
        /// Creates and returns an instance of a <see cref="CodeProvider"/> based on a specified descriptor.
        /// </summary>
        static public CodeProvider Create(string DescriptorName, string TableName, string CodeFieldName)
        {
            return Create(Find(DescriptorName), TableName, CodeFieldName);
        }
        /// <summary>
        /// Creates and returns an instance of a <see cref="CodeProvider"/> based on a specified descriptor.
        /// </summary>
        static public CodeProvider Create(CodeProviderDef Descriptor, string TableName, string CodeFieldName)
        {
            if (Descriptor == null)
                Sys.Throw($"Cannot create a {nameof(CodeProvider)}. Descriptor is null.");

            CodeProvider Result = TypeStore.Create(Descriptor.TypeClassName) as CodeProvider;
            Result.Descriptor = Descriptor;
            Result.TableName = TableName;
            Result.CodeFieldName = CodeFieldName;
            return Result;
        }

        /* public */
        /// <summary>
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        { 
            if (string.IsNullOrWhiteSpace(this.Name))
                Sys.Throw(Res.GS("E_CodeProviderDef_NameIsEmpty", "CodeProviderDef Name is empty"));

            if (string.IsNullOrWhiteSpace(this.Text))
                Sys.Throw(Res.GS("E_CodeProviderDef_TextIsEmpty", "CodeProviderDef Text is empty. Must be something like XXX-XXX"));
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
        /// A character that used in separating the parts of the produced Code.
        /// </summary>
        public string PartSeparator { get; set; } = "-";

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
