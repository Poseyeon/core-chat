using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CoreChat.Data;
using CoreChat.Models;
using System.Security.Claims;

namespace CoreChat.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ChatDbContext _context;

    public MessagesController(ChatDbContext context)
    {
        _context = context;
    }

    [HttpGet("history/{otherUserId}")]
    public async Task<IActionResult> GetHistory(int otherUserId)
    {
        var currentUserIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
        {
            return Unauthorized();
        }

        var messages = await _context.Messages
            .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        return Ok(messages);
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var currentUserIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
        {
            return Unauthorized();
        }

        // Get list of unique users we have chatted with
        var senderIds = await _context.Messages
            .Where(m => m.ReceiverId == currentUserId)
            .Select(m => m.SenderId)
            .Distinct()
            .ToListAsync();

        var receiverIds = await _context.Messages
            .Where(m => m.SenderId == currentUserId)
            .Select(m => m.ReceiverId)
            .Distinct()
            .ToListAsync();

        var allContactIds = senderIds.Union(receiverIds).Distinct();

        return Ok(allContactIds);
    }
}
