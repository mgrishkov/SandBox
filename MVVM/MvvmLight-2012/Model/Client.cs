using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MvvmLight.Model
{
    public class Client : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            };
        }
        #endregion 
        #region IDataErrorInfo implementation
        public string Error
        {
            get { return String.Empty; }
        }
        public string this[string propertyName]
        {
            get
            {
                String errorMessage = String.Empty;
                switch (propertyName)
                {
                    case "ID":
                        if (this.ID <= 0)
                        {
                            errorMessage = "Incorrect ID";
                        };
                        break;
                    case "Name":
                        if (String.IsNullOrWhiteSpace(this.FirstName))
                        {
                            errorMessage = "FirstName is a mandatory field";
                        }
                        break;
                };
                return errorMessage;
            }
        }
        #endregion
        
        private int _id;
        private string _firstName;

        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                };
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                };
            }
        }

    }
}
