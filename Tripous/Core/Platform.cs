namespace Tripous
{
    /// <summary>
    /// Platform information helper
    /// </summary>
    static public class Platform
    {


        /* construction */
        /// <summary>
        /// Static constuctor
        /// </summary>
        static Platform()
        {
        }

 

        /* properties */
        /// <summary>
        /// True if this is a 32-bit system
        /// </summary>
        static public bool IsBit32 { get; private set; } = !Environment.Is64BitProcess;
        /// <summary>
        /// True if this is a 64-bit system
        /// </summary>
        static public bool IsBit64 { get; private set; } = Environment.Is64BitProcess;

        /// <summary>
        /// True if is a MS Windows platform (Win32-64, Web, Compact)
        /// </summary>
        static public bool IsWindows { get; private set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// True if is a Unix platform (Linux with Mono)
        /// </summary>
        static public bool IsLinux { get; private set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        /// <summary>
        /// True if is a MacOSX platform
        /// </summary>
        static public bool IsMacOSX { get; private set; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        /// <summary>
        /// True if is a Xbox platform
        /// </summary>
        static public bool IsXbox { get; private set; } = Environment.OSVersion.Platform == PlatformID.Xbox;

        /// <summary>
        /// True if is a WinForms desktop application on a normal Pc (Pc only, NOT Compact Framework) 
        /// </summary>
        static public bool IsDesktop { get; set; }
        /// <summary>
        /// True if is a web (ASP or MVC) application
        /// </summary>
        static public bool IsWeb { get; set; }
        /// <summary>
        /// A Windows Service (running under Service Manager) or a Unix deamon.
        /// </summary>
        static public bool IsService { get; set; }
        /// <summary>
        /// A console application
        /// </summary>
        static public bool IsConsole { get; set; }

        /// <summary>
        /// True if is a WinForms Compact Framework application
        /// </summary>
        static public bool IsCompact { get; set; }
        /// <summary>
        /// True if is a PocketPc platform
        /// </summary>
        static public bool IsPocketPC { get; set; }
        /// <summary>
        /// True if is a Smartphone platform
        /// </summary>
        static public bool IsSmartphone { get; set; }
        /// <summary>
        /// True if is a WinCe generic platform
        /// </summary>
        static public bool IsWinCEGeneric { get; set; }

        /// <summary>
        /// True if the device is a phone
        /// </summary>
        static public bool IsPhone { get; set; }
        /// <summary>
        /// True if the application is run using the emulator
        /// </summary>
        static public bool IsEmulator { get; set; }
        /// <summary>
        /// True if the PDA system has a touch screen
        /// </summary>
        static public bool IsTouchScreen { get; set; }

        /// <summary>
        /// True if is a WinForms application (Desktop or Compact Framework)
        /// </summary>
        static public bool IsWinForms { get { return IsCompact || IsDesktop; } }
    }
}
