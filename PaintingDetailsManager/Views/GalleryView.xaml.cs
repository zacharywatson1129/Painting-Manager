using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for GalleryView.xaml
    /// </summary>
    public partial class GalleryView : UserControl
    {
        public GalleryView()
        {
            InitializeComponent();

            /*ControlTemplate ct = Categories.Template as ControlTemplate;
            Grid grid = ct.FindName("MainGrid", Categories) as Grid;
            ToggleButton toggle = grid.Children[1] as ToggleButton;  //Not Named in the Default Template.
            ControlTemplate ct2 = toggle.Template as ControlTemplate;
            Path path = ct2.FindName("Arrow", toggle) as Path;
            path.Fill = new SolidColorBrush(Colors.Red);*/
        }
    }
}
