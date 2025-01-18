using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingLibrary.Models
{
    public class CategorizedPainting
    {
        public Guid PaintingID { get; set; }
        public Guid CategoryID { get; set; }
    }
}
