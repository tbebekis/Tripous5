namespace Tripous
{

    /// <summary>
    /// Base class for application settings.
    /// <para>Watches for changes in the settings file and reloads this instance.</para>
    /// <para><strong>NOTE: </strong> If this class contains lists, then override the <see cref="Load()"/> method, 
    /// clean the lists, and after that call the base implementation.</para>
    /// </summary>
    public class AppSettingsBase
    {
        const string SFileName = "AppSettings.json";

        /* protected */
        protected string Folder { get; private set; }
        protected string FileName { get; private set; }        
        protected string FilePath { get; private set; }
        protected FileSystemWatcher Watcher { get; private set; }
        protected bool IsSaving { get; private set; }
        protected bool IsReloadable { get; private set; }

        /// <summary>
        /// event handler
        /// </summary>
        protected void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsSaving)
            {
                string S = e.FullPath.ToLower();
                string S2 = FilePath.ToLower();

                if (S == S2)
                    Load();
            }
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
        protected AppSettingsBase(string Folder = "", string FileName = "", bool IsReloadable = true)
        { 
            if (string.IsNullOrWhiteSpace(Folder))
                Folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            if (string.IsNullOrWhiteSpace(FileName))
                FileName = SFileName;

            this.Folder = Folder;
            this.FileName = FileName;
            this.IsReloadable = IsReloadable;

            this.FilePath = Path.Combine(this.Folder, this.FileName);

            if (!File.Exists(this.FilePath))
            {
                if (!Directory.Exists(this.Folder))
                    Directory.CreateDirectory(this.Folder);
                Save();
            }

            Load();

            if (IsReloadable)
            {
                Watcher = new FileSystemWatcher(this.Folder);
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.Filter = this.FileName;
                Watcher.EnableRaisingEvents = true;
                Watcher.Changed += Watcher_Changed;
            }

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
            IsSaving = true;
            try
            {
                Json.SaveToFile(this, FilePath);
            }
            finally
            {
                IsSaving = false;
            }            
        }
    }
}
