using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintingDetailsManager
{
    /// <summary>
    /// Interaction logic for InputDetails.xaml
    /// </summary>
    public partial class InputDetails : Window
    {

        public Painting PaintingField { get; set; } = new Painting();

        public InputDetails(string fileName, string imagesFolder)
        {
            InitializeComponent();
            PaintingField.FileName = fileName;
            string imgSource = imagesFolder + fileName;
            imgBox.Source = new BitmapImage(new Uri(imgSource, UriKind.Absolute));
        }

        private void btnSavePainting_Click(object sender, RoutedEventArgs e)
        {
            PaintingField.Name = paintingNameTextBox.Text;
            PaintingField.Width = Convert.ToInt32(widthTextBox.Text);
            PaintingField.Length = Convert.ToInt32(heightTextBox.Text);
            PaintingField.DatePainted = datePicker.SelectedDate.;
            PaintingField.Price = Convert.ToDouble(priceTextBox.Text);
            SqliteDataAccess.savePainting(PaintingField);
            this.Close();
        }
    }
}
