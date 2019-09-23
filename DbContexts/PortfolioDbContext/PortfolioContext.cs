using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyPortfolioWebApp.DbContexts.PortfolioDbContext
{
    public partial class PortfolioContext : DbContext    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options): base(options) { }       

        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectsHtml> ProjectsHtml { get; set; }
        public virtual DbSet<ProjectsImages> ProjectsImages { get; set; }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.ToTable("projects", "portfolio");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Imageschanged).HasColumnName("imageschanged");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Publishingdate)
                    .HasColumnName("publishingdate")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<ProjectsHtml>(entity =>
            {
                entity.ToTable("projects_html", "portfolio");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Html)
                    .IsRequired()
                    .HasColumnName("html")
                    .HasColumnType("mediumtext");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ProjectsHtml)
                    .HasForeignKey<ProjectsHtml>(d => d.Id)
                    .HasConstraintName("projects_id");
            });

            modelBuilder.Entity<ProjectsImages>(entity =>
            {
                entity.HasKey(e => new { e.Projectid, e.Imageid });

                entity.ToTable("projects_images", "portfolio");

                entity.Property(e => e.Projectid)
                    .HasColumnName("projectid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Imageid)
                    .HasColumnName("imageid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasColumnName("extension")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectsImages)
                    .HasForeignKey(d => d.Projectid)
                    .HasConstraintName("projectidfk");
            });
        }
    }
}
