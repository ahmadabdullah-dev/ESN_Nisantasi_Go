using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanParticipant> PlanParticipants => Set<PlanParticipant>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatParticipant> ChatParticipants => Set<ChatParticipant>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(e =>
        {
            e.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            e.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            e.Property(u => u.Country).HasMaxLength(100).IsRequired();
            e.Property(u => u.Department).HasMaxLength(100).IsRequired();
        });

        builder.Entity<Plan>(e =>
        {
            e.Property(p => p.Title).HasMaxLength(200).IsRequired();
            e.Property(p => p.LocationName).HasMaxLength(200).IsRequired();
            e.Property(p => p.Description).HasMaxLength(1000);

            e.HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); 

            e.HasIndex(p => p.PlannedAt);
        });

        builder.Entity<Chat>(e =>
        {
            e.HasOne(c => c.Plan)
                .WithOne(p => p.Chat)
                .HasForeignKey<Chat>(c => c.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(c => c.PlanId).IsUnique();
        });

        builder.Entity<PlanParticipant>(e =>
        {
            e.HasIndex(pp => new { pp.PlanId, pp.ParticipantId }).IsUnique();

            e.HasOne(pp => pp.Plan)
                .WithMany(p => p.Participants)
                .HasForeignKey(pp => pp.PlanId)
                .OnDelete(DeleteBehavior.Cascade); 

            e.HasOne(pp => pp.Participant)
                .WithMany()
                .HasForeignKey(pp => pp.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ChatParticipant>(e =>
        {
            e.HasIndex(cp => new { cp.ChatId, cp.ParticipantId }).IsUnique();

            e.HasOne(cp => cp.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.ChatId)
                .OnDelete(DeleteBehavior.Cascade); 

            e.HasOne(cp => cp.Participant)
                .WithMany()
                .HasForeignKey(cp => cp.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Message>(e =>
        {
            e.Property(m => m.Content).HasMaxLength(2000).IsRequired();

            e.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); 

            e.HasIndex(m => new { m.ChatId, m.SentAt }); 
        });
    }
}