using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;

namespace PEngine.Persistance;

public class DatabaseContext : DbContext
{
    public DbSet<Attachment>? Attachments { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserCredentials>? UserCredentials { get; set; }

    public DatabaseContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connString = Program.WebsiteConfiguration?.GetConnectionString("Database");
        optionsBuilder.UseMySql(ServerVersion.AutoDetect(connString));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");
        
        base.OnModelCreating(modelBuilder);
    }
}