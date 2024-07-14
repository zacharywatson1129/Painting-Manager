using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.EventModels
{
    public class AddPaintingEvent
    {
        public FileImage PaintingFileImage { get; set; } 
        public AddPaintingEvent(FileImage paintingFileImage)
        {
            PaintingFileImage = paintingFileImage;
        }
    }
}
