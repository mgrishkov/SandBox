using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight.Model;

namespace MvvmLight.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private ObservableCollection<Client> _items;
        private Client _selectedItem;

        public ObservableCollection<Client> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged("Items");
                };
            }
        }
        public Client SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                };
            }
        }
        public string NewItemName { get; set; }
        public bool IsListNotEmpty
        {
            get
            {
                return (Items != null && Items.Count > 0);
            }
        }

        public RelayCommand ClearListCommand { get; private set; }
        public RelayCommand AddItemToListCommand { get; private set; }
        public RelayCommand RemoveItemFromListCommand { get; private set; }
        public RelayCommand ShowDataItemsCommand { get; private set; }

        private void initializeProperty()
        {
            Items = new ObservableCollection<Client>();
            Items.CollectionChanged += Items_CollectionChanged;
            ClearListCommand = new RelayCommand(clearList);
            AddItemToListCommand = new RelayCommand(addItem);
            RemoveItemFromListCommand = new RelayCommand(removeItem);
            ShowDataItemsCommand = new RelayCommand(showDataItems);
            NewItemName = "New Item";
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("IsListNotEmpty");
        }
        private void clearList()
        {
            Items.Clear();
        }
        private void addItem()
        {
            Items.Add(new Client() { ID = 100, FirstName = NewItemName });
        }
        private void removeItem()
        {
            Items.Remove(SelectedItem);
        }
        private void showDataItems()
        {
            var dlg = new View.DataItemView();
            dlg.ShowDialog();
        }

        public MainViewModel(IDataService dataService)
        {
            initializeProperty();
            _dataService = dataService;
            loadClients();
        }

        private void loadClients()
        {
             _dataService.GetClients(
                (item, error) =>
                    {
                        Items.Add(item);
                    });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}