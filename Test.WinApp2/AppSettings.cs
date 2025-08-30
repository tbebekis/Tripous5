using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Tripous;

namespace Test.WinApp2
{
    public class AppSettings
    {
        const string SFileName = "AppSettings.json";
        readonly string SFilePath = Path.Combine(AppContext.BaseDirectory, SFileName);

        public AppSettings()
        {
            Load();
        }

        public void Load()
        {
            if (File.Exists(SFilePath))
                Json.LoadFromFile(this, SFilePath);
        }

        public void Save()
        {
            Json.SaveToFile(this, SFilePath);
        }

        
        public string LastProject { get; set; } = "Story";
        public bool AutoSave { get; set; } = true;
        public int AutoSaveSecondsInterval { get; set; } = 30;
        public Part Part { get; set; } = new();
        public List<string> List { get; set; } = new() { "one", "two", "three" };
    }

    public class Part
    {
        public string Name { get; set; } = "Oti nanai re file";
    }


}
