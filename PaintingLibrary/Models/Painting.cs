 using System;
using System.Collections.Generic;

namespace PaintingLibrary.Models
{
    public class Painting
    {
        // 
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        
        /// <summary>
        /// A string containing only the file name of the painting.
        /// </summary>
        public string FileName { get; set; } = String.Empty;

        private int _width;
        /// <summary>
        /// Width of the painting. If a value less than 0 is set, the absolute value
        /// of that value will be taken and set as the value.
        /// </summary>
        public int Width 
        {
            get {  return _width; }
            set
            {
                if (value < 0)
                    value *= -1;
                _width = value;
            }
        }
        // Must be >= 0
        private int _height;
        private DateTime _datePainted;

        /// <summary>
        /// Height of the painting. If a value less than 0 is set, the absolute value
        /// of that value will be taken and set as the value.
        /// </summary>
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    value *= -1;
                _height = value;
            }
        }


        public string TextSize { get { return $"{Width}\"x {Height}\""; } }
        
        /// <summary>
        /// When the painting was made.
        /// </summary>
        public DateTime DatePainted 
        {
            get { return _datePainted;  }
            set { _datePainted = value; }
        }
        // Must be >= 0
        public double Price { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public SurfaceType PaintingSurface { get; set; }
    }
}