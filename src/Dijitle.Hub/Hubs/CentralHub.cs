using Dijitle.Hub.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dijitle.Hub.Hubs
{
  [Authorize]
  public class CentralHub : Hub<IHub>
  {
    private Messages _messages;

    public CentralHub(Messages messages)
    {
      _messages = messages;
    }

    public async Task SendMessage(string message)
    {
      var m = _messages.AddMessage(UserName, message);
      await Clients.All.MessageReceived(m);
    }
    
    public async Task GetMessages(int amount, string earliestMessageId)
    {
      await Clients.Caller.GetMessages(_messages.GetMessagesBefore(amount, earliestMessageId));
    }

    
    public override async Task OnConnectedAsync()
    {
      var m = _messages.AddMessage("system", $"user {UserName} has connected.");      
      await Clients.All.MessageReceived(m);
      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      var m = _messages.AddMessage("system", $"user {UserName} has disconnected.");
      await Clients.All.MessageReceived(m);
      await base.OnDisconnectedAsync(exception);
    }


    private string UserName { get { return Context.User.Claims.FirstOrDefault(c => c.Type == "https://dijitle.com/userName").Value; } }
  }
}
 