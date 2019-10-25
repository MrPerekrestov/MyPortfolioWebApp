using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyPortfolioWebApp.DbContexts.BlogDbContext
{
    public partial class BlogContext : DbContext
    {        

        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options) {}

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Logo> Logos { get; set; }
        public virtual DbSet<Post> Posts { get; set; }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments", "blog");

                entity.HasIndex(e => e.PostId)
                    .HasName("postId_fk_idx");

                entity.Property(e => e.CommentId)
                    .HasColumnName("commentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommentText)
                    .IsRequired()
                    .HasColumnName("commentText")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.PostId)
                    .HasColumnName("postId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostedDate)
                    .HasColumnName("postedDate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.RepliedOnText)
                    .HasColumnName("repliedOnText")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RepliedOnUserName)
                    .HasColumnName("repliedOnUserName")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("post_Id_fk");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.BlogPostId });

                entity.ToTable("images", "blog");

                entity.HasIndex(e => e.BlogPostId)
                    .HasName("blog_post_id_fk_idx");

                entity.Property(e => e.ImageId)
                    .HasColumnName("image_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BlogPostId)
                    .HasColumnName("blog_post_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasColumnName("extension")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ImageBlob)
                    .IsRequired()
                    .HasColumnName("image_blob")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.TimeChanged).HasColumnName("time_changed");

                entity.HasOne(d => d.BlogPost)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.BlogPostId)
                    .HasConstraintName("blog_post_id_fk");
            });

            modelBuilder.Entity<Logo>(entity =>
            {
                entity.ToTable("logos", "blog");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasColumnName("extension")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LogoBytes)
                    .IsRequired()
                    .HasColumnName("logoBytes")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeChanged).HasColumnName("time_changed");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts", "blog");

                entity.HasIndex(e => e.LogoId)
                    .HasName("logo_fk_idx");

                entity.HasIndex(e => e.Title)
                    .HasName("title_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Html)
                    .IsRequired()
                    .HasColumnName("html")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.ImagesChanged).HasColumnName("images_changed");

                entity.Property(e => e.LogoId)
                    .HasColumnName("logo_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Published)
                    .HasColumnName("published")
                    .HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Logo)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.LogoId)
                    .HasConstraintName("logo_id_fk");
            });
        }
    }
}
