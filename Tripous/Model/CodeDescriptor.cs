/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tripous.Model
{

    /// <summary>
    /// Describes a Code for the <see cref="CodeProducer"/> to create.
    /// <para>
    /// A Code consists of a Prefix, one or more Parts and lastly a Pivot part, which is the part where the increment takes place.</para>
    /// <para>
    /// The final form of the Code is</para>
    /// <para><c>   [Prefix] [Part][,...n] [Pivot Part] </c></para>
    /// <para>
    /// The Pivot Part is mandatory.</para>
    /// <para>
    /// The string of a Code is constructed by</para>
    /// <list type="bullet"> 
    ///   <description> 1. the Prefix</description>
    ///   <description> 2. then the values of all the Parts that are not Pivot, using the given order</description>
    ///   <description> 3. and lastly the value of the Pivot Part </description>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(CodeDescriptorJsonConverter))]
    public class CodeDescriptor : Descriptor, IList
    {
 
        private List<CodePart> list = new List<CodePart>();
        private string typeClassName;

        #region explicit interface implementations

        int IList.Add(object value)
        {
            if (value is CodePart)
                list.Add(value as CodePart);
            return list.Count;
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value as CodePart);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value as CodePart);
        }

        void IList.Insert(int index, object value)
        {
            if (value is CodePart)
                list.Insert(index, value as CodePart);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            if (value is CodePart)
                list.Remove(value as CodePart);
        }

        void IList.RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                CodePart Value = (value as CodePart);
                if (Value != null)
                    list[index] = Value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            (list as IList).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return (list as ICollection).SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();
            list.Clear();
        }
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoAssign(object Source)
        {
            if (Source is CodeDescriptor)
            {
                CodeDescriptor SourceDes = Source as CodeDescriptor;

                this.Name = SourceDes.Name;
                this.Alias = SourceDes.Alias;
                this.Title = SourceDes.Title;
                this.TitleKey = SourceDes.TitleKey;

                this.Prefix = SourceDes.Prefix.Clone() as CodePart;
                this.Pivot = SourceDes.Pivot.Clone() as CodePart;

                foreach (var SourceItem in SourceDes)
                {
                    Add((SourceItem as CodePart).Clone() as CodePart);
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeDescriptor()
        {
            Prefix.Mode = CodePartMode.Literal;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeDescriptor (string Name, string PivotFieldName, string PivotFormat, string ProducerClassName, string PrefixLiteral)
        {
            this.Name = Name;
            this.TypeClassName = ProducerClassName;

            this.Pivot.Mode = CodePartMode.FieldName;
            this.Pivot.Text = PivotFieldName;
            this.Pivot.Format = PivotFormat;

            this.Prefix.Mode = CodePartMode.Literal;
            this.Prefix.Text = PrefixLiteral; 
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeDescriptor(string Name, string PivotFieldName, string PivotFormat)
            : this(Name, PivotFieldName, PivotFormat, "", "")
        {
        }

        /// <summary>
        /// Adds a <see cref="CodePart"/> to the list
        /// </summary>
        public void Add(CodePart Part)
        {
            list.Add(Part);
        }
        /// <summary>
        /// Adds a <see cref="CodePart"/> to the list
        /// </summary>
        public void Add(CodePartMode Mode, string Text, string Format)
        {
            CodePart Part = new CodePart();
            Part.Mode = Mode;
            Part.Text = Text;
            Part.Format = Format;
            Add(Part);
        }

        /// <summary>
        /// Override
        /// </summary>
        public override object Clone()
        {
            string JsonText = Tripous.Json.ToJson(this);
            CodeDescriptor Result = Tripous.Json.FromJson<CodeDescriptor>(JsonText);
            return Result;
        }

        /// <summary>
        /// Gets the Prefix of the code.
        /// <para>The Prefix is a string value and becomes the prefix of the whole Code.</para>
        /// <para>It is given either as a Literal value </para>
        /// <para>    <c>ABC</c> or <c>YYYY-MM-DD</c></para>
        /// <para>or it is produced by executing a lookup SQL which must result in a SINGLE value.</para>
        /// </summary>
        public CodePart Prefix { get; set; } = new CodePart();
        /// <summary>
        /// Gets the pivot of the code.
        /// </summary>
        public CodePart Pivot { get; set; } = new CodePart();
        /// <summary>
        /// Gets the number of parts in the list.
        /// </summary>
        public int Count { get { return list.Count; } }
        /// <summary>
        /// Gets a part by index
        /// </summary>
        public CodePart this[int Index] { get { return list[Index]; } }
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
        public string TypeClassName
        {
            get { return string.IsNullOrEmpty(typeClassName) ? "CodeProducer" : typeClassName; }
            set { typeClassName = value; }
        }

    }




    /// <summary>
    ///  A custom json converter for the <see cref="CodeDescriptor"/>
    /// </summary>
    internal class CodeDescriptorJsonConverter : JsonConverter
    {
        /// <summary>
        ///  Writes the JSON representation of the object.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            CodeDescriptor v =  value as CodeDescriptor;
            if (v != null)
            {
                JObject JO = new JObject();
                
                dynamic Dyn = JO;

                Dyn.Name = v.Name;
                Dyn.Alias = v.Alias;
                Dyn.Title = v.Title;
                Dyn.TitleKey = v.TitleKey;

                Dyn.Pivot = Tripous.Json.ToDynamic(Tripous.Json.ToJson(v.Pivot));
                Dyn.Prefix = Tripous.Json.ToDynamic(Tripous.Json.ToJson(v.Prefix));

                JArray JItems = new JArray();
                JO.Add("Items", JItems);

                JObject JItem;
                foreach (var Item in v)
                {
                    JItem = JObject.Parse(Tripous.Json.ToJson(Item));
                    JItems.Add(JItem);
                }

                JO.WriteTo(writer);
 
            }
            
        }
        /// <summary>
        /// Reads the JSON representation of the object. Returns the object value.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject JO = JObject.Load(reader);
            dynamic Dyn = JO;

            CodeDescriptor v = new CodeDescriptor();
            v.Name = Dyn.Name;
            v.Alias = Dyn.Alias;
            v.Title = Dyn.Title;
            v.TitleKey = Dyn.TitleKey;

            v.Pivot = Tripous.Json.FromJson<CodePart>(Dyn.Pivot.ToString());
            v.Prefix = Tripous.Json.FromJson<CodePart>(Dyn.Prefix.ToString());

            JArray JItems = Dyn.Items as JArray;
            foreach (var Item in JItems)
            {
                v.Add(Tripous.Json.FromJson<CodePart>(Item.ToString()));
            }

            return v;

        }
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CodeDescriptor);
        }

        /* properties */
        /// <summary>
        /// Gets a value indicating whether this instance can read JSON.
        /// </summary>
        public override bool CanRead { get { return true; } }
        /// <summary>
        /// Gets a value indicating whether this instance can write JSON.
        /// </summary>
        public override bool CanWrite { get { return true; } }
    }

}
