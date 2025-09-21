
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class MisContext : DbContext
{
    public MisContext()
    {
    }

    public MisContext(DbContextOptions<MisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Meme> Memes { get; set; }

    public virtual DbSet<MemeMetadatum> MemeMetadata { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UploadStat> UploadStats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__collecti__53D3A5CAA30B7500");

            entity.ToTable("collections");

            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.CollectionName)
                .HasMaxLength(100)
                .HasColumnName("collection_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Collections)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__collectio__user___3FD07829");

            entity.HasMany(d => d.Memes).WithMany(p => p.Collections)
                .UsingEntity<Dictionary<string, object>>(
                    "CollectionMeme",
                    r => r.HasOne<Meme>().WithMany()
                        .HasForeignKey("MemeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__collectio__meme___43A1090D"),
                    l => l.HasOne<Collection>().WithMany()
                        .HasForeignKey("CollectionId")
                        .HasConstraintName("FK__collectio__colle__42ACE4D4"),
                    j =>
                    {
                        j.HasKey("CollectionId", "MemeId").HasName("PK__collecti__B67E80B26976637C");
                        j.ToTable("collection_memes");
                        j.IndexerProperty<int>("CollectionId").HasColumnName("collection_id");
                        j.IndexerProperty<int>("MemeId").HasColumnName("meme_id");
                    });
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comments__E7957687FA3CF74E");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("comment_date");
            entity.Property(e => e.CommentText)
                .HasMaxLength(1000)
                .HasColumnName("comment_text");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Meme).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MemeId)
                .HasConstraintName("FK__comments__meme_i__3B0BC30C");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comments__user_i__3BFFE745");
        });

        modelBuilder.Entity<Meme>(entity =>
        {
            entity.HasKey(e => e.MemeId).HasName("PK__memes__5AD2578F87F22068");

            entity.ToTable("memes");

            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("upload_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Memes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__memes__user_id__2610A626");

            entity.HasMany(d => d.Tags).WithMany(p => p.Memes)
                .UsingEntity<Dictionary<string, object>>(
                    "MemeTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__meme_tags__tag_i__32767D0B"),
                    l => l.HasOne<Meme>().WithMany()
                        .HasForeignKey("MemeId")
                        .HasConstraintName("FK__meme_tags__meme___318258D2"),
                    j =>
                    {
                        j.HasKey("MemeId", "TagId").HasName("PK__meme_tag__2EFB3DA44C7753AF");
                        j.ToTable("meme_tags");
                        j.IndexerProperty<int>("MemeId").HasColumnName("meme_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<MemeMetadatum>(entity =>
        {
            entity.HasKey(e => e.MemeId).HasName("PK__meme_met__5AD2578F8F16AD1D");

            entity.ToTable("meme_metadata");

            entity.Property(e => e.MemeId)
                .ValueGeneratedNever()
                .HasColumnName("meme_id");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_updated");
            entity.Property(e => e.Shares)
                .HasDefaultValue(0)
                .HasColumnName("shares");
            entity.Property(e => e.Views)
                .HasDefaultValue(0)
                .HasColumnName("views");

            entity.HasOne(d => d.Meme).WithOne(p => p.MemeMetadatum)
                .HasForeignKey<MemeMetadatum>(d => d.MemeId)
                .HasConstraintName("FK__meme_meta__meme___2BC97F7C");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.ReactionId).HasName("PK__reaction__36A9D29813A75BC4");

            entity.ToTable("reactions");

            entity.Property(e => e.ReactionId).HasColumnName("reaction_id");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.ReactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reaction_date");
            entity.Property(e => e.ReactionType)
                .HasMaxLength(20)
                .HasColumnName("reaction_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Meme).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.MemeId)
                .HasConstraintName("FK__reactions__meme___36470DEF");

            entity.HasOne(d => d.User).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reactions__user___373B3228");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC0823FF07");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B1EC8A87C0").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__tags__4296A2B6691AE076");

            entity.ToTable("tags");

            entity.HasIndex(e => e.TagName, "UQ__tags__E298655CBAEA6588").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TagName)
                .HasMaxLength(50)
                .HasColumnName("tag_name");
        });

        modelBuilder.Entity<UploadStat>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("PK__upload_s__B8A52560473B37ED");

            entity.ToTable("upload_stats");

            entity.Property(e => e.StatId).HasColumnName("stat_id");
            entity.Property(e => e.LastUploadDate)
                .HasColumnType("datetime")
                .HasColumnName("last_upload_date");
            entity.Property(e => e.TotalViews)
                .HasDefaultValue(0)
                .HasColumnName("total_views");
            entity.Property(e => e.UploadCount)
                .HasDefaultValue(0)
                .HasColumnName("upload_count");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UploadStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__upload_st__user___4865BE2A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FF16AC13E");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61643B91F4FA").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572F86B8864").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__user_role__role___22401542"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__user_role__user___214BF109"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__user_rol__6EDEA153CC4A6AC6");
                        j.ToTable("user_roles");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
