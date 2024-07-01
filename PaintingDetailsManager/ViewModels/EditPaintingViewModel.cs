using Caliburn.Micro;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintingDetailsManager.ViewModels
{
    public class EditPaintingViewModel : Screen
    {

        const string IMG_DIALOG_FILTER = "All Image Files | *.*";

        private int _myNum = 5;
        public int MyNum 
        { 
            get { return _myNum; }
            set { _myNum = value; NotifyOfPropertyChange(() => MyNum); }
        }

        public EditPaintingViewModel(int id)
        {
            List<Painting> allPaintings = SqliteDataAccess.loadAllPaintings();
            CurrentPainting = allPaintings.Where((p) => p.Id == id).First();

            // Probably a bad name. We are sort of assuming we're using a DatePicker, when technically it could be any component
            // used to change the date. 
            DatePicker_Date = CurrentPainting.DatePainted;
            
            // This is because the file name is JUST the file name, does not include path.
            CurrentPaintingPath = imagesFolderPath + CurrentPainting.FileName;
            Categories = SqliteDataAccess.loadAllCategories();

            MySurfaceType = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
            SelectedSurfaceType =CurrentPainting.PaintingSurface;
            AddedCategories = new ObservableCollection<Category>(CurrentPainting.Categories); // new ObservableCollection<Category>();
            //MySurfaceType = CurrentPainting.PaintingSurface; //Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
        
        }

        private SurfaceType _selectedSurfaceType;
        public SurfaceType SelectedSurfaceType 
        {
            get => _selectedSurfaceType;
            set { Set(ref _selectedSurfaceType, value); NotifyOfPropertyChange(() => SelectedSurfaceType); }
        }

        // This complicated setup is used to convert from enum type to a combobox usable list form.
        // We then bind selected items from that list. See constructor for conversion.
        public IReadOnlyList<SurfaceType> MySurfaceType { get; }

        private SurfaceType _surfaceTypeOptions;
        public SurfaceType SurfaceTypeOptions 
        {
            get => _surfaceTypeOptions;
            set { Set(ref _surfaceTypeOptions, value); NotifyOfPropertyChange(() => SurfaceTypeOptions); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory 
        {
            get { return _selectedCategory; } 
            set 
            { _selectedCategory = value; 
                NotifyOfPropertyChange(() => SelectedCategory);
                NotifyOfPropertyChange(() => CanAddCategory);
            }
        }

        private Category _addedSelectedCategory;
        public Category AddedSelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _addedSelectedCategory = value;
                NotifyOfPropertyChange(() => AddedSelectedCategory);
                NotifyOfPropertyChange(() => CanDeleteCategory);
            }
        }

        public bool IsFormCompleted { get; set; } = false;

        private ObservableCollection<Category> _addedCategories;

        public ObservableCollection<Category> AddedCategories
        {
            get { return _addedCategories; }
            set { _addedCategories = value; NotifyOfPropertyChange(() => AddedCategories); }
        }

        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        public List<Category> Categories { get; set; }

        private string _currentPaintingPath;
        public string CurrentPaintingPath
        {
            get { return _currentPaintingPath; }
            set 
            { 
                _currentPaintingPath = value; 
                NotifyOfPropertyChange(() => CurrentPaintingPath); 
            }
        }

        public bool CanAddCategory
        {
            get 
            {
                if (SelectedCategory != null)
                    return true;
                else
                    return false;
                
            }
        }

        public void AddCategory()
        {
            if (!AddedCategories.Contains(SelectedCategory))
            {
               AddedCategories.Add(SelectedCategory);
               NotifyOfPropertyChange(() => AddedCategories);
            }
        }

        public bool CanDeleteCategory
        {
            get
            {
                if (AddedSelectedCategory != null)
                    return true;
                else
                    return false;
            }
        }

        public void DeleteCategory()
        {
            AddedCategories.Remove(AddedSelectedCategory);
        }

        public Painting CurrentPainting { get; set; }


        DateTime _datePicker_Date = DateTime.Now;
        public DateTime DatePicker_Date 
        { 
            get
            {
                return _datePicker_Date;
            }
            set
            {
                if (value.Year > 2000)
                {
                    _datePicker_Date = value;
                    NotifyOfPropertyChange(() => DatePicker_Date);
                }
            } 
        }

        public void UpdatePainting()
        {
            // TODO: Think we can update CurrentPainting properties whenever we change the individual properties.
            // For now, let's just leave things as is.

            // Step 1 - copy the date--most common thing that might get fixed.
            CurrentPainting.DatePainted = DatePicker_Date;

            // Step 2 - Copy Categories over. Maybe it accidentally got put in a wrong category or needs added to one.
            CurrentPainting.Categories = AddedCategories.ToList();

            // Step 3 - Probably next most common thing, we have a better picture (or maybe we had the wrong picture).
            // CurrentPainting.FileName = CurrentPaintingPath;

            // TODO: Fill in all the other properties.

            // Step 1 - Save updated painting to the database.
            SqliteDataAccess.updatePainting(CurrentPainting);

            // Step 2 - Clear categories for this painting - just wipe them out and resave them. Much easier than
            // determining which ones may have been deleted or which ones were not. Might approach this differently
            // if there was going to be an exceptionally large amount of categories but I don't expect that.
            SqliteDataAccess.clearCategoriesForPainting(CurrentPainting.Id);
            
            // Step 3 - Save categories to the database.
            foreach (Category c in AddedCategories)
            {
                // Console.Write("test");
                SqliteDataAccess.saveCategorizedPainting(CurrentPainting.Id, c.ID);
            }

            // Copy the picture over. If it's a new one it gets copied, if an old one, I guess it gets overwritten?
            if (!System.IO.File.Exists(imagesFolderPath + CurrentPainting.FileName))
            {
                System.IO.File.Copy(CurrentPaintingPath, imagesFolderPath + CurrentPainting.FileName);
            }            
            
            // Step 3 - TryClose();
            TryClose();
        }
        public void ChangePicture()
        {
            MessageBox.Show("Something really is happening.");
            Stream checkStream = null;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Multiselect = false;

            openFileDialog.Filter = IMG_DIALOG_FILTER;

            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    if ((checkStream = openFileDialog.OpenFile()) != null)
                    {
                        CurrentPaintingPath = openFileDialog.FileName;
                        //MessageBox.Show(CurrentPaintingPath);
                        string imgSource = imagesFolderPath + openFileDialog.SafeFileName;

                        Console.WriteLine("Hey there. imgSource: " + imgSource);
                        //string imgSource = imagesFolderPath + openFileDialog.SafeFileName;
                        // if the image is not in the folder currently basically (meaning we must have changed it),
                        // then let's copy it to the folder.
                        if (!System.IO.File.Exists(imgSource))
                        {
                            System.IO.File.Copy(openFileDialog.FileName, imgSource);
                        }
                        //CurrentPaintingPath = imgSource;
                        CurrentPainting.FileName = openFileDialog.SafeFileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Debug.WriteLine("Well, sorry to be the bearer of bad news, but something went south.");
                    // MessageBox.Show("Sorry, the application has run into a problem.");
                    Trace.TraceInformation("Error during opening image dialog inside . " + DateTime.Now + "\n" + ex.Message);
                    // You must close or flush the trace to empty the output buffer.
                    Trace.Flush();
                }
            }
        }
    }
}
