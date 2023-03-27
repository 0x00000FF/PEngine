using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;

namespace PEngine.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<Attachment>? Attachments { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserCredentials>? UserCredentials { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connString = Program.WebsiteConfiguration?["Persistence:Database:ConnectionString"];
        optionsBuilder.UseMySql(ServerVersion.AutoDetect(connString));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");
        
        base.OnModelCreating(modelBuilder);
    }
}