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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintingDetailsManager.Views
{
    /// <summary>
    /// Interaction logic for NumberPicker.xaml
    /// </summary>
    public partial class NumberPicker : UserControl
    {
        public NumberPicker()
        {
            InitializeComponent();
        }

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(ProtocolNumberProperty, value); }
        }

        public static DependencyProperty MinimumProperty =
           DependencyProperty.Register("Minimum", typeof(int), typeof(NumberPicker));

        public int Maximum
        {
            get { return (int)GetValue(ProtocolNumberProperty); }
            set { SetValue(ProtocolNumberProperty, value); }
        }

        public static DependencyProperty ProtocolNumberProperty =
           DependencyProperty.Register("Maximum", typeof(int), typeof(NumberPicker));


        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set 
            {
                if (value >= Minimum && value <= Maximum)
                {
                    SetValue(ValueProperty, value);
                }
            }
        }

        public static DependencyProperty ValueProperty =
           DependencyProperty.Register("Value", typeof(int), typeof(NumberPicker));

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (Value > Minimum)
                Value--;
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
                Value++;
        }
    }
}
