using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectP.Data.Entities;

namespace ProjectP.Data.Configuration;

public class OfferConfiguration :IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder
            .HasOne<Hotel>(o => o.Hotel)
            .WithOne(h => h.Offer)
            .HasForeignKey<Hotel>(y => y.OfferId)
            
            .OnDelete(DeleteBehavior.SetNull);
    }
}