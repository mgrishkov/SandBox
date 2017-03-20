using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RecurrentTasks;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using SampleServer.Hubs;

namespace SampleServer
{
    public class TimeEngine : IRunnable
    {
        private readonly ILogger            _logger;
        private readonly IConnectionManager _connectionManager;

        public TimeEngine(IConnectionManager connectionManager, ILogger<TimeEngine> logger)
        {
            _logger     = logger;
            _connectionManager = connectionManager;
        }

        public void Run(ITask currentTask)
        {
            var msg = $"Now: {DateTimeOffset.Now}";
            _logger.LogInformation(msg);

            _connectionManager.GetHubContext<TestHub>().Clients.All.Echo(msg);
        }
    }
}
