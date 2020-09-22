using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        [HttpGet]
        public async Task<int> GetLog()
        {
            int a = 0;
            var b = 1/a;
            return 0;
        }

        [HttpGet]
        public  JsonResult GetLogMessage()
        {
            try
            {
                var basePath = Directory.GetCurrentDirectory() + "\\logs\\system.log";
                var fs = new FileStream(basePath, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                var reader = new StreamReader(fs);
                var json = reader.ReadToEnd();
                var str = json.Replace("\r\n", "").Replace("|", "\"");
                int length = str.Length;
                var sub = str.Remove(length-1);
                var result = "[" + sub + "]";
                var data = JsonConvert.DeserializeObject<List<LogData>>(result);
                return new JsonResult(data);
            }
            catch (HubException ex)
            {
                return new JsonResult(ex);
            }
        }
    }

    public class LogData
    {
        public string CreateTime { get; set; }
        public string  ThreadId { get; set; }
        public string LogLevel { get; set; }
        public string Summary { get; set; }
    }
}