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
    /// Generates the text for the data-setup attribute of a control.
    /// <para>NOTE: The data-setup of a control has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
    /// </summary>
    public class ViewControlDataSetup
    {
        JObject JSetup;
        JObject JControl;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDataSetup()
        {
            JSetup = new JObject();
            JSetup["Text"] = Sys.None;

            JControl = new JObject();
            JSetup["Control"] = JControl;

            JControl["TypeName"] = "TextBox";

            // <div class="tp-CtrlRow" data-setup="{Text: 'Code', Control: { TypeName: 'TextBox', DataField: 'Code' } }"></div>
            // <div class="tp-CtrlRow" data-setup="{Text: 'Test', Control: { TypeName: 'ComboBox', DataField: '', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0} }"></div>

        }

        /* static */
        /// <summary>
        /// Generates the text for the data-setup attribute of a control based on a specified <see cref="ViewControlDef"/> and optionally a <see cref="SqlBrokerFieldDef"/>.
        /// </summary>
        static public string GetSetupText(ViewControlDef ControlDef, SqlBrokerFieldDef FieldDef = null)
        {
            ViewControlDataSetup Result = new ViewControlDataSetup();

            if (FieldDef != null)
            {
                Result.Text = FieldDef.Title;

                Result.SetControlProperty("TypeName", ViewControlDef.GetTypeName(FieldDef));
                Result.SetControlProperty("SourceName", ControlDef.TableName);
                Result.SetControlProperty("DataField", FieldDef.Name);
                Result.SetControlProperty("ReadOnly", FieldDef.IsReadOnly);
                Result.SetControlProperty("Required", FieldDef.IsRequired); 
            }
            else
            {
                Result.Text = FieldDef.Title;

                Result.SetControlProperty("TypeName", ControlDef.TypeName);
                Result.SetControlProperty("SourceName", ControlDef.TableName);
                Result.SetControlProperty("DataField", ControlDef.DataField);
                Result.SetControlProperty("ReadOnly", ControlDef.ReadOnly);
                Result.SetControlProperty("Required", ControlDef.Required);
            }

            return Result.GetSetupText();
        }
 
        /* public */
        /// <summary>
        /// Get a <see cref="JToken"/> with the value of a specified property
        /// </summary>
        public JToken GetControlProperty(string Name)
        {
            return JControl.ContainsKey(Name) ? JControl[Name] : null;
        }
        /// <summary>
        /// Sets the value of a specified property. The second parameter, the <see cref="JToken"/>, could be a value of any type.
        /// </summary>
        public void SetControlProperty(string Name, JToken Value)
        {
            JControl[Name] = Value;
        }
        /// <summary>
        /// Returns the text of this setup.
        /// </summary>
        public string GetSetupText()
        {
            return JSetup.ToString();
        }


        /* properties */
        /// <summary>
        /// Gets or sets the "Text" part of this setup.
        /// </summary>
        private string Text
        {
            get { return JSetup["Text"].ToString(); }
            set { JSetup["Text"] = value; }
        }
    }
}
