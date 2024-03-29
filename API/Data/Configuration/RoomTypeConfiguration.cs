using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;

namespace ProjectP.Data.Configuration;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.HasMany(r => r.HotelRoomTypes)
            .WithOne(t => t.RoomType)
            .HasForeignKey(t => t.RoomTypeId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}