using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using CoreChat.Data;
using CoreChat.Models;
using System.Security.Claims;

namespace CoreChat.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ChatDbContext _context;

    public ChatHub(ChatDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(int receiverId, string content)
    {
        var senderIdClaim = Context.User?.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(senderIdClaim) || !int.TryParse(senderIdClaim, out int senderId))
        {
            throw new HubException("User ID not found in token.");
        }

        var message = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // Send to receiver if online
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, content, message.Timestamp);
        
        // Also send back to sender for confirmation/sync
        await Clients.Caller.SendAsync("MessageSent", message);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("userId")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }
        await base.OnConnectedAsync();
    }
}
