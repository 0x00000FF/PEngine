using Microsoft.EntityFrameworkCore;
using PEngine.Common.DataModels;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace PEngine.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<Attachment>? Attachments { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserCredentials>? UserCredentials { get; set; }

    public DatabaseContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connString = Program.WebsiteConfiguration?["Persistence:Database:ConnectionString"];
        optionsBuilder.UseMySql(connString, 
            ServerVersion.Create(Version.Parse("8.0"), ServerType.MySql));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");
        
        base.OnModelCreating(modelBuilder);
    }
}