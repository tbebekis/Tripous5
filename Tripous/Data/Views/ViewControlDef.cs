using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 
    /// <summary>
    /// Represents a row in a column.
    /// <para>The row is the control container along with its caption text.</para>
    /// <para>The control may be data-bindable or not.</para>
    /// </summary>
    public class ViewControlDef: ViewDefComponent
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string TextBox = "TextBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string CheckBox = "CheckBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string DateBox = "DateBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string ComboBox = "ComboBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string ListBox = "ListBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string Memo = "Memo";
        /// <summary>
        /// Constant
        /// </summary>
        public const string LocatorBox = "LocatorBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string NumberBox = "NumberBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string Grid = "Grid"; 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef(SqlBrokerFieldDef Field)
        {
            Title = Field.Title;
            TitleKey = Field.TitleKey;

            DataField = Field.Name;

            ReadOnly = Field.IsReadOnly;
            Required = Field.IsRequired;
            TypeName = GetTypeName(Field);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef(string TypeName, string TitleKey, string DataField = "", string TableName = "", object Properties = null)
        {
            this.TypeName = TypeName;
            this.TitleKey = TitleKey;
            this.DataField = DataField;
            this.TableName = TableName;

            if (Properties != null)
            {
                Type T = Properties.GetType();
                var PropList = T.GetProperties();

                foreach (var Prop in PropList)
                {
                    var PropName = Prop.Name;
                    var Value = Prop.GetValue(Properties);
                    this.Properties[PropName] = Value;
                }
            }

            if (this.TypeName == Grid && !this.Properties.ContainsKey("Width"))
                this.Properties["Width"] = "100%";
        }

        /* static */
        /// <summary>
        /// Returns the 'TypeName' of a field definition, e.g. 'TextBox', 'CheckBox', etc.
        /// </summary>
        static public string GetTypeName(SqlBrokerFieldDef Field)
        {
            switch (Field.DataType)
            {
                case DataFieldType.String:
                    return Bf.In(FieldFlags.Memo, Field.Flags) ? Memo : TextBox;
  

                case DataFieldType.Integer:
                    return Bf.In(FieldFlags.Boolean, Field.Flags) ? CheckBox : NumberBox;
 
                case DataFieldType.Float:
                case DataFieldType.Decimal:
                    return NumberBox;
 
                case DataFieldType.Date:
                case DataFieldType.DateTime:
                    return DateBox;  
                case DataFieldType.Boolean:
                    return CheckBox;
 
                case DataFieldType.Memo:
                    return Memo;

            }

            return TextBox;
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Title;
        }
        /// <summary>
        /// Assigns properties of this instance from a specified <see cref="SqlBrokerFieldDef"/>
        /// </summary>
        public void AssignField(SqlBrokerFieldDef FieldDef = null)
        {
            if (FieldDef != null)
            {
                if (!string.IsNullOrWhiteSpace(FieldDef.Title))
                    this.Title = FieldDef.Title;

                if (!string.IsNullOrWhiteSpace(ViewControlDef.GetTypeName(FieldDef)))
                    this.TypeName = ViewControlDef.GetTypeName(FieldDef);

                if (!string.IsNullOrWhiteSpace(FieldDef.Name))
                    this.DataField = FieldDef.Name;

                if (FieldDef.IsReadOnly)
                    this.ReadOnly = FieldDef.IsReadOnly;

                if (FieldDef.IsRequired)
                    this.Required = FieldDef.IsRequired;
            }
        }
        /// <summary>
        /// Assigns properties to a data-setup object
        /// </summary>
        public override void AssignTo(Dictionary<string, object> DataSetup)
        {
            if (!string.IsNullOrWhiteSpace(TypeName))
                DataSetup["TypeName"] = TypeName;

            if (!string.IsNullOrWhiteSpace(DataField))
                DataSetup["DataField"] = DataField;

            if (!string.IsNullOrWhiteSpace(Id))
                DataSetup["Id"] = Id;

            if (ReadOnly)
                DataSetup["ReadOnly"] = ReadOnly;

            if (Required)
                DataSetup["Required"] = Required;

            foreach (var Entry in Properties)
                DataSetup[Entry.Key] = Entry.Value;
        }
 
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public override string  GetDataSetupText()
        {
            // <div class="tp-CtrlRow" data-setup="{Text: 'Code', Control: { TypeName: 'TextBox', DataField: 'Code' } }"></div>
            // <div class="tp-CtrlRow" data-setup="{Text: 'Test', Control: { TypeName: 'ComboBox', DataField: '', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0} }"></div>

            Dictionary<string, object> Result = null;

            if (TypeName == ViewControlDef.Grid)
            {
                Result = this.Properties;
            }
            else
            {
                Result = new Dictionary<string, object>();
                Dictionary<string, object> Control = new Dictionary<string, object>();
                AssignTo(Control);

                Result["Text"] = Title;
                Result["Control"] = Control;
            }


            string JsonText = Json.Serialize(Result);
            return JsonText;
        }


        /* properties */
        /// <summary>
        /// The HTML Id and HTML Name of the control.
        /// <para>The name of a desktop control.</para>
        /// </summary>
        public string Id { get; set; } = "";
   
        /// <summary>
        /// Indicates the control type, such as TextBox
        /// </summary>
        public string TypeName { get; set; } = TextBox;
        /// <summary>
        /// When true the control is readonly
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// When true the control must have a value
        /// </summary>
        public bool Required { get; set; } 
 
        /// <summary>
        /// The data field to bind
        /// </summary>
        public string DataField { get; set; } = "";


 
    }


    /// <summary>
    /// Extensions
    /// </summary>
    static public class ViewControlDefExtensions
    {


        /// <summary>
        /// Adds and returns a <see cref="ViewControlDef"/>
        /// </summary>
        static public ViewControlDef Add(this List<ViewControlDef> Controls, string TypeName, string TitleKey, string DataField = "", string TableName = "", object Properties = null)
        {
            ViewControlDef Result = new ViewControlDef(TypeName, TitleKey, DataField, TableName, Properties);
            Controls.Add(Result);
            return Result;
        }
    }


}
