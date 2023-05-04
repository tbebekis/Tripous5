using System;
//using System.Drawing;
using System.Drawing.Imaging;

namespace Tripous
{
    /// <summary>
    /// Extensions
    /// see also: https://blogs.msdn.microsoft.com/dotnet/2017/01/19/net-core-image-processing/
    /// </summary>
    static public class ImageExtensions
    {
        /// <summary>
        /// Returns the file extension of a specified image
        /// </summary>
        static public string FileExtension(this Image Instance)
        {
            return Instance.RawFormat.FileExtension();
        }
        /// <summary>
        /// Returns the file extension of a specified image format
        /// </summary>
        static public string FileExtension(this ImageFormat RawFormat)
        {
            if (RawFormat.Guid == ImageFormat.Png.Guid)
            {
                return "png";
            }
            else if (RawFormat.Guid == ImageFormat.Bmp.Guid)
            {
                return "bmp";
            }
            else if (RawFormat.Guid == ImageFormat.Emf.Guid)
            {
                return "x-emf";
            }
            else if (RawFormat.Guid == ImageFormat.Exif.Guid)
            {
                return "jpeg";
            }
            else if (RawFormat.Guid == ImageFormat.Gif.Guid)
            {
                return "gif";
            }
            else if (RawFormat.Guid == ImageFormat.Icon.Guid)
            {
                return "ico";
            }
            else if (RawFormat.Guid == ImageFormat.Jpeg.Guid)
            {
                return "jpeg";
            }
            else if (RawFormat.Guid == ImageFormat.MemoryBmp.Guid)
            {
                return "bmp";
            }
            else if (RawFormat.Guid == ImageFormat.Tiff.Guid)
            {
                return "tiff";
            }
            else if (RawFormat.Guid == ImageFormat.Wmf.Guid)
            {
                return "wmf";
            }

            return string.Empty;
        }
        /// <summary>
        /// Returns the mime of an image
        /// </summary>
        static public string Mime(this Image Instance)
        {
            return Mime(Instance.RawFormat);
        }
        /// <summary>
        /// Returns the mime of an image format
        /// </summary>
        static public string Mime(this ImageFormat RawFormat)
        {
            return $"image/{RawFormat.FileExtension()}";             
        }

    }
}
