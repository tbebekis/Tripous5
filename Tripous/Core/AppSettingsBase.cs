using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous
{

    /// <summary>
    /// Base class for application settings.
    /// <para>Watches for changes in the settings file and reloads this instance.</para>
    /// </summary>
    public class AppSettingsBase
    {
        const string SFileName = "AppSettings.json";

        /* protected */
        protected string Folder { get; private set; }
        protected string FileName { get; private set; }        
        protected string FilePath { get; private set; }
        protected FileSystemWatcher Watcher { get; private set; }

        /// <summary>
        /// event handler
        /// </summary>
        protected void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string S = e.FullPath.ToLower();
            string S2 = FilePath.ToLower();

            if (S == S2)
                Load();
        }


        /* construction */
        /// <summary>
        /// Private constructor
        /// </summary>
        AppSettingsBase()
        {
        }
        /// <summary>
        /// Constructor.
        /// <para>Writes the file if not exists, and loads the file, so settings are available from the creation of this instance.</para>
        /// <para>Also it sets up a <see cref="FileSystemWatcher"/> to detect changes in the specified file and re-load it.</para>
        /// </summary>
        protected AppSettingsBase(string sFolder = "", string sFileName = "")
        {
            this.Folder = sFolder;
            this.FileName = sFileName;

            
            if (string.IsNullOrWhiteSpace(Folder))
                Folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            if (string.IsNullOrWhiteSpace(FileName))
                FileName = SFileName;

            FilePath = Path.Combine(Folder, FileName);

            if (!File.Exists(FilePath))
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
                Save();
            }

            Load();


            Watcher = new FileSystemWatcher(Folder);
            Watcher.NotifyFilter = NotifyFilters.LastWrite;
            Watcher.Filter = FileName;
            Watcher.EnableRaisingEvents = true;
            Watcher.Changed += Watcher_Changed;
        }


        /// <summary>
        /// Loads this instance's properties from the disk file
        /// </summary>
        public virtual void Load()
        {
            Json.LoadFromFile(this, FilePath);
        }
        /// <summary>
        /// Saves this instance to a disk file
        /// </summary>
        public virtual void Save()
        {
            Json.SaveToFile(this, FilePath);
        }
    }
}
