using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)

{
    // public AppDbContext(DbContextOptions<AppDbContext> options)
    //     : base(options)
    // {
    // }
    public DbSet<User> Users { get; set; } = null!;
    // public DbSet<User> Users => Set<User>(); we can use this way also
    public DbSet<TaskItem> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var random = new Random();
        int adminId = random.Next(10000, 100000);

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            UserName = "admin",
            // Hash created by BCrypt.Net-Next
            UserPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin"
        });

         

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // User entity config
        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.HasKey(u => u.Id);
        //     entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
        //     entity.Property(u => u.UserPasswordHash).IsRequired();
        //     entity.Property(u => u.Role).IsRequired();
        // });

        // // TaskItem entity config
        // modelBuilder.Entity<TaskItem>(entity =>
        // {
        //     entity.HasKey(t => t.Id);
        //     entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
        //     entity.Property(t => t.Description).HasMaxLength(500);

        //     entity.HasOne(t => t.AssignedTo)
        //           .WithMany(u => u.Tasks)
        //           .HasForeignKey(t => t.AssignedToId)
        //           .OnDelete(DeleteBehavior.Restrict);
        // });
    }

}
