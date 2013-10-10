using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalDB.Models;

namespace WPFLocalDB
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            using (var dc = new DocumentDataContext())
            {
                if (!dc.DatabaseExists())
                {
                    dc.CreateDatabase();
                }
            }
        }
    }
}
