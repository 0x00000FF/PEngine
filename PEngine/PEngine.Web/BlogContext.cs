using Microsoft.EntityFrameworkCore;
using PEngine.Web.Models;

namespace PEngine.Web;

public sealed class BlogContext : DbContext
{
    private static string? _connectionString;

    public static void SetConnectionString(string? connectionString)
    {
        _connectionString ??= connectionString;
    }
    
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Guestbook> Guestbooks { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FileTag> FileTags { get; set; } = null!;

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
        modelBuilder.Entity<Category>()
            .HasData(
            new Category() { Count = default }
        );

        modelBuilder.Entity<Introduction>()
            .HasNoKey();

        base.OnModelCreating(modelBuilder);
    }
}