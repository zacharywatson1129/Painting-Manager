 using System;
using System.Collections.Generic;

namespace PaintingLibrary.Models
{
    public class Painting
    {
       /* public override bool Equals(object obj)
        {
            Painting painting = (Painting)obj;
            if (this.Id == painting.Id) // They must be equal. Otherwise, either SQL or my code broke.
                return true;
            return false;
        }*/
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string FileName { get; set; } = String.Empty;
        // Must be >= 0
        private int _width;
        public int Width 
        {
            get {  return _width; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value + "must be >= 0");
                _width = value;
            }
        }
        // Must be >= 0
        private int _height;
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value + "must be >= 0");
                _height = value;
            }
        }
        public string TextSize { get { return $"{Width}\"x {Height}\""; } }
        public DateTime DatePainted { get; set; } = DateTime.Now;
        // Must be >= 0
        public double Price { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public SurfaceType PaintingSurface { get; set; }
    }
}