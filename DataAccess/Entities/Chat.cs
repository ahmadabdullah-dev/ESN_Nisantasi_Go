namespace DataAccess.Entities;

public class Chat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string PlanId { get; set; }
    public required Plan Plan { get; set; }

    public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();   

}
