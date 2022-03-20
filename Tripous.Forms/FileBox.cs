/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Tripous.Forms
{

    /// <summary>
    /// File open-save dialob box helper
    /// </summary>
    static public class FileBox
    {
#if !COMPACT
        static private string LastFolder = @"C:\";
#else
        static private string LastFolder = @"\";
#endif

        /// <summary>
        /// 
        /// </summary>
        private static bool Box(FileDialog Dlg, ref string FileName, string Filters, string InitDir, string Title)
        {
            try
            {
                Dlg.Filter = FiltersToStr(Filters);
                if (Dlg.Filter.Length == 0)
                    Dlg.Filter = "All files (*.*)|*.*";

                if (Directory.Exists(InitDir))
                    Dlg.InitialDirectory = InitDir;
                else
                    Dlg.InitialDirectory = Directory.GetCurrentDirectory();

#if !COMPACT
                if (Title.Length > 0)
                    Dlg.Title = Title;
                else
                    Dlg.Title = Application.ProductName;
#endif

                Dlg.FileName = FileName;

                if ((Dlg.ShowDialog() == DialogResult.OK))
                {
                    FileName = Dlg.FileName;
                    return true;
                }
                else
                    return false;

            }
            finally
            {
                if (!string.IsNullOrEmpty(Dlg.FileName))
                    LastFolder = Path.GetDirectoryName(Dlg.FileName);
                Dlg.Dispose();
            }
        }

        /* Common Dialog boxes */
        /// <summary>
        /// Returns as ref parameters the Name of a file filter and the Extension, based on
        /// the passed Filter.
        /// <para>For example if the Filter = "BMP", it returns "Bitmap images" and ".bmp".</para>
        /// </summary>
        static public void FilterToStr(string Filter, ref string Name, ref string Extension)
        {
            Filter = Filter.ToUpper();
            if (Filter.StartsWith("*"))
                Filter = Filter.Remove(1, 1);
            if (Filter.StartsWith("."))
                Filter = Filter.Remove(1, 1);

            switch (Filter)
            {
                case "DBF": Name = "DBF files"; Extension = "*.DBF"; break;
                case "DB": Name = "DB files"; Extension = "*.DB"; break;
                case "INI": Name = "INI files"; Extension = "*.INI"; break;
                case "TEXT": Name = "Text files"; Extension = "*.TXT"; break;
                case "ASCII": Name = "Ascii files"; Extension = "*.TXT;*.ASC;*.INI;*.INF;*.DIZ"; break;
                case "CSV": Name = "Csv files"; Extension = "*.csv"; break;
                case "EXE": Name = "Exe files"; Extension = "*.exe"; break;
                case "DLL": Name = "Dynamic link libraries"; Extension = "*.dll"; break;
                case "EXEC": Name = "Executables"; Extension = "*.exe;*.dll"; break;
                case "BMP": Name = "Bitmap images"; Extension = "*.bmp"; break;
                case "JPG": Name = "Jpeg images"; Extension = "*.jpg;*.jpeg"; break;
                case "JPEG": Name = "Jpeg images"; Extension = "*.jpg;*.jpeg"; break;
                case "GIF": Name = "Gif images"; Extension = "*.gif"; break;
                case "TIFF": Name = "Tif images"; Extension = "*.tif;*.tiff"; break;
                case "TIF": Name = "Tif images"; Extension = "*.tif;*.tiff"; break;
                case "PNG": Name = "Png images"; Extension = "*.png"; break;
                case "EMF": Name = "Emf images"; Extension = "*.emf"; break;
                case "WMF": Name = "Wmf images"; Extension = "*.wmf"; break;
                case "ICO": Name = "Ico images"; Extension = "*.ico"; break;
                case "XML": Name = "Xml files"; Extension = "*.xml"; break;
                case "CPP": Name = "C++ files"; Extension = "*.cpp"; break;
                case "HPP": Name = "C++ files"; Extension = "*.hpp"; break;
                case "PAS": Name = "Pascal files"; Extension = "*.pas"; break;
                case "DFM": Name = "Delphi form files"; Extension = "*.dfm"; break;
                case "DPR": Name = "Delphi project files"; Extension = "*.dpr"; break;
                case "JAVA": Name = "Java files"; Extension = "*.java"; break;
                case "JS": Name = "JavaScript files"; Extension = "*.js"; break;
                case "SQL": Name = "Sql files"; Extension = "*.sql"; break;
                case "VB": Name = "Visual Basic files"; Extension = "*.vb"; break;
                case "VBS": Name = "VBScript files"; Extension = "*.vbs"; break;
                default: Name = "All files"; Extension = "*.*"; break;

            }
        }
        /// <summary>
        /// Returns a filter string to be used with FileDialog.Filter property. 
        /// <para>Values is a group of semicolon delimited filters, i.e. "BMP;JPG;JPEG" </para>
        /// </summary>
        static public string FiltersToStr(string Values)
        {
            string[] Filters = Values.Split(';');

            string Name = "";
            string Extension = "";
            string Result = "";

            foreach (string S in Filters)
            {
                FilterToStr(S, ref Name, ref Extension);
                if (Result.Length > 0)
                    Result = Result + "|";

                Result = Result + string.Format("{0} ({1})|{1}", Name, Extension);
            }

            return Result;
        }
        /// <summary>
        /// Ensuers that FileName has the Extension.
        /// </summary>
        static public string EnsureExtension(string FileName, string Extension)
        {
            return Path.ChangeExtension(FileName, Extension);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        static public bool Open(ref string FileName)
        {
            return Open(ref FileName, "ALL", LastFolder, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        static public bool Open(ref string FileName, string Filters)
        {
            return Open(ref FileName, Filters, LastFolder, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        static public bool Open(ref string FileName, string Filters, string InitDir)
        {
            return Open(ref FileName, Filters, InitDir, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Filters"></param>
        /// <param name="InitDir"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        static public bool Open(ref string FileName, string Filters, string InitDir, string Title)
        {
            return Box(new OpenFileDialog(), ref FileName, Filters, InitDir, Title);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        static public bool Save(ref string FileName)
        {
            return Save(ref FileName, "ALL", LastFolder, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Filters"></param>
        /// <returns></returns>
        static public bool Save(ref string FileName, string Filters)
        {
            return Save(ref FileName, Filters, LastFolder, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        static public bool Save(ref string FileName, string Filters, string InitDir)
        {
            return Save(ref FileName, Filters, InitDir, "Please select a file");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Filters"></param>
        /// <param name="InitDir"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        static public bool Save(ref string FileName, string Filters, string InitDir, string Title)
        {
            return Box(new SaveFileDialog(), ref FileName, Filters, InitDir, Title);
        }

        /// <summary>
        /// Infers the ImageFormat from the extension of the FileName
        /// </summary>
        static public ImageFormat ExtensionToImageFormat(string FileName)
        {
            string Extension = Path.GetExtension(FileName);
            if (!string.IsNullOrEmpty(Extension))
            {
                Extension = Extension.ToUpper();

                switch (Extension)
                {
                    case ".BMP": return ImageFormat.Bmp;
                    case ".JPG": return ImageFormat.Jpeg;
                    case ".JPEG": return ImageFormat.Jpeg;
                    case ".GIF": return ImageFormat.Gif;
                    case ".PNG": return ImageFormat.Png;
#if !COMPACT
                    case ".EMF": return ImageFormat.Emf;
                    case ".WMF": return ImageFormat.Wmf;
                    case ".ICO": return ImageFormat.Icon;
                    case ".TIFF": return ImageFormat.Tiff;
                    case ".TIF": return ImageFormat.Tiff;
#endif
                }


            }

            return ImageFormat.Bmp;

        }

        /// <summary>
        /// Browse for folder
        /// </summary>
        static public bool GetFolder(ref string Folder, string SelectedPath, string Description, bool ShowNewFolderButton)
        {
            using (FolderBrowserDialog Dlg = new FolderBrowserDialog())
            {
                Dlg.ShowNewFolderButton = ShowNewFolderButton;

                if (!string.IsNullOrEmpty(SelectedPath) && Directory.Exists(SelectedPath))
                    Dlg.SelectedPath = SelectedPath;
                else if (!string.IsNullOrEmpty(Folder) && Directory.Exists(Folder))
                    Dlg.SelectedPath = Folder;

                if (!string.IsNullOrEmpty(Description))
                    Dlg.Description = Description;

                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    Folder = Dlg.SelectedPath;
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Browse for folder
        /// </summary>
        static public bool GetFolder(ref string Folder, string SelectedPath, bool ShowNewFolderButton)
        {
            return GetFolder(ref Folder, SelectedPath, "Please, select a folder", ShowNewFolderButton);
        }
        /// <summary>
        /// Browse for folder
        /// </summary>
        static public bool GetFolder(ref string Folder, bool ShowNewFolderButton)
        {
            return GetFolder(ref Folder, Folder, "Please, select a folder", ShowNewFolderButton);
        }
        /// <summary>
        /// Browse for folder
        /// </summary>
        static public bool GetFolder(ref string Folder)
        {
            return GetFolder(ref Folder, Folder, "Please, select a folder", true);
        }
    }

#if COMPACT
	/// <summary>
	/// Represents a common dialog box that allows the user to choose a folder.
    /// <para>Taken from: http://www.peterfoot.net/FolderBrowserDialogForWindowsCE.aspx</para>
	/// </summary>
	/// <remarks>Not supported on Windows Mobile, and possibly other platforms - throws a PlatformNotSupportedException if API is missing.</remarks>
    [DesignTimeVisible(false)]
    public class FolderBrowserDialog: Component //: CommonDialog
	{

        /* NOTE: It seems that there is some problem when inheriting from CommonDialog. 
                 Trying to add items to the Toolbox results in an error that the
                 FolderBrowserDialog class doesn't implement the abstract Reset()
                 method although that method is not part of the Compact Framework version
                 of the CommonDialog class. 
                 Setting the Component class as the base class removes the error.
         */
        private BrowseInfo info;
		private string folder = string.Empty;

		/// <summary>
		/// Initializes a new instance of the FolderBrowserDialog class.
		/// </summary>
		public FolderBrowserDialog()
		{
			info = new BrowseInfo();
			info.Title = string.Empty;
			InitCommonControls();
		}

		/// <summary>
		/// Runs a common dialog box with a default owner.
		/// </summary>
		/// <returns></returns>
		public DialogResult ShowDialog()
		{
			IntPtr pitemidlist;

			try
			{
				pitemidlist = SHBrowseForFolder(info.ToByteArray());
			}
			catch(MissingMethodException mme)
			{
				throw new PlatformNotSupportedException("Your platform doesn't support the SHBrowseForFolder API",mme);
			}

			if(pitemidlist==IntPtr.Zero)
			{
				return DialogResult.Cancel;
			}

			//maxpath unicode chars
			byte[] buffer = new byte[520];
			bool success = SHGetPathFromIDList(pitemidlist, buffer);
			//get string from buffer
			if(success)
			{
				folder = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);
				
				int nullindex = folder.IndexOf('\0');
				if(nullindex!=-1)
				{
					folder = folder.Substring(0, nullindex);
				}
			}
			LocalFree(pitemidlist);

			return DialogResult.OK;
		}

		/// <summary>
		/// Gets the path selected by the user.
		/// </summary>
		public string SelectedPath
		{
			get
			{
				return folder;
			}
            set
            {
                info.FileName = value;
            }
		}
		/// <summary>
		/// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
		/// </summary>
		public string Description
		{
			get
			{
				return info.Title;
			}
			set
			{
				info.Title = value;
			}
		}
        /// <summary>
        /// Does nothing
        /// </summary>
        public bool ShowNewFolderButton { get; set; }


    #region P/Invokes

		[DllImport("commctrl", SetLastError=true)]
		private static extern void InitCommonControls();

		[DllImport("ceshell", SetLastError=true)]
		private static extern IntPtr SHBrowseForFolder(byte[] lpbi);

		[DllImport("ceshell", SetLastError=true)]
		private static extern bool SHGetPathFromIDList(IntPtr pidl, byte[] pszPath); 

		[DllImport("coredll", SetLastError=true)]
		private static extern IntPtr LocalFree(IntPtr ptr);

        #endregion

    #region helper class for BROWSEINFO struct
		private class BrowseInfo
		{
			private byte[] m_data;
			private byte[] m_displayname;
			private byte[] m_title;
			private GCHandle namehandle;
			private GCHandle titlehandle;

			public BrowseInfo()
			{
				m_data = new byte[32];
				m_displayname = new byte[512];
				m_title = new byte[128];

				namehandle = GCHandle.Alloc(m_displayname, GCHandleType.Pinned);
				titlehandle = GCHandle.Alloc(m_title, GCHandleType.Pinned);

				BitConverter.GetBytes((int)namehandle.AddrOfPinnedObject() + 4).CopyTo(m_data, 8);
				BitConverter.GetBytes((int)titlehandle.AddrOfPinnedObject() + 4).CopyTo(m_data, 12);
			}

			public byte[] ToByteArray()
			{
				return m_data;
			}

			~BrowseInfo()
			{
				namehandle.Free();
				titlehandle.Free();
			}

			public string Title
			{
				get
				{
					string title = System.Text.Encoding.Unicode.GetString(m_title, 0, m_title.Length);
					int nullindex = title.IndexOf('\0');
					if(nullindex==-1)
					{
						return title;
					}
					return title.Substring(0, title.IndexOf('\0'));
				}
				set
				{
					byte[] titlebytes = System.Text.Encoding.Unicode.GetBytes(value + '\0');
					if(titlebytes.Length > m_title.Length)
					{
						throw new ArgumentException("Description must be no longer than 64 characters");
					}
					try
					{
						Buffer.BlockCopy(titlebytes, 0, m_title,0, titlebytes.Length); 
					}
					catch
					{
					}
				}
			}

			public string FileName
			{
				get
				{
					string filename = System.Text.Encoding.Unicode.GetString(m_displayname, 0, m_displayname.Length);
					int nullindex = filename.IndexOf('\0');
					if(nullindex==-1)
					{
						return filename;
					}
					return filename.Substring(0, filename.IndexOf('\0'));
				}
				set
				{
					byte[] filenamebytes = System.Text.Encoding.Unicode.GetBytes(value + '\0');
					if(filenamebytes.Length > m_title.Length)
					{
						throw new ArgumentException("SelectedFolder must be no longer than 256 characters");
					}
					Buffer.BlockCopy(filenamebytes, 0, m_displayname,0, filenamebytes.Length); 
				}
			}
			
						/*HWND hwndOwner;
						LPCITEMIDLIST pidlRoot;
						LPTSTR pszDisplayName;
						LPCTSTR lpszTitle;
						UINT ulFlags;
						BFFCALLBACK lpfn;
						LPARAM lParam;
						int iImage;*/
					
		}
        #endregion
	}
#endif
}









