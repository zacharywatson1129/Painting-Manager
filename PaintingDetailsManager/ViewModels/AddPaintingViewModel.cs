using Caliburn.Micro;
using PaintingDetailsManager.EventModels;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    public class AddPaintingViewModel : Screen, IHandle<AddPaintingEvent>
    {
        private SimpleContainer _container;

        public AddPaintingViewModel(IEventAggregator events, SimpleContainer container)
        {
            _events = events;
            _events.Subscribe(this);
            _container = container;

            CurrentPainting = new Painting();
            //CurrentPainting.FileName = justFileName;

            //CurrentPaintingPath = originalFilePath;
            Categories = SqliteDataAccess.loadAllCategories();
            CurrentPainting.PaintingSurface = SelectedSurfaceType;
            AddedCategories = new ObservableCollection<Category>();
            MySurfaceType = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
        
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

        public IEventAggregator _events { get; set; }

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

        public void SavePainting()
        {
            // Step -1 - Copy the binded date over...as just the date. that's all we need for now.
            CurrentPainting.DatePainted = DatePicker_Date;

            // Step 0 - Copy Categories over.
            CurrentPainting.Categories = AddedCategories.ToList();

            // Step 1 - Save painting to the database.
            
            DBQueryResult result = SqliteDataAccess.savePainting(CurrentPainting);
            CurrentPainting.Id = result.LastID;

            // We need to get the ID of the item we just saved.

            // Step 2 - Super important, save categories to the database.
            foreach (Category c in CurrentPainting.Categories)
            {
                SqliteDataAccess.saveCategorizedPainting(CurrentPainting.Id, c.ID);
            }

            // Step 2 - If we were successful, copy the image to images folder.
            // Ignore check for now
            if (!System.IO.File.Exists(imagesFolderPath + CurrentPainting.FileName))
            {
                System.IO.File.Copy(CurrentPaintingPath, imagesFolderPath + CurrentPainting.FileName);
            }            
            
            // Step 3 - TryClose();
            TryClose();
        }

        public void Handle(AddPaintingEvent message)
        {
            CurrentPainting.FileName = message.PaintingFileImage.FileName;
            CurrentPaintingPath = message.PaintingFileImage.CompleteFilePath;

            Debug.WriteLine($"AddPaintingEvent handled. FileName: {CurrentPainting.FileName}, FilePath: {CurrentPaintingPath}");
        }
    }
}
