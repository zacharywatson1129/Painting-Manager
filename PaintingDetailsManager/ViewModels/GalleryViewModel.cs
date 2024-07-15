using Caliburn.Micro;
using Microsoft.Win32;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace PaintingDetailsManager.ViewModels
{
    public class GalleryViewModel : Screen
    {
        public GalleryViewModel()
        {
            loadImages();
            Categories = SqliteDataAccess.loadAllCategories();

            Years = new List<int>();
            Years.Add(0);
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i);
            }
        }

        public string SizeAndSurface
        {
            get { return $"{ CurrentPainting.Width} x {CurrentPainting.Height} on {CurrentPainting.PaintingSurface}"; }
        }

        
        private string _currentPaintingPath;
        /// <summary>
        /// The entire path of the current painting's image (images folder path + filename)
        /// </summary>
        public string CurrentPaintingPath
        {
            get { return _currentPaintingPath; }
            set 
            { 
                _currentPaintingPath = value; 
                NotifyOfPropertyChange(() => CurrentPaintingPath);
                NotifyOfPropertyChange(() => SizeAndSurface);
            }
        }

        private Painting _painting;
        public Painting CurrentPainting
        {
            get { return _painting; }
            set
            {
                _painting = value;
                NotifyOfPropertyChange(() => CurrentPainting);
                if (value != null)
                    CurrentPaintingPath = CurrentPainting.FileName;
                else
                    CurrentPaintingPath = "";
            }
        }

        private int imgIndex;
        

        public List<Painting> allPaintings { get; set; } = new List<Painting>();

        private readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        public void loadImages()
        {
            SqliteDataAccess.loadAllPaintings().ForEach(p => allPaintings.Add(p));
            foreach (Painting p in allPaintings)
            {
                p.FileName = imagesFolderPath + p.FileName;
            }
            FilteredPaintings = allPaintings;
            SetPainting(imgIndex);
        }

        private string _currentImageNumber;

        public string CurrentImageNumber
        {
            get { return _currentImageNumber; }
            set { _currentImageNumber = value; NotifyOfPropertyChange(() => CurrentImageNumber); }
        }

        public void Left()
        {
            if (imgIndex == 0)
            {
                imgIndex = FilteredPaintings.Count - 1;
            }
            else
            {
                imgIndex--;
            }
            CurrentPainting = FilteredPaintings[imgIndex];
            CurrentImageNumber = (imgIndex + 1) + "/" + FilteredPaintings.Count;
        }

        public void Right()
        {
            
            if (imgIndex == FilteredPaintings.Count - 1)
            {
                imgIndex = 0;
            }
            else
            {
                imgIndex++;
            }
            CurrentPainting = FilteredPaintings[imgIndex];
            CurrentImageNumber = (imgIndex + 1) + "/" + FilteredPaintings.Count;
        }

        private List<Painting> _filteredPaintings;
        /// <summary>
        /// Represents the paintings which should be available based on the current filter.
        /// </summary>
        public List<Painting> FilteredPaintings 
        {
            get
            {
                return _filteredPaintings;
            }
            set
            {
                // Order by date by default...this may change depending on what ideas I get.
                var orderedPaintings = from p in value
                                       orderby p.DatePainted
                                       select p;
                _filteredPaintings = orderedPaintings.ToList();


                // _filteredPaintings = value;
            }
        }

        public string AppliedFilter
        {
            get 
            {
                if (SelectedCategory == null && SelectedYear == 0)
                    return $"All Paintings ({allPaintings.Count})";
                else if (SelectedCategory == null && SelectedYear != 0)
                    return $"Painted {SelectedYear} ({FilteredPaintings.Count})";
                else if (SelectedCategory != null && SelectedYear == 0)
                {
                    return $"All Paintings in Category {SelectedCategory.Name} ({FilteredPaintings.Count})";
                }
                return $"{SelectedCategory.Name} in {SelectedYear} ({FilteredPaintings.Count})";
            }
        }

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; NotifyOfPropertyChange(() => Categories); }
        }


        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set // Setting happens upon choosing an item in the combobox.
            {
                // I think this should work, even if we set the selectedcategory to be null
                _selectedCategory = value;
                // this allows us, when clearing filter, to deselect in combobox
                NotifyOfPropertyChange(() => SelectedCategory); 

                if (value != null) // If a category is selected.
                {
                    FilteredPaintings = FindPaintingsWithCategory(_selectedCategory);
                    imgIndex = 0;
                    SetPainting(imgIndex);
                    NotifyOfPropertyChange(() => AppliedFilter);
                }
                else // If we've cleared out all categories or one is not selected...
                {
                    FilteredPaintings = allPaintings;
                    imgIndex = 0;
                    SetPainting(imgIndex);
                    NotifyOfPropertyChange(() => AppliedFilter);
                }

            }
        }

        private List<int> _years;

        public List<int> Years
        {
            get { return _years; }
            set { _years = value; NotifyOfPropertyChange(() => Years); }
        }


        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set { 
                _selectedYear = value; 
                NotifyOfPropertyChange(() => SelectedYear);
                if (value != 0) // We have a filtered year
                {

                    if (SelectedCategory != null) // We also selected a category.
                    {
                        // first filter by category, then date.
                        FilteredPaintings = FindPaintingsWithCategory(SelectedCategory);
                        FilteredPaintings = FilteredPaintings.Where((p) => p.DatePainted.Year == SelectedYear).ToList();
                    }
                    else // No category, but year is selected.
                    {
                        FilteredPaintings = allPaintings.Where((p) => p.DatePainted.Year == SelectedYear).ToList();
                    }
                }

                else // value is 0, don't filter by date.
                {
                    if (SelectedCategory != null)
                    {
                        FilteredPaintings = FindPaintingsWithCategory(SelectedCategory);
                    }
                    else
                    {
                        FilteredPaintings = allPaintings;
                    }
                }
                imgIndex = 0;
                SetPainting(imgIndex);

                NotifyOfPropertyChange(() => CurrentImageNumber);
                
                NotifyOfPropertyChange(() => AppliedFilter);
            }
        }


        public void ClearDateFilter()
        {
            SelectedYear = 0;
            if (SelectedCategory == null)
            {
                FilteredPaintings = allPaintings;
                imgIndex = 0;
                SetPainting(imgIndex);
            } else
            {
                //FilteredPaintings = allPaintings.Where((p) => p.Categories.Contains(SelectedCategory)).ToList();
                FilteredPaintings = FindPaintingsWithCategory(SelectedCategory);
                imgIndex = 0;
                SetPainting(imgIndex);
            }
        }

        public List<Painting> FindPaintingsWithCategory(Category cat)
        {
            List<Painting> output = new List<Painting>();
            foreach (Painting p in allPaintings)
            {
                foreach (Category c in p.Categories)
                {
                    if (c.ID == cat.ID)
                    {
                        output.Add(p);
                        break;
                    }
                }
            }
            return output;
        }

        public void SetPainting(int index)
        {
            if (FilteredPaintings == null || FilteredPaintings.Count == 0)
            {
                CurrentPainting = null;
                CurrentImageNumber = "0/0";
            }
            else
            {
                CurrentPainting = FilteredPaintings[index];
                CurrentImageNumber = $"{index+1}/{FilteredPaintings.Count}";
            }
        }

        public void ClearFilter()
        {
            SelectedCategory = null;
            NotifyOfPropertyChange(() => AppliedFilter);
        }


        public void OrderByDate()
        {
            var orderedPaintings = from p in FilteredPaintings
                                   orderby p.DatePainted
                                   select p;
            FilteredPaintings = orderedPaintings.ToList();
        }
        
        public void EditPainting()
        {
            EditPaintingViewModel vm = new EditPaintingViewModel(CurrentPainting.Id);
            WindowManager manager = new WindowManager();
            manager.ShowDialog(vm, null, null); 
            
            //SetPainting();
        }

        /*public bool CanOrderByDate
        {
            get
            {
                if (SelectedYear != 0)
                    return true;
                return false;
            }
        }*/
    }
}