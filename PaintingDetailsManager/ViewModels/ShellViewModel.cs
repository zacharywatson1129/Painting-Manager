using Caliburn.Micro;
using PaintingDetailsManager.EventModels;
using PaintingLibrary;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<object>, IShell
    {
        const string IMG_DIALOG_FILTER = "All Image Files | *.*";
        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));
       

        public ShellViewModel(SimpleContainer container, NotesViewModel notesVM, IEventAggregator events, IWindowManager windowManager)
        {
            _container = container;
            _notesVM = notesVM;
            ActivateItem(_container.GetInstance<DefaultViewModel>());
            _events = events;
            _events.Subscribe(this);
            _windowManager = windowManager;
        }

        private SimpleContainer _container;
        private IEventAggregator _events;
        private IWindowManager _windowManager;
        private NotesViewModel _notesVM;

        public void LoadDefaultView()
        {
            ActivateItem(_container.GetInstance<DefaultViewModel>());
        }

        public void CreateNewImage()
        {
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
                        AddPaintingViewModel vm = _container.GetInstance<AddPaintingViewModel>();

                        _events.PublishOnUIThread(new AddPaintingEvent
                            ( new PaintingLibrary.Models.FileImage()
                                {
                                    FileName = openFileDialog.SafeFileName,
                                    CompleteFilePath = openFileDialog.FileName
                                }
                            )
                        );

                        _windowManager.ShowDialog(vm, null, null);
                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Sorry, the application has run into a problem.");
                    Trace.TraceInformation("Error during opening image dialog inside . " + DateTime.Now + "\n" + ex.Message);
                    // You must close or flush the trace to empty the output buffer.
                    Trace.Flush();
                }
            }

        }

        public void LoadNotesPage()
        {
            // We won't need a new instance each time because
            // we can't change the notes from any other view.
            // Might be a little faster anyway.
            ActivateItem(_notesVM);
        }

        public void LoadListView()
        {
            // Going to create a new instance each time since
            // we might need to reload the database anyway.
            ActivateItem(_container.GetInstance<ListViewModel>());
        }

        public void LoadGalleryView()
        {
            ActivateItem(_container.GetInstance<GalleryViewModel>());
        }
    }
}