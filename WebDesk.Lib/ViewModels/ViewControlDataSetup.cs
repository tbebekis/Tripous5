using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using Tripous;
using Tripous.Data;

namespace WebLib.Models
{

    /// <summary>
    /// Generates the text for the data-setup attribute of a control row.
    /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
    /// </summary>
    public class ViewControlDataSetup
    {

        ViewControlDef ControlDef;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDataSetup()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDataSetup(ViewControlDef ControlDef)
        {
            this.ControlDef = ControlDef;

            this.Text = ControlDef.Title;

            ControlDef.AssignTo(this.Properties); 
        }

        /* static */
        /// <summary>
        /// Generates the text for the data-setup attribute of a control based on a specified <see cref="ViewControlDef"/> and optionally a <see cref="SqlBrokerFieldDef"/>.
        /// </summary>
        static public string GetDataSetupText(ViewControlDef ControlDef, SqlBrokerFieldDef FieldDef = null)
        {
            ViewControlDataSetup Result = new ViewControlDataSetup(ControlDef);

            if (FieldDef != null)
            {
                Result.Text = FieldDef.Title;

                Result.Properties["TypeName"] = ViewControlDef.GetTypeName(FieldDef);

                if (!string.IsNullOrWhiteSpace(FieldDef.Name))
                    Result.Properties["DataField"] = FieldDef.Name;

                if (FieldDef.IsReadOnly)
                    Result.Properties["ReadOnly"] = FieldDef.IsReadOnly;

                if (FieldDef.IsRequired)
                    Result.Properties["Required"] = FieldDef.IsRequired;
            }

            string JsonText =  Result.Serialize();
            return JsonText;
        }
 
        /* public */    
        /// <summary>
        /// Returns the text of this setup.
        /// </summary>
        public string Serialize()
        {
            // <div class="tp-CtrlRow" data-setup="{Text: 'Code', Control: { TypeName: 'TextBox', DataField: 'Code' } }"></div>
            // <div class="tp-CtrlRow" data-setup="{Text: 'Test', Control: { TypeName: 'ComboBox', DataField: '', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0} }"></div>

            Dictionary<string, object> Result = null;

            if (this.ControlDef != null && this.ControlDef.TypeName == ViewControlDef.Grid)
            {
                Result = this.Properties;
            }
            else
            {
                Result = new Dictionary<string, object>();

                Result["Text"] = Text;
                Result["Control"] = Properties;
            }      


            string JsonText = Json.Serialize(Result);
            return JsonText;
        }


        /* properties */
        /// <summary>
        /// Gets or sets the "Text" part of this setup.
        /// </summary>
        private string Text { get; set; }

        /// <summary>
        /// Custom properties helper.
        /// <para>Custom properties for more properties of the Control part of the data-setup attribute.</para>
        /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }
        /// <summary>
        /// Dictionary with custom properties.
        /// <para>Custom properties for more properties of the Control part of the data-setup attribute.</para>
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }
}
