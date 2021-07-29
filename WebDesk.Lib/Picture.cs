using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WebDesk
{
    /// <summary>
    /// A picture of an item, i.e. product, etc.
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Display order
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Base64 text of picture bytes
        /// </summary>
        public string PictureText { get; set; }
        /// <summary>
        /// Url of the image
        /// </summary>
        [JsonIgnore()]
        public string ImageUrl { get; set; }
    }
}
