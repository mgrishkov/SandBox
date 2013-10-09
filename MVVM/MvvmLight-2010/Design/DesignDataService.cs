using System;
using MvvmLight.Model;

namespace MvvmLight.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data
            for (int i = 0; i < 10; i++)
            {
                var item = new DataItem() { ID = i, Name = String.Format("Item_{0}", i) };
                callback(item, null);
            };
        }
    }
}