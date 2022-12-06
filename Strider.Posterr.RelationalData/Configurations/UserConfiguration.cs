using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Strider.Posterr.Common;
using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.RelationalData.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
            .HasKey(a => a.Id);
        
        builder.Property(s => s.UserName)
            .HasMaxLength(14)
            .IsUnicode(false)
            .IsRequired();
        
        builder.HasIndex(a => a.UserName)
            .IsUnique();

        builder.HasData(UserFactory.UserOne(), UserFactory.UserTwo());
    }
}