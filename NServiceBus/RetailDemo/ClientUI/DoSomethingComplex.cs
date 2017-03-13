using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    public class DoSomethingComplex : ICommand
    {
        public int SomeId { get; set; }
        public ChildClass ChildStuff { get; set; }
        public List<ChildClass> ListOfStuff { get; set; }

        public DoSomethingComplex()
        {
            ListOfStuff = new List<ChildClass>();
        }
    }
    public class ChildClass
    {
        public string SomeProperty { get; set; }
    }

}
