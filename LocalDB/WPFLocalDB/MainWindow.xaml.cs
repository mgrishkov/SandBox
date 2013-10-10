using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFLocalDB.Models;

namespace WPFLocalDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        DocumentDataContext _dc;

        public List<Document> Documents
        {
            get
            {
                return _dc.Documents.ToList();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _dc = new DocumentDataContext();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var doc = new Document()
            {
                Title = "Test",
                Author = "TestUser",
                Body = "zxczczczcz",
                CreationTime = DateTime.Now
            };
            _dc.Documents.InsertOnSubmit(doc);
            _dc.SubmitChanges();
            RaisePropertyChanged("Documents");

        }
    }
}
