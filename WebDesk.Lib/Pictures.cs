using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;


using Tripous;

namespace WebDesk
{

    /// <summary>
    /// Helper
    /// </summary>
    static public class Pictures
    {

        /// <summary>
        /// Constructor
        /// </summary>
        static Pictures()
        {
            //ImageUrlFunc = DefaultImageUrl;
        }

        /* public */
        /// <summary>
        /// Returns true if a specified file name is not null or empty and it just contains characters that are valid for a file name.
        /// </summary>
        static public bool IsValidFileName(string FileName)
        {
            return !string.IsNullOrWhiteSpace(FileName) && FileName.Trim() != "-" && Sys.IsValidFileName(FileName);
        }
        /// <summary>
        /// Validates and returns a file name, complete with a specified prefix.
        /// <para>NOTE: If the specified file name is not valid, it uses a random file name.</para>
        /// </summary>
        static public string ValidateFileName(string FileName, string Prefix = "")
        {
            if (!IsValidFileName(FileName))
                FileName = Path.GetRandomFileName();

            if (!string.IsNullOrWhiteSpace(Prefix) && !FileName.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
                FileName = Prefix + FileName;

            return FileName;
        }
        /// <summary>
        /// Validates and returns a file name, complete with a specified prefix and extension.
        /// <para>NOTE: If the specified file name is not valid, it uses a random file name.</para>
        /// </summary>
        static public string ValidateFileName(string FileName, string Ext, string Prefix = "")
        {
            FileName = ValidateFileName(FileName, Prefix);
            return Path.ChangeExtension(FileName, Ext);
        }

        /// <summary>
        /// Returns true if a picture with a specified filename exists in disk.
        /// </summary>
        static public bool Exists(string FileName, string Prefix = "")
        {
            return !string.IsNullOrWhiteSpace(FindImageUrl(FileName, Prefix));
        }
        /// <summary>
        /// Returns the image url path of a file name ONLY if the file exists in Theme's image folder, else returns EMPTY string.
        /// </summary>
        static public string FindImageUrl(string FileName, string Prefix = "")
        {
            string[] Exts = { "jpeg", "png", "jpg" };
            FileName = ValidateFileName(FileName, Prefix);

            string FilePath;
            string Result;
            foreach (string Ext in Exts)
            {
                Result = Path.ChangeExtension(FileName, Ext);
                FilePath = Path.Combine(ImagesPath, Result);
                if (File.Exists(FilePath))
                {
                    return ImageUrlFunc(Result);
                }
            }

            return "";
        }

        /// <summary>
        /// Saves an image in Theme's image folder and returns the image url path.
        /// </summary>
        static public string Save(string FileName, Image Image, string Prefix = "")
        {
            string Result = "";
            if (Image != null)
            {
                try
                {
                    FileName = ValidateFileName(FileName, Image.FileExtension(), Prefix);
                    string FilePath = Path.Combine(ImagesPath, FileName);
                    if (!File.Exists(FilePath))
                        Image.Save(FilePath);
                    Result = ImageUrlFunc(FileName);
                }
                catch
                {
                }
            }
            return Result;
        }
        /// <summary>
        /// Saves a base64 string as an image in Theme's image folder and returns the image url path.
        /// </summary>
        static public string Save(string FileName, string PictureText, string Prefix = "")
        {
            string Result = "";

            if (!string.IsNullOrWhiteSpace(PictureText))
            {
                try
                {
                    Image Image = Sys.Base64ToImage(PictureText);
                    Result = Save(FileName, Image, Prefix);
                }
                catch
                {
                }
            }

            return Result;
        }
        /// <summary>
        /// Saves a base64 string as an image in Theme's image folder and returns the image url path.
        /// </summary>
        static public string Save(Picture Picture, string Prefix = "")
        {
            return Picture == null ? string.Empty : Save(Picture.FileName, Picture.PictureText, Prefix);
        }

        /// <summary>
        /// Returns the path url of a 'system' image, e.g. ~/images/system/IMAGE.png
        /// </summary>
        static public string DefaultSystemImageUrl(string FileName)
        {
            string S = $"~/images/system";
            S = Sys.UrlCombine(S, FileName);
            return S;
        }
        /// <summary>
        /// Returns the path url of an image, e.g. ~/images/IMAGE.png
        /// </summary>
        static public string DefaultImageUrl(string FileName)
        {
            string S = $"~/images";
            S = Sys.UrlCombine(S, FileName);
            return S;
        }

        /* properties */
        /// <summary>
        /// The full path where images are placed
        /// </summary>
        static public string ImagesPath { get; set; }

        /// <summary>
        /// The function to construct a url to an image. Defaults to DefaultImageUrl() method
        /// </summary>
        static public Func<string, string> ImageUrlFunc { get; set; } = DefaultImageUrl;
        /// <summary>
        /// The function to construct a url to a 'system' image. Defaults to DefaultSystemImageUrl() method
        /// </summary>
        static public Func<string, string> SystemImageUrlFunc { get; set; } = DefaultSystemImageUrl;
    }

}
