using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SampleServer.Hubs
{
    public class TestHub : Hub
    {
        private readonly ILogger<TestHub> _logger;

        public TestHub(ILogger<TestHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnected()
        {
            base.OnConnected();

            _logger.LogInformation($"{Context.ConnectionId} - connected");

            return Task.CompletedTask;
        }

        public override Task OnReconnected()
        {
            base.OnReconnected();

            _logger.LogInformation($"{Context.ConnectionId} - reconnected");


            return Task.CompletedTask;
        }

        public override Task OnDisconnected(Boolean stopCalled)
        {
            base.OnDisconnected(stopCalled);

            _logger.LogInformation($"{Context.ConnectionId} - disconnected");

            return Task.CompletedTask;
        }

        public string Echo(string message)
        {
            return message;
        }

        public async Task CallEcho(string message)
        {
            await Clients.Caller.Echo(message);
        }

    }
}
