using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u=> u.Id);
        builder.Property(u=> u.UserName).IsRequired().HasMaxLength(100);
        builder.Property(u=> u.UserPasswordHash).IsRequired();
        builder.Property(u=> u.Role).IsRequired();
    }
}
