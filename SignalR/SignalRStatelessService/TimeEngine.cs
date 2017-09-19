using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RecurrentTasks;
using SignalRStatelessService.Hubs;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Microsoft.AspNetCore.Sockets.Client;

namespace SignalRStatelessService
{
    public class TimeEngine : IRunnable
    {
        private readonly ILogger _logger;
        private readonly IHubContext<TestHub> _testHub;

        public TimeEngine(ILogger<TimeEngine> logger, IHubContext<TestHub> testHub)
        {
            _logger = logger;
            _testHub = testHub;
        }
        
        public void Run(ITask currentTask, CancellationToken cancellationToken)
        {
            var msg = $"Now {DateTimeOffset.Now}";

            _testHub.Clients.All.InvokeAsync("NewBroadcastMessage", msg);
        }
    }
}
