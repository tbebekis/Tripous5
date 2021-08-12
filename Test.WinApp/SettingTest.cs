using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

namespace Test.WinApp
{
    static public class SettingTest
    {
        static List<Setting> SettingsList;

        [RegisterSchemaFunc(Version: 1)]
        static void RegisterSchema(Schema Schema, SchemaVersion Version)
        {
            string SqlText = Setting.GetTableDef(TableName: "AppSettings");
            Version.AddTable(SqlText);
        }

        static public void LoadSettings()
        {
            SettingsList = Setting.LoadAll();

            if (SettingsList.Count == 0)
            {
                // select list for the Cities setting
                List<SettingSelectItem> SelectList = new List<SettingSelectItem>();
                SelectList.Add(new SettingSelectItem() { Id = 1, ValueKey = "Thessaloniki" });
                SelectList.Add(new SettingSelectItem() { Id = 2, ValueKey = "Athens" });
                SelectList.Add(new SettingSelectItem() { Id = 3, ValueKey = "Rome" });
                SelectList.Add(new SettingSelectItem() { Id = 4, ValueKey = "Madrid" });

                // settings
                SettingsList.Add(new Setting() { Id = "Sales Email", DataType = SettingDataType.String, Value = "sales@company.com"});
                SettingsList.Add(new Setting() { Id = "Is OK", DataType = SettingDataType.Boolean, Value = true });
                SettingsList.Add(new Setting() { Id = "Cities", DataType = SettingDataType.MultiSelect, Value = new int[] { 2, 3}, SelectList = SelectList });

                Setting.SaveAll(SettingsList);
            }

        }
    }
}
