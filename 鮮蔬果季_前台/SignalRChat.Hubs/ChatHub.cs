﻿using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace 鮮蔬果季_前台.SignalRChat.Hubs
{
        public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string a = Context.ConnectionId; //每一個連結者都有一個獨立的連結ID
            await Clients.All.SendAsync("ReceiveMessage", a, message);
        }
        public async Task AddGroup(string groupName, string username,string oldgroup)
        {
            if (groupName != oldgroup) {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldgroup);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("RecAddGroupMsg", $"{username} 已加入 群組：{groupName}。");
        }
        public Task SendMessageToGroup(string groupName, string username, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, username, message);
        }
        // 連線事件
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        // 斷線事件
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
