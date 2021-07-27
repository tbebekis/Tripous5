/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Tripous
{
    /// <summary>
    /// Helper class for images
    /// </summary>
    static public class ImageUtls
    {

        /// <summary>
        /// Resizes Source image to the specified dimensions, keeping the aspect ratio.
        /// <para>WARNING: Quality must be non-negative.</para>
        /// </summary>    
        static public Image Resize(this Image Source, int MaxWidth, int MaxHeight, int Quality = 90)
        {
            // Get the image's original width and height
            int originalWidth = Source.Width;
            int originalHeight = Source.Height;

            // To preserve the aspect ratio
            float ratioX = (float)MaxWidth / (float)originalWidth;
            float ratioY = (float)MaxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            using (Bitmap WorkingImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
            {

                // Draws the image in the specified size with quality mode set to HighQuality
                using (Graphics graphics = Graphics.FromImage(WorkingImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(Source, 0, 0, newWidth, newHeight);
                }

                WorkingImage.MakeTransparent();

                // Get an ImageCodecInfo object that represents the JPEG codec.
                ImageCodecInfo imageCodecInfo = GetEncoderInfo(Source.RawFormat);

                // Create an Encoder object for the Quality parameter.
                Encoder encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                EncoderParameters encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                EncoderParameter encoderParameter = new EncoderParameter(encoder, Quality);
                encoderParameters.Param[0] = encoderParameter;

                using (MemoryStream MS = new MemoryStream())
                {
                    WorkingImage.Save(MS, imageCodecInfo, encoderParameters);
                    return Image.FromStream(MS);
                }

            }
        }
        /// <summary>
        /// Resizes Source image to the specified dimensions, keeping the aspect ratio.
        /// <para>FROM: http://stackoverflow.com/questions/1940581/c-sharp-image-resizing-to-different-size-while-preserving-aspect-ratio </para>
        /// </summary>    
        static public Image Resize2(this Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        static public ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        /// <summary>
        /// Returns the mime format string, i.e image/jpg
        /// </summary>
        static public string MimeOf(Image Source)
        {
            return MimeOf(Source.RawFormat);
        }
        /// <summary>
        /// Returns the mime format string, i.e image/jpg
        /// </summary>
        static public string MimeOf(ImageFormat Format)
        {
            if (Format.Equals(ImageFormat.Png))
                return "image/png";
            if (Format.Equals(ImageFormat.Jpeg))
                return "image/jpeg";
            if (Format.Equals(ImageFormat.Gif))
                return "image/gif";
            if (Format.Equals(ImageFormat.Tiff))
                return "image/tiff";

            return "";
        }


        /// <summary>
        /// The method takes two objects - the image to crop (System.Drawing.Image) 
        /// and the rectangle to crop out (System.Drawing.Rectangle). 
        /// The next thing done is to create a Bitmap (System.Drawing.Bitmap) of the image. 
        /// The only thing left is to crop the image. 
        /// This is done by cloning the original image but only taking a rectangle of the original.
        /// From: http://tech.pro/tutorial/620/csharp-tutorial-image-editing-saving-cropping-and-resizing
        /// </summary>
        static public Image CropImage(Image img, Rectangle cropArea)
        {
            if (img != null)
            {
                Bitmap bmpImage = new Bitmap(img);
                Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                return (Image)(bmpCrop);
            }

            return null;
        }
 
        /// <summary>
        /// Converts an image to a base64 string
        /// </summary>
        static public string ImageToBase64(Image Image, bool InsertLineBreaks = true)
        {
            return Sys.ImageToBase64(Image, InsertLineBreaks);
        }
        /// <summary>
        /// Converts a base64 string back to an image
        /// </summary>
        static public Image Base64ToImage(string Text)
        {
            return Sys.Base64ToImage(Text);
        }

 
    }
}
