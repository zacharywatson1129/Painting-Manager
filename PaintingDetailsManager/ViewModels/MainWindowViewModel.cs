using Caliburn.Micro;
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
    public class MainWindowViewModel : Conductor<object>, IShell
    {
        const string IMG_DIALOG_FILTER = "All Image Files | *.*";
        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        public MainWindowViewModel()
        {
            LoadDefaultView();
        }

        public bool IsDefaultView { get; set; } = false;

        public void LoadDefaultView()
        {
            ActivateItem(new DefaultViewModel());
            IsDefaultView = true;
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

                        string imgSource = imagesFolderPath + openFileDialog.SafeFileName;

                        // System.IO.File.Copy(openFileDialog.FileName, imgSource);

                        AddPaintingViewModel vm = new AddPaintingViewModel(openFileDialog.SafeFileName, openFileDialog.FileName);
                        WindowManager manager = new WindowManager();
                        manager.ShowDialog(vm, null, null);
                        /*vm.CurrentPainting.FileName = openFileDialog.SafeFileName;
                        SqliteDataAccess.savePainting(vm.CurrentPainting);
                        DefaultViewModel vm2 = new DefaultViewModel();
                        this.ActivateItem(vm2);
                        vm2.CurrentPainting = vm2.allPaintings.Where((p) => p.Id == vm.CurrentPainting.Id).FirstOrDefault();*/
                        /*this.ActivateItem(this.ActiveItem);*/
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
            ActivateItem(new NotesViewModel());
        }

        public void LoadListView()
        {
            ActivateItem(new ListViewModel());
        }

        public void LoadGalleryView()
        {
            ActivateItem(new GalleryViewModel());
        }
    }
}
// -----------------------In case I need it.-------------------

/*
 * Stream checkStream = null;
Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

openFileDialog.Multiselect = false;

openFileDialog.Filter = IMG_DIALOG_FILTER;

if ((bool)openFileDialog.ShowDialog())
{
    try
    {
        if ((checkStream = openFileDialog.OpenFile()) != null)
        {

            string imgSource = imagesFolderPath + openFileDialog.SafeFileName;

            System.IO.File.Copy(openFileDialog.FileName, imgSource);

            // Now pass to InputDetails window
            InputDetails win = new InputDetails(openFileDialog.SafeFileName, imagesFolderPath);
            win.ShowDialog();
            loadImages();
            if (allPaintings.Count > 0)
            {
                imgIndex++;
                setImgAndDetails(allPaintings[imgIndex]);
            }
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
else
{
    // MessageBox.Show("Something went wrong. Please try again.");
}
 * 
 * 
 */
