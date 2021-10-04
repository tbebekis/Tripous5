using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using Tripous;
using Tripous.Data;

namespace WebLib
{
    public class DataViewControlDataSetup
    {
        JObject Setup;
        JObject Control;


        public DataViewControlDataSetup()
        {
            Setup = new JObject();
            Setup["Text"] = Sys.None;

            Control = new JObject();
            Setup["Control"] = Control;

            Control["TypeName"] = "TextBox";

            // <div class="tp-CtrlRow" data-setup="{Text: 'Code', Control: { TypeName: 'TextBox', DataField: 'Code' } }"></div>
            // <div class="tp-CtrlRow" data-setup="{Text: 'Test', Control: { TypeName: 'ComboBox', DataField: '', Mode: 'ListOnly', ListValueField: 'Id', ListDisplayField: 'Name', List: [{Id: 100, Name: 'All'}, {Id: 0, Name: 'No stops'}, {Id:1, Name: '1 stop'}], SelectedIndex: 0} }"></div>

        }

        static public string GetSetupText(ViewControlDef ControlDef, SqlBrokerFieldDef FieldDef = null)
        {
            DataViewControlDataSetup Result = new DataViewControlDataSetup();

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



        public JToken GetControlProperty(string Name)
        {
            return Control.ContainsKey(Name) ? Control[Name] : null;
        }
        public void SetControlProperty(string Name, JToken Value)
        {
            Control[Name] = Value;
        }
        public string GetSetupText()
        {
            return Setup.ToString();
        }

        public string Text
        {
            get { return Setup["Text"].ToString(); }
            set { Setup["Text"] = value; }
        }
    }
}
