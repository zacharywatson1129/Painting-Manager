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
        public int Width { get; set; }
        public int Length { get; set; }
        public string TextSize { get { return $"{Width}\"x {Length}\""; } }
        public DateTime DatePainted { get; set; } = DateTime.Now;
        public double Price { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public SurfaceType PaintingSurface { get; set; }
    }
}