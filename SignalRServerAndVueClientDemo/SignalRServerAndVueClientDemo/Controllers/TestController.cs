using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRServerAndVueClientDemo.Hubs;

namespace SignalRServerAndVueClientDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<TestController> _logger;
        public TestController(IHubContext<ChatHub> hubContext,ILogger<TestController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<int> Get()
        {
            _logger.LogInformation("系统通知：ReceiveMessage");
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "系统通知", $"北京时间{DateTime.Now}");
            return 0;
        }
    }
}