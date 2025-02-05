﻿using Caliburn.Micro;
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
    public class ListViewModel : Screen
    {
        // private BindableCollection<Painting> _Paintings;
        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
                                            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                            @"..\..\Images\"));
        private IDataAccess _dataAccess;
        public ListViewModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            Paintings = new BindableCollection<Painting>(_dataAccess.LoadAllPaintings().AsEnumerable());
            RefreshData();
        }

        public BindableCollection<Painting> Paintings { get; set; } 
            //= new BindableCollection<Painting>(_dataAccess.loadAllPaintings().AsEnumerable());

        public Painting SelectedPainting { get; set; }

        public void RefreshData()
        {
            var allPaintings = _dataAccess.LoadAllPaintings();

            var orderedPaintings = from p in allPaintings
                                   orderby p.DatePainted
                                   select p;


            Paintings = new BindableCollection<Painting>(orderedPaintings);
            foreach (Painting p in Paintings)
            {
                p.FileName = imagesFolderPath + p.FileName;
            }
        }

        public void RemovePainting()
        {
            if (SelectedPainting != null)
            {
                _dataAccess.DeletePainting(SelectedPainting.Id);
                RefreshData();
            }
        }

        public void Print()
        {
            string writePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\index.html";
            HTMLWriter.Write(writePath, new List<Painting>(Paintings));

            System.Diagnostics.Process.Start(writePath);
        }
    }
}
