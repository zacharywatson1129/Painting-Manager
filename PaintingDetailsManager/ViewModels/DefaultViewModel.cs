using Caliburn.Micro;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    public class DefaultViewModel : Screen
    {
        private IDataAccess _dataAccess;

        public DefaultViewModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            loadImages();

            Categories = _dataAccess.loadAllCategories();

            this.SizeQuantityTable = new DataTable();

            DataColumn categoryColumn = new DataColumn();
            categoryColumn.ColumnName = "Category";
            this.SizeQuantityTable.Columns.Add(categoryColumn);

            DataColumn sColumn = new DataColumn();
            /*sColumn.ColumnName = "2015";
            this.SizeQuantityTable.Columns.Add(sColumn);*/

            /*DataColumn mColumn = new DataColumn();
            mColumn.ColumnName = "M";
            this.SizeQuantityTable.Columns.Add(mColumn);*/
            var cols = new List<DataColumn>();
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                var tempCol = new DataColumn();
                tempCol.ColumnName = i.ToString();
                this.SizeQuantityTable.Columns.Add(tempCol);
                cols.Add(tempCol);
            }
            //sizeQuantityTable.Columns.Add()
            
            /*
            DataRow row1 = this.SizeQuantityTable.NewRow();
            
            row1[sizeQuantityColumn] = "Blue";
            row1[sColumn] = "12";
            row1[mColumn] = "15";
            this.SizeQuantityTable.Rows.Add(row1);

            DataRow row2 = this.SizeQuantityTable.NewRow();
            row2[sizeQuantityColumn] = "Red";
            row2[sColumn] = "18";
            row2[mColumn] = "21";
            this.SizeQuantityTable.Rows.Add(row2);

            DataRow row3 = this.SizeQuantityTable.NewRow();
            row3[sizeQuantityColumn] = "Green";
            row3[sColumn] = "24";
            row3[mColumn] = "27";
            this.SizeQuantityTable.Rows.Add(row3);

            DataRow row4 = this.SizeQuantityTable.NewRow();
            row4[sizeQuantityColumn] = "Yellow";
            row4[sColumn] = "30";
            row4[mColumn] = "33";
            this.SizeQuantityTable.Rows.Add(row4);*/

            foreach (Category c in Categories)
            {
                // first column contains category name, then the first year's date
                // second year's date, third year's date...
                DataRow tempRow = this.SizeQuantityTable.NewRow();
                tempRow[categoryColumn] = c.Name;
                foreach (DataColumn col in cols)
                {
                    int year = Convert.ToInt32(col.ColumnName);
                    tempRow[col] = allPaintings.Where
                        ((p)=>p.DatePainted.Year == year 
                        && DoesContainCategory(p, c)).Count(); 
                }
                this.SizeQuantityTable.Rows.Add(tempRow);
            }
        }

        public bool DoesContainCategory(Painting painting, Category cat)
        {
            foreach (Category c in painting.Categories)
            {
                if (c.ID == cat.ID)
                {
                    return true;
                }
            }
            return false;
        }

        private DataTable sizeQuantityTable;
        public DataTable SizeQuantityTable
        {
            get
            {
                return sizeQuantityTable;
            }
            set
            {
                sizeQuantityTable = value;
                NotifyOfPropertyChange(()=>SizeQuantityTable);
            }
        }

        private string _sizeAndSurface;
        public string SizeAndSurface
        {
            get { return $"{ CurrentPainting.Width} x {CurrentPainting.Height} on {CurrentPainting.PaintingSurface}"; }
        }

        int imgIndex = 0;

        private string _currentPaintingPath;
        public string CurrentPaintingPath 
        {
            get { return _currentPaintingPath; }
            set { _currentPaintingPath = value; NotifyOfPropertyChange(() => CurrentPaintingPath); } 
        }

        private string _title;
        public string Title 
        {
            get { return _title ; }
            set { _title = value; NotifyOfPropertyChange(() => Title); } 
        }

        private Painting _currentPainting;
        public Painting CurrentPainting
        {
            get { return _currentPainting; }
            set
            { 
                _currentPainting = value; 
                Title = value.Name; 
                NotifyOfPropertyChange(() => CurrentPainting); 
                CurrentPaintingPath = CurrentPainting.FileName;
                Categories = CurrentPainting.Categories;
                //Width = "Width: " + CurrentPainting.Width.ToString();
                //Height = "Length: " + CurrentPainting.Length.ToString();
            }
        }

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; NotifyOfPropertyChange(() => Categories); }
        }

        List<string> imageFileNames = new List<string>();
        public List<Painting> allPaintings { get; set; } = new List<Painting>();

        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        public void loadImages()
        {
            try
            {
                allPaintings.Clear();
                _dataAccess.loadAllPaintings().ForEach(p => allPaintings.Add(p));
                foreach (Painting p in allPaintings)
                {
                    p.FileName = imagesFolderPath + p.FileName;
                }
                if (allPaintings.Count > 0)
                {
                    CurrentPainting = allPaintings[0];
                    NotifyOfPropertyChange(() => CurrentImageNumber);
                }
            }
            // TODO: We need real error handling here.
            // Probably should give the user an error or message box.
            catch (Exception ex)
            {
                Trace.TraceInformation("Error during loading images. Here are the details:\n" + ex.Message);
                Trace.Flush();
            }
        }

        public void Left()
        {
            if (imgIndex == 0)
            {
                imgIndex = allPaintings.Count - 1;
            }
            else
            {
                imgIndex--;
            }
            CurrentPainting = allPaintings[imgIndex];
            NotifyOfPropertyChange(() => CurrentImageNumber);
            NotifyOfPropertyChange(() => SizeAndSurface);
        }

        public void Right()
        {
            if (imgIndex == allPaintings.Count - 1)
            {
                imgIndex = 0;
            }
            else
            {
                imgIndex++;
            }
            CurrentPainting = allPaintings[imgIndex];
            NotifyOfPropertyChange(() => CurrentImageNumber);
            NotifyOfPropertyChange(() => SizeAndSurface);
        }

        public string CurrentImageNumber 
        { 
            get
            {
                return $"{imgIndex + 1}/{allPaintings.Count}";
            }
        }

        
    }
}
