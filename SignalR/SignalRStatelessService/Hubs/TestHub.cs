using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalRStatelessService.Hubs
{
    public class TestHub : Hub
    {
        private readonly ILogger<TestHub> _logger;

        public TestHub(ILogger<TestHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();

            _logger.LogInformation($"{Context.ConnectionId} - connected");

            return Clients.All.InvokeAsync("NewBroadcastMessage", $"Congratulation! [{Context.ConnectionId}] joined!");
        }
        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            base.OnDisconnectedAsync(exception);

            _logger.LogInformation($"{Context.ConnectionId} - disconnected");

            return Clients.All.InvokeAsync("NewBroadcastMessage", $"Bad news! [{Context.ConnectionId}] left!");
        }
        
        public async Task SendToAll(string message)
        {
            _logger.LogInformation($"New info: {message}");

            await Clients.All.InvokeAsync("NewBroadcastMessage", $"[{Context.ConnectionId}] says: {message}");
        }

    }
}
