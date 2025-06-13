using ChatContracts;
using Microsoft.AspNetCore.SignalR;

namespace HubServer
{
    public class ChatHub : Hub<IChatClient>
    {
        private static readonly object _lockusers = new();
        private static List<ConnectedUser> _connectedUsers = [];

        public override async Task OnConnectedAsync()
        {
            var userId = string.Empty;
            var userName = string.Empty;
            userId = Context.GetHttpContext()?.Request.Query["userId"];
            userName = Context.GetHttpContext()?.Request.Query["userName"];

            lock (_lockusers)
            {

                _connectedUsers.Add(new ConnectedUser
                {
                    UserId = userId,
                    UserName = userName,
                    ConnectionId = Context.ConnectionId
                });
            }


            await Clients.Caller.ReceiveSystemMessage($"Hi {userName}, you have connected");
            await Clients.All.UpdateUserList(_connectedUsers);
        }//end of OnConnectedAsync

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedUser? user;

            lock (_lockusers)
            {
                user = _connectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    _connectedUsers.Remove(user);
                }
            }

            if (user != null)
            {
                await Clients.All.UpdateUserList(_connectedUsers);
            }

            await base.OnDisconnectedAsync(exception);
        }//end of onDisconnetAsync

        public async Task ForwardMessage(string fromUserId, string toConnectionId, string message)
        {
            if (!string.IsNullOrWhiteSpace(toConnectionId))
            {
                await Clients.Client(toConnectionId).ReceiveMessage(fromUserId, Context.ConnectionId, message);
            }
        }
    }
}
