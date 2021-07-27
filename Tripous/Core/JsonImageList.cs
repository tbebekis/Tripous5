/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tripous
{
 
    /// <summary>
    /// For serialization
    /// </summary>
    public class JsonImage 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonImage()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonImage(Image Image)
        {
            Mime = ImageUtls.MimeOf(Image);
            Width = Image.Width;
            Height = Image.Height;
            Data = Sys.ImageToBase64(Image, false);
        }


        /// <summary>
        /// Mime type
        /// </summary>
        public string Mime { get; set; }
        /// <summary>
        /// Base64 string
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Used when it is an icon
        /// </summary>
        public int Size => Width;
    }
 

    /// <summary>
    /// A list of images for json serialization
    /// </summary>
    public class JsonImageList
    {
        Dictionary<string, JsonImage> List = new Dictionary<string, JsonImage>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonImageList()
        {
        }

        /* static */
        /// <summary>
        /// Creates and returns a <see cref="JsonImage"/> containing and "Image", that is and image where Width may not equals to Height
        /// <para>ENTRY FORMAT: {"Mime": "image/png", "Width": 200, "Height": 100, "Data": [Data Url here ] }</para>
        /// </summary>
        static public JsonImage CreateImage(Image Image)
        {
            JsonImage Result = new JsonImage(Image);
            return Result;
        }
        /// <summary>
        /// Creates and returns a <see cref="JsonImage"/> containing and "Ico", that is an image where Width equals to Height
        /// <para>ENTRY FORMAT: {"Mime": "image/png", "Size": 32, "Data": [Data Url here ] }</para>
        /// </summary>
        static public JsonImage CreateIcon(Image Ico)
        {
            return CreateImage(Ico);
        }


        /* public */
        /// <summary>
        /// JO must contain an image 
        /// <para>ENTRY FORMAT for general image: {"Mime": "image/png", "Width": 200, "Height": 100, "Data": [Data Url here ] }</para>
        /// <para>ENTRY FORMAT for icon: {"Mime": "image/png", "Size": 32, "Data": [Data Url here ] }</para>
        /// </summary>
        public void Add(string Name, JsonImage Image)
        {
            List[Name] = Image;
        }
        /// <summary>
        /// Adds and "Image" (that is and image where Width may not equals to Height) to the list under Name
        /// <para>ENTRY FORMAT: {"Mime": "image/png", "Width": 200, "Height": 100, "Data": [Data Url here ] }</para>
        /// </summary>
        public void AddImage(string Name, Image Image)
        {
            List[Name] = CreateImage(Image);
        }
        /// <summary>
        /// Adds an "Ico" (that is an image where Width equals to Height) to the list under Name
        /// <para>ENTRY FORMAT:{"Mime": "image/png", "Size": 32, "Data": [Data Url here ] }</para>
        /// </summary>
        public void AddIcon(string Name, Image Ico)
        {
            List[Name] = CreateIcon(Ico);
        }

        /// <summary>
        /// Returns the json text
        /// </summary>
        public string ToJson()
        {
            return Json.Serialize(List);
        }
    }
}
