using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MvvmLight.Model;

namespace MvvmLight.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DataItemViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private ObservableCollection<DataItem> _items;

        public ObservableCollection<DataItem> Items
        {
            get { return _items; }
            set 
            {
                if(_items != value)
                {
                    _items = value;
                    RaisePropertyChanged("Items");
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the DataItemViewModel class.
        /// </summary>
        public DataItemViewModel(IDataService dataService)
        {
            _dataService = dataService;
            initializeProperties();
        }

        private void initializeProperties()
        {
            Items = new ObservableCollection<DataItem>();
            loadItems();
        }

        private void loadItems()
        {
            Items.Clear();
            _dataService.GetItems(
                (item, error) =>
                    {
                        Items.Add(item);
                    });
        }
    }
}