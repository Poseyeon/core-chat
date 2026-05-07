using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreChat.Models;

[Table("CHAT_MESSAGES")]
public class ChatMessage
{
    [Key]
    [Column("MSG_ID")]
    public int Id { get; set; }

    [Column("SENDER_ID")]
    public int SenderId { get; set; }

    [Column("RECEIVER_ID")]
    public int ReceiverId { get; set; }

    [Column("MSG_CONTENT")]
    public string Content { get; set; } = string.Empty;

    [Column("MSG_TIMESTAMP")]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [Column("IS_READ")]
    public bool IsRead { get; set; } = false;
}
