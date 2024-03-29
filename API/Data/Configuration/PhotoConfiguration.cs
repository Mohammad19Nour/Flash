using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;

namespace ProjectP.Data.Configuration;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder
            .HasOne(p => p.Hotel)
            .WithMany(h => h.Photos)
            .HasForeignKey(p => p.HotelId);
        builder
            .HasOne(p => p.TouristPlace) 
            .WithMany(h => h.Photos) 
            .HasForeignKey(p => p.TouristPlaceId);
    }
}