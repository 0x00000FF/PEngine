using Microsoft.EntityFrameworkCore;
using PEngine.Web.Models.Entities;
using PEngine.Web.Models;

namespace PEngine.Web.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }


        public DbSet<AuthFactor> AuthFactors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<KeyChain> KeyChains { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAuditLog> MemberAuditLogs { get; set; }
        public DbSet<MemberWhitelist> MemberWhitelists { get; set; }
        public DbSet<MemberAuthFactor> MemberAuthFactors { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Models.Entities.File> Files { get; set; }
        public DbSet<FileHit> FileHits { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ProfileVersion> ProfileVersions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<PostVersion> PostVersions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<PostHit> PostHits { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<GuestbookVersion> GuestbookVersions { get; set; }
        public DbSet<Guestbook> Guestbooks { get; set; }
        public DbSet<GuestbookCommentVersion> GuestbookCommentVersions { get; set; }
        public DbSet<GuestbookComment> GuestbookComments { get; set; }
        public DbSet<CommentVersion> CommentVersions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 복합 키, 인덱스 및 기타 관계 설정
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.Tag });

            // 필요한 Fluent API 설정을 여기에 추가
        }
    }
}
