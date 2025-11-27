using Domain.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(User.MaxNameLength);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(User.MaxUsernameLength);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(User.MaxEmailLength);

        builder.Property(u => u.AddressStreet).HasMaxLength(User.MaxStreetLength);
        builder.Property(u => u.AddressCity).HasMaxLength(User.MaxCityLength);

        builder.Property(u => u.Website).HasMaxLength(User.MaxWebsiteLength);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(User.MaxPasswordLength);
    }
}