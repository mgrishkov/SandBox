using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DevExpress.Xpf.LayoutControl;

namespace application
{
    public class SomeItem
    {
        public int ID { get; set; }
        public string Caption { get; set; }
        public int SortIndex { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public ObservableCollection<SomeItem> Tiles { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            Tiles = new ObservableCollection<SomeItem>() { 
                        new SomeItem() { ID = 3, Caption = "Second item", SortIndex = 2 },
                        new SomeItem() { ID = 4, Caption = "Fourth item", SortIndex = 4 },
                        new SomeItem() { ID = 2, Caption = "First item", SortIndex = 1 },
                        new SomeItem() { ID = 1, Caption = "Third item", SortIndex = 3 }
                    };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = TC.Children.OfType<Tile>();
            int i = 1;
            foreach (var itm in t)
            {
                var si = itm.DataContext as SomeItem;
                si.SortIndex = i;
                i++;
            }
        }




    }
}
