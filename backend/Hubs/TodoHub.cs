using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs
{
    public class TodoHub : Hub {
        public void Hello(string name) {
            Clients.All.hi($"[Broadcast] Received 'Hello({name})' from {Context.ConnectionId}");
            Clients.Caller.hi($"Hi {name}");
        }
    }
}