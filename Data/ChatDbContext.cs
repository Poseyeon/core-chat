using Microsoft.EntityFrameworkCore;
using CoreChat.Models;

namespace CoreChat.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<ChatMessage> Messages { get; set; }
}
