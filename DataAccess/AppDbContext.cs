using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanParticipant> PlanParticipants => Set<PlanParticipant>();

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

      
    }
}