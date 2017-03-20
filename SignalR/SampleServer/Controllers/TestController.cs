using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using SampleServer.Hubs;

namespace SampleServer.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IConnectionManager _connectionManager;

        public TestController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string message)
        {
            _connectionManager.GetHubContext<TestHub>().Clients.All.Echo(message);

            return Ok();
        }

    }
}
