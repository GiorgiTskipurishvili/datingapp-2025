using API.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub(PresenceTracker presenceTracker) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await presenceTracker.UserConnected(GetUserId(),Context.ConnectionId);

            await Clients.Others.SendAsync("UserIsOnline", GetUserId());

            var currentUsers = await presenceTracker.GetOnlineUsers();
            //await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await presenceTracker.UserDisconnected(GetUserId(),Context.ConnectionId);

            await Clients.Others.SendAsync("UserIsOffline", GetUserId());

            var currentUsers = await presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }


        private string GetUserId()
        {
            return Context.User?.GetMemberId() ?? throw new Exception("Cannot get member id");
        }
    }
}
