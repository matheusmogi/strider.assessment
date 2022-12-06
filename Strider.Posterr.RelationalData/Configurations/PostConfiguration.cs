using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.RelationalData.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts")
            .HasKey(a => a.Id);
        builder.Property(s => s.Content)
            .HasMaxLength(777);
        builder.HasOne(a=>a.OriginalPost)
            .WithMany(a=>a.Reposted)
            .IsRequired(false);
        builder.HasOne(a => a.CreatedBy)
            .WithMany(a => a.PostsCreated)
            .HasForeignKey(a => a.CreatedById);
        builder.HasMany(a => a.UsersMentioned)
            .WithMany(a => a.PostsMentioned)
            .UsingEntity<Dictionary<string, object>>(
                "Mentions",
                j => j.HasOne<User>().WithMany().OnDelete(DeleteBehavior.ClientCascade),
                j => j.HasOne<Post>().WithMany().OnDelete(DeleteBehavior.ClientCascade));
    }
}