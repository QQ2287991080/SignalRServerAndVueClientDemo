using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRServerAndVueClientDemo.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServerAndVueClientDemo.Filters
{
    public class SysExceptionFilter : IAsyncExceptionFilter
    {
        readonly IHubContext<ChatHub> _hub;
        public SysExceptionFilter(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            string message = ex.Message;
            string url = context.HttpContext?.Request.Path;

            var basePath = Directory.GetCurrentDirectory() + "\\logs\\system.log";
            var fs = new FileStream(basePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new StreamReader(fs);
            var json = reader.ReadToEnd();
            var str = json.Replace("\r\n", "").Replace("|", "\"");
            int length = str.Length;
            var sub = str.Remove(length - 1);
            var result = "[" + sub + "]";
            var data = JsonConvert.DeserializeObject<List<LogData>>(result);

            await _hub.Clients.All.SendAsync("ReceiveLog", data);
            context.Result = new JsonResult(new { ErrCode = 0, ErrMsg = message, Data = true });
        }
    }
}
