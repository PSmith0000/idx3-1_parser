using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace idx3_ubyte_parser
{
    internal class IDX3
    {
        /// <summary>
        /// Type of content (Image or Label..)
        /// </summary>
        public ContentType contentType { get; set; }

        /// <summary>
        /// How many items exist
        /// </summary>
        public int ContentSize { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public List<Image> Images = new List<Image>();

        public enum ContentType : int
        {
            image = 2051,
            Label = 2049
        }
    }
}