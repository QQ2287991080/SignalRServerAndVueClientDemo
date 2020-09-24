using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRServerAndVueClientDemo.Hubs;
using SignalRServerAndVueClientDemo.LogHelper;
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
        //使用log4
        ILog _log = LogManager.GetLogger(Startup.Logger.Name, typeof(SysExceptionFilter));
        public SysExceptionFilter(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            //错误
            var ex = context.Exception;
            //错误信息
            string message = ex.Message;
            //请求方法的路由
            string url = context.HttpContext?.Request.Path;
            //写入日志文件描述  注意这个地方尽量不要用中文冒号，否则读取日志文件的时候会造成信息确实，当然你可以定义自己的规则
            string logMessage = $"错误信息=>【{message}】，【请求地址=>{url}】";
            //写入日志
            _log.Error(logMessage);
            //读取日志
            var data = ReadHelper.Read();
            //发送给客户端
            await _hub.Clients.All.SendAsync("ReceiveLog", data);
            //返回一个正确的200http码，避免前端错误
            context.Result = new JsonResult(new { ErrCode = 0, ErrMsg = message, Data = true });
        }
    }
}
