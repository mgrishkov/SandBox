using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LocalDB.Models
{
    [Table]
    public class Document : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        #region INotifyPropertyChanging implementation
        public event PropertyChangingEventHandler PropertyChanging;
        protected virtual void RaisePropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion

        private int _id;
        private string _title;
        private DateTime _creationTime;
        private string _author;
        private string _body;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    RaisePropertyChanging("ID");
                    _id = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        [Column(CanBeNull = false, DbType = "NVARCHAR(255) NOT NULL")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    RaisePropertyChanging("Title");
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        [Column(CanBeNull = false, DbType = "DATETIME NOT NULL")]
        public DateTime CreationTime
        {
            get { return _creationTime; }
            set
            {
                if (_creationTime != value)
                {
                    RaisePropertyChanging("CreationDate");
                    _creationTime = value;
                    RaisePropertyChanged("CreationDate");
                }
            }
        }

        [Column(CanBeNull = false, DbType = "NVARCHAR(255) NOT NULL")]
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    RaisePropertyChanging("Author");
                    _author = value;
                    RaisePropertyChanged("Author");
                }
            }
        }
        [Column(CanBeNull = true, DbType = "NVARCHAR(4000)")]
        public string Body
        {
            get { return _body; }
            set
            {
                if (_body != value)
                {
                    RaisePropertyChanging("Body");
                    _body = value;
                    RaisePropertyChanged("Body");
                }
            }
        }

    }
}
