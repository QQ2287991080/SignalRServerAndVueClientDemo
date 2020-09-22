﻿using log4net;
using log4net.Core;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServerAndVueClientDemo.Hubs
{

    public class ChatHub : Hub
    {
        static ILog _logger = LogManager.GetLogger(Startup.Logger.Name, typeof(ChatHub));
        public ChatHub()
        {
        }
        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            //var token = Context.ConnectionAborted;
            //var cancel = token.CanBeCanceled;
            //if (cancel)
            //{ 
              
            //}
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        /// <summary>
        /// 向调用客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveCaller", message);
        }

        /// <summary>
        /// 客户端连接服务端
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;
            _logger.Info($"客户端ConnectionId：【{id}】已连接服务器！");
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            _logger.Info($"客户端ConnectionId：【{id}】已断开服务器连接！");
            return base.OnDisconnectedAsync(exception);
        }

    }
}
