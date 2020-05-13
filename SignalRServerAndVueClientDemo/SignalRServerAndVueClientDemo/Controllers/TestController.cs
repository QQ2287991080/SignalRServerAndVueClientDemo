using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServerAndVueClientDemo.Hubs;

namespace SignalRServerAndVueClientDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public TestController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet]
        public async Task<int> Get()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "系统通知", $"北京时间{DateTime.Now}");
            return 0;
        }
    }
}