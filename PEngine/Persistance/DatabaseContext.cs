using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;

namespace PEngine.Persistance;

public class DatabaseContext : DbContext
{
    private DbSet<Attachment>? Attachments { get; set; }
    private DbSet<Comment>? Comments { get; set; }
    private DbSet<Post>? Posts { get; set; }
    private DbSet<User>? Users { get; set; }
    private DbSet<UserCredentials>? UserCredentials { get; set; }

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