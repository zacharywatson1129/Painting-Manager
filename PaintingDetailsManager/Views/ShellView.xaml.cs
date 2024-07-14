using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintingDetailsManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {

        // List<string> imageFileNames = new List<string>();
        /*List<Painting> allPaintings = new List<Painting>();
        int imgIndex = 0;

        readonly string imagesFolderPath = System.IO.Path.GetFullPath(System.IO.Path.Combine
            (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            @"..\..\Images\"));

        public void loadImages()
        {
            allPaintings.Clear();           
            SqliteDataAccess.loadAllPaintings().ForEach(p => allPaintings.Add(p));
        }


        List<Category> categories;
        public void setCategories()
        {
            categories = SqliteDataAccess.loadAllCategories();
            foreach (var category in categories)
            {
                // TODO: Make a function and a window to create new categories
                categoriesComboBox.Items.Add(category.Name);
            }
        }
        */
        public ShellView()
        {
            InitializeComponent();
            /*loadImages();
            if (allPaintings.Count > 0)
            {
                setImgAndDetails(allPaintings[imgIndex]);
            }
            setCategories();*/
        }/*

        const string IMG_DIALOG_FILTER = "All Image Files | *.*";

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("Sorry, the application has run into a problem.");
                    Trace.TraceInformation("Error during opening image dialog inside . " + DateTime.Now + "\n" + ex.Message);
                    // You must close or flush the trace to empty the output buffer.
                    Trace.Flush();
                }

            }
            else
            {
                MessageBox.Show("Something went wrong. Please try again.");
            }
            
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgIndex < allPaintings.Count - 1)
                {
                    imgIndex++;
                    setImgAndDetails(allPaintings[imgIndex]);
                }
                else if (imgIndex == allPaintings.Count - 1)
                {
                    imgIndex = 0;
                    setImgAndDetails(allPaintings[imgIndex]);
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (imgIndex == 0)
            {
                imgIndex = allPaintings.Count - 1;
                setImgAndDetails(allPaintings[imgIndex]);
            } else
            {
                imgIndex--;
                setImgAndDetails(allPaintings[imgIndex]);
            }
        }

        public void setImgAndDetails(Painting p)
        {
            string imgSource = imagesFolderPath + p.FileName;
            img.Source = new BitmapImage(new Uri(imgSource, UriKind.Absolute));
            paintingName.Text = p.Name;
            paintingSize.Text = p.Width + "\" x " + p.Length + "\"";
            picNum.Text = imgIndex + 1 + "/" + allPaintings.Count;
            paintedDate.Text = p.DatePainted;
            price.Text = p.Price.ToString(); 
            // TODO: Update this to display differently if price is negative.
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = (string) categoriesComboBox.SelectedItem;

            // List<Category> categorizedPaintings = PaintingLibrary.SqliteDataAccess.loadAllCategories()
            // int index = allPaintings.Where((p)=>p.Name == categoriesComboBox.SelectedItem).ElementAt(0);
            Category lookupCategory = categories.Where((cat) => cat.Name == selectedCategory).ToList().First();
            // int catID = lookupCategory.ID;

            allPaintings = allPaintings.Where((p) => p.Categories.Contains(lookupCategory)).ToList();
            // allPaintings = 
            // loadImages();
            if (allPaintings.Count > 0)
            {
                imgIndex++;
                setImgAndDetails(allPaintings[imgIndex]);
            }
        }*/
    }
}
