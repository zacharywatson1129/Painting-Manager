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
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            /*PrintDialog printDlg = new PrintDialog();

            
            FlowDocument doc = new FlowDocument(Paintings);


            FixedDocument fix = new FixedDocument();
            PageContent pc = new PageContent();
            FixedPage fp = new FixedPage();
            fp.Width = fix.DocumentPaginator.PageSize.Width;
            fp.Height = fix.DocumentPaginator.PageSize.Height;

            fp.Children.Add(yourDataGrid);
            ((System.Windows.Markup.IAddChild)pc).AddChild(fp);

            fix.Pages.Add(pc);
            dv.Document = fix;

            doc.Name = "FlowDoc";*/

            /*System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                Paintings.Measure(pageSize);
                Paintings.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(Paintings, "Paintings");
            }*/
            PrintDG print = new PrintDG();
            print.printDG(Paintings, "Title");
        }
    }
}
