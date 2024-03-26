using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;

namespace ProjectP.Data.Configuration;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasMany(c => c.Photos).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(h => h.Location).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}