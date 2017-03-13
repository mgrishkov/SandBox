using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    public class DoSomething : ICommand
    {
        public string SomeProperty { get; set; }
    }
}
