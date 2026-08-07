namespace DataAccess.Entities;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ChatId { get; set; }
    public required Chat Chat { get; set; }
    public required string SenderId { get; set; }
    public required AppUser Sender { get; set; }
    public required string Content { get; set; }
    public DateTime SentAt { get; set; }
}