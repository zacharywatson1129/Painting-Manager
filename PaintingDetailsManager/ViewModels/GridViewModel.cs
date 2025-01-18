using Caliburn.Micro;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    public class GridViewModel
    {
        private readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        private IDataAccess _dataAccess;
        public GridViewModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            List<Painting> allPaintings = _dataAccess.LoadAllPaintings();
            List<string> paths = new List<string>();
            foreach (Painting p in allPaintings)
            {
                p.FileName = imagesFolderPath + p.FileName;
            }
            foreach (var p in allPaintings)
            {
                paths.Add(p.FileName);
            }
            ImageList = new BindableCollection<string>(paths);
        }

        private BindableCollection<string> _imageList;
        public BindableCollection<string> ImageList
        {
            get { return _imageList; }
            set { _imageList = value;
                // NotifyOfPropertyChange(() => ImageList); 
            }
        }

    }
}
