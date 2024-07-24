 

namespace Test.WinApp
{
    static public class SettingTest
    {
 

        [RegisterSchemaFunc(Version: 1)]
        static void RegisterSchema(Schema Schema, SchemaVersion Version)
        {
            string SqlText = Settings.GetTableDef(TableName: "AppSettings");
            Version.AddTable(SqlText);
        }

        static public void LoadSettings()
        {
            Settings.LoadFromTable();

            if (Settings.Items.Count == 0)
            {
                // select list for the Cities setting
                List<SettingSelectItem> SelectList = new List<SettingSelectItem>();
                SelectList.Add(new SettingSelectItem() { Id = "1", TextKey = "Thessaloniki" });
                SelectList.Add(new SettingSelectItem() { Id = "2", TextKey = "Athens" });
                SelectList.Add(new SettingSelectItem() { Id = "3", TextKey = "Rome" });
                SelectList.Add(new SettingSelectItem() { Id = "4", TextKey = "Madrid" });

                // settings
                Settings.Items.Add(new Setting() { Id = "Sales Email", DataType = SettingDataType.String, Value = "sales@company.com"});
                Settings.Items.Add(new Setting() { Id = "Is OK", DataType = SettingDataType.Boolean, Value = true });
                Settings.Items.Add(new Setting() { Id = "Cities", DataType = SettingDataType.MultiSelect, Value = new string[] { "2", "3"}, SelectList = SelectList });

                Settings.SaveToTable();
                Settings.SaveToFile("AppSettings.json");
            }
            else
            {
                Settings.SaveToTable();
                Settings.SaveToFile("AppSettings.json");
            }

        }

        /* properties */
        static public Settings Settings { get; set; } = new Settings();
    }
}
