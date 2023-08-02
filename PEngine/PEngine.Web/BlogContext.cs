using Microsoft.EntityFrameworkCore;
using PEngine.Web.Models;

namespace PEngine.Web;

public class BlogContext : DbContext
{
    private static string? _connectionString;

    public static void SetConnectionString(string? connectionString)
    {
        _connectionString ??= connectionString;
    }
    
    public DbSet<Comment> Comments { get; set; }
    /* public DbSet<Guestbook> Guestbooks {get;set;} */
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }

    public BlogContext(DbContextOptions<BlogContext> contextOptions) : base(contextOptions)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            connectionString: _connectionString, 
            MySqlServerVersion.LatestSupportedServerVersion);
     
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}