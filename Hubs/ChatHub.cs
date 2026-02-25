using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly AppDbContext _db;

    public ChatHub(AppDbContext db)
    {
        _db = db;
    }

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName)
            .SendAsync("UserJoined", $"{Context.ConnectionId} joined the room.");
    }

    public async Task SendMessage(string roomName, string message, int userId)
    {
        var msg = new Message
        {
            Content = message,
            UserId = userId,
            RoomId = _db.Rooms.First(r => r.Name == roomName).Id
        };
        _db.Messages.Add(msg);
        await _db.SaveChangesAsync();

        await Clients.Group(roomName)
            .SendAsync("ReceiveMessage", new {
                User = userId,
                Message = message,
                SentAt = DateTime.UtcNow
            });
    }

    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName)
            .SendAsync("UserLeft", $"{Context.ConnectionId} left the room.");
    }
}