using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CoreChat.Hubs;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("userId")?.Value;
    }
}
