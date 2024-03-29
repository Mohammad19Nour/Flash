using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;

namespace ProjectP.Data.Configuration;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasMany(c => c.Photos);
        builder.HasOne(h => h.Location).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne<Offer>(o => o.Offer)
            .WithOne()
            .HasForeignKey<Offer>(y => y.HotelId)
            .IsRequired();

        builder.HasMany(h => h.HotelRoomTypes)
            .WithOne(t => t.Hotel)
            .HasForeignKey(t => t.HotelId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}