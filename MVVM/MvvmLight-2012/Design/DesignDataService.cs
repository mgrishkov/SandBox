using System;
using MvvmLight.Model;

namespace MvvmLight.Design
{
    public class DesignDataService : IDataService
    {
        public void GetItems(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data
            for (int i = 0; i < 10; i++)
            {
                var item = new DataItem() { ID = i, Name = String.Format("DesignItem_{0}", i) };
                callback(item, null);
            };
        }
        public void GetClients(Action<Client, Exception> callback)
        {
            // Use this to connect to the actual data service
            for (int i = 0; i < 10; i++)
            {
                var item = new Client() { ID = i, FirstName = String.Format("DesignMan_{0}", i) };
                callback(item, null);
            };
        }
    }
}