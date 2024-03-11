using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRYoutube.Data;
using SignalRYoutube.Models;

namespace SignalRYoutube.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ApplicationDBContext dbContext;

        public NotificationHub(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
      

        public async Task SendNotificationToAll(string message)
        {
            await Clients.All.SendAsync("ReceivedNotification", message);
        }
     
        public async Task SendNotificationToClient(string message, string username)
        {
            var hubConnections = await dbContext.HubConnections.Where(con => con.Username == username).ToListAsync();
            foreach (var hubConnection in hubConnections)
            {
                await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedPersonalNotification", message, username);
            }
        }
        public async Task SendNotificationToGroup(string message, string group)
        {
            var hubConnections = await dbContext.HubConnections.Join(dbContext.TblUser, c => c.Username, o => o.Username, (c, o) => new { c.Username, c.ConnectionId, o.Dept }).Where(o => o.Dept == group).ToListAsync();
            foreach (var hubConnection in hubConnections)
            {
                string username = hubConnection.Username;
                await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedPersonalNotification", message, username);
                //Call Send Email function here
            }
        }
        private static readonly HashSet<string> ConnectedUsers = new HashSet<string>();

        public override async Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");

            string username = await GetUsernameFromContext(); // Implement logic to retrieve username

            ConnectedUsers.Add(username);

            await UpdateConnectedUsersList(); // Broadcast the updated list to all clients
        }
        public async Task SaveUserConnection(string username)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection
            {
                ConnectionId = connectionId,
                Username = username
            };

            dbContext.HubConnections.Add(hubConnection);
            await dbContext.SaveChangesAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = await GetUsernameFromContext(); // Implement logic to retrieve username

            ConnectedUsers.Remove(username);

            await UpdateConnectedUsersList(); // Broadcast the updated list to all clients

            await base.OnDisconnectedAsync(exception);
        }

        private async Task UpdateConnectedUsersList()
        {
            List<string> userList = ConnectedUsers.ToList(); // Convert to a list for easier client-side handling
            await Clients.All.SendAsync("UpdateUsersList", userList);
        }

        private async Task<string> GetUsernameFromContext()
        {
            var httpContext = Context.GetHttpContext();
            var username = httpContext.Session.GetString("Username");

            return !string.IsNullOrEmpty(username) ? username : "UnknownUser";
        }

    }
}
