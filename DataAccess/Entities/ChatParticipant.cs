namespace DataAccess.Entities;

public class ChatParticipant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ChatId { get; set; }
    public required Chat Chat { get; set; }
    public required string ParticipantId { get; set; }
    public required AppUser Participant { get; set; }
}
