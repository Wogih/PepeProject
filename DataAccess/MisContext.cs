using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

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

    public virtual DbSet<CollectionMeme> CollectionMemes { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Meme> Memes { get; set; }

    public virtual DbSet<MemeMetadatum> MemeMetadata { get; set; }

    public virtual DbSet<MemeTag> MemeTags { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UploadStat> UploadStats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-K6LFJKO;Database=MIS;User Id=sa;Password=1;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__collecti__53D3A5CAB5272981");

            entity.ToTable("collections");

            entity.HasIndex(e => new { e.UserId, e.CollectionName }, "UC_UserCollectionName").IsUnique();

            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.CollectionName)
                .HasMaxLength(100)
                .HasColumnName("collection_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsPublic)
                .HasDefaultValue(false)
                .HasColumnName("is_public");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Collections)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__collectio__user___693CA210");
        });

        modelBuilder.Entity<CollectionMeme>(entity =>
        {
            entity.HasKey(e => e.CollectionMemeId).HasName("PK__collecti__019B66EEB1148A19");

            entity.ToTable("collection_memes");

            entity.HasIndex(e => new { e.CollectionId, e.MemeId }, "UC_CollectionMeme").IsUnique();

            entity.Property(e => e.CollectionMemeId).HasColumnName("collection_meme_id");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("added_at");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");

            entity.HasOne(d => d.Collection).WithMany(p => p.CollectionMemes)
                .HasForeignKey(d => d.CollectionId)
                .HasConstraintName("FK__collectio__colle__6E01572D");

            entity.HasOne(d => d.Meme).WithMany(p => p.CollectionMemes)
                .HasForeignKey(d => d.MemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__collectio__meme___6EF57B66");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comments__E79576870956273D");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("comment_date");
            entity.Property(e => e.CommentText)
                .HasMaxLength(1000)
                .HasColumnName("comment_text");
            entity.Property(e => e.EditedAt)
                .HasColumnType("datetime")
                .HasColumnName("edited_at");
            entity.Property(e => e.IsEdited)
                .HasDefaultValue(false)
                .HasColumnName("is_edited");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Meme).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MemeId)
                .HasConstraintName("FK__comments__meme_i__60A75C0F");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__comments__parent__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comments__user_i__619B8048");
        });

        modelBuilder.Entity<Meme>(entity =>
        {
            entity.HasKey(e => e.MemeId).HasName("PK__memes__5AD2578F5B5FFD53");

            entity.ToTable("memes");

            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsPublic)
                .HasDefaultValue(true)
                .HasColumnName("is_public");
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
                .HasConstraintName("FK__memes__user_id__46E78A0C");
        });

        modelBuilder.Entity<MemeMetadatum>(entity =>
        {
            entity.HasKey(e => e.MetadataId).HasName("PK__meme_met__C1088FC4BB567167");

            entity.ToTable("meme_metadata");

            entity.HasIndex(e => e.MemeId, "UQ__meme_met__5AD2578E3117FEA1").IsUnique();

            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FileFormat)
                .HasMaxLength(10)
                .HasColumnName("file_format");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.MimeType)
                .HasMaxLength(50)
                .HasColumnName("mime_type");
            entity.Property(e => e.Width).HasColumnName("width");

            entity.HasOne(d => d.Meme).WithOne(p => p.MemeMetadatum)
                .HasForeignKey<MemeMetadatum>(d => d.MemeId)
                .HasConstraintName("FK__meme_meta__meme___4BAC3F29");
        });

        modelBuilder.Entity<MemeTag>(entity =>
        {
            entity.HasKey(e => e.MemeTagId).HasName("PK__meme_tag__16773B431689E4B4");

            entity.ToTable("meme_tags");

            entity.HasIndex(e => new { e.MemeId, e.TagId }, "UC_MemeTag").IsUnique();

            entity.Property(e => e.MemeTagId).HasColumnName("meme_tag_id");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TaggedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("tagged_at");

            entity.HasOne(d => d.Meme).WithMany(p => p.MemeTags)
                .HasForeignKey(d => d.MemeId)
                .HasConstraintName("FK__meme_tags__meme___5441852A");

            entity.HasOne(d => d.Tag).WithMany(p => p.MemeTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__meme_tags__tag_i__5535A963");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.ReactionId).HasName("PK__reaction__36A9D29816879338");

            entity.ToTable("reactions");

            entity.HasIndex(e => new { e.UserId, e.MemeId, e.ReactionType }, "UC_UserMemeReaction").IsUnique();

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
                .HasConstraintName("FK__reactions__meme___5AEE82B9");

            entity.HasOne(d => d.User).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reactions__user___5BE2A6F2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CCD6CF1D64");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B187997332").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__tags__4296A2B6C2048044");

            entity.ToTable("tags");

            entity.HasIndex(e => e.TagName, "UQ__tags__E298655CA7AA5A3E").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.TagName)
                .HasMaxLength(50)
                .HasColumnName("tag_name");
        });

        modelBuilder.Entity<UploadStat>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("PK__upload_s__B8A52560A415E415");

            entity.ToTable("upload_stats");

            entity.HasIndex(e => e.MemeId, "UQ__upload_s__5AD2578EADFA737A").IsUnique();

            entity.Property(e => e.StatId).HasColumnName("stat_id");
            entity.Property(e => e.DownloadCount)
                .HasDefaultValue(0)
                .HasColumnName("download_count");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_updated");
            entity.Property(e => e.LastViewed)
                .HasColumnType("datetime")
                .HasColumnName("last_viewed");
            entity.Property(e => e.MemeId).HasColumnName("meme_id");
            entity.Property(e => e.ShareCount)
                .HasDefaultValue(0)
                .HasColumnName("share_count");
            entity.Property(e => e.ViewsCount)
                .HasDefaultValue(0)
                .HasColumnName("views_count");

            entity.HasOne(d => d.Meme).WithOne(p => p.UploadStat)
                .HasForeignKey<UploadStat>(d => d.MemeId)
                .HasConstraintName("FK__upload_st__meme___76969D2E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F7F35AA08");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616448A45D4C").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572F302E448").IsUnique();

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
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__user_rol__B8D9ABA2CDA4630A");

            entity.ToTable("user_roles");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UC_UserRole").IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assigned_at");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_role__role___4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__user_role__user___412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}