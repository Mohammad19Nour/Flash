using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;

namespace ProjectP.Data.Configuration;

public class UserHotelConfiguration : IEntityTypeConfiguration<UserHotel>
{
    public void Configure(EntityTypeBuilder<UserHotel> builder)
    {
        builder.HasKey(uh => new { uh.UserId,uh.HotelId });
    }
}