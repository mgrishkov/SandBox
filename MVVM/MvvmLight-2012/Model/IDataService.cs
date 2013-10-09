using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight.Model
{
    public interface IDataService
    {
        void GetItems(Action<DataItem, Exception> callback);
        void GetClients(Action<Client, Exception> callback);
    }
}
