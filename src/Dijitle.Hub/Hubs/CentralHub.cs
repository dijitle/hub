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
    private Messages _messages = new Messages();

    public async Task SendMessage(string message)
    {
      await Clients.All.MessageReceived(Context.User.Claims.FirstOrDefault(c => c.Type == "https://dijitle.com/userName").Value, message);
    }

    public override Task OnConnectedAsync()
    {

      Clients.All.MessageReceived("system", $"user {Context.User.Claims.FirstOrDefault(c => c.Type == "https://dijitle.com/userName").Value} has connected.");
      return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
      return base.OnDisconnectedAsync(exception);
    }
  }
}
 