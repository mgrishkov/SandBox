using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public async Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            // Do something with the message here
        }
    }
}
