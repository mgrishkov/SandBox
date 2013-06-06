using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ORM
{
    public class NoDataFoundException : Exception
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            //var t = select(new Func<admins, bool>(x => x.admin_id == EAdminType.SysDba)).First();
            var usr = new admins();
            usr.PropertyChanged += usr_PropertyChanged;
            usr.admin_login = "!!!!asdsdasd!!!";
        }

        static void usr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
        static ObservableCollection<admins> select(Func<admins, bool> predicate)
        {
            ObservableCollection<admins> list = null;
            using (var db = new wft23Entities())
            {
                if (db.admins.Any(predicate))
                {
                    list = new ObservableCollection<admins>(db.admins.Where(predicate));
                };
            };
            return list;
        }
        static void delete(Func<admins, bool> predicate)
        {
            using (var db = new wft23Entities())
            {
                if (db.admins.Any(predicate))
                {
                    var usrs = db.admins.Where(predicate);
                    
                    foreach (var itm in usrs)
                    {
                        db.admins.Remove(itm);
                    };
                    db.SaveChanges();
                }
                else
                {
                    throw new NoDataFoundException();
                };
            };
        }
        static void insert(admins usr)
        {
            using (var db = new wft23Entities())
            {
                db.admins.Add(usr);
                db.SaveChanges();
            };
        }
    }
}
