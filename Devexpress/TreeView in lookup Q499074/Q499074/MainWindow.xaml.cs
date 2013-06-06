using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Q499074
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
            customers.Add(new Customer() { ID = 1, Name = "Name1", ParentID = 0 });
            customers.Add(new Customer() { ID = 2, Name = "Name2", ParentID = 1 });
            customers.Add(new Customer() { ID = 3, Name = "Name3", ParentID = 1 });
            customers.Add(new Customer() { ID = 4, Name = "Name4", ParentID = 2 });
            customers.Add(new Customer() { ID = 5, Name = "Name5", ParentID = 0 });
            customers.Add(new Customer() { ID = 6, Name = "Name6", ParentID = 5 });

            lookUpEdit1.ItemsSource = customers;
        }
    }
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IT
        {
            get;
            set;
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return IT;
        }
    }


    public class Customer
    {
        public int ParentID
        {
            get;
            set;
        }
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
