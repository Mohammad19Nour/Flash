using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Configuration;
using ProjectP.Data.Entities;
using ProjectP.Data.Entities.Identity;

namespace ProjectP.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DataContext()
    {
    }

    public DbSet<AppUser> AppUsers;
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<UserHotel> Favorites { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppRole>().HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

       
        builder.Entity<Photo>()
            .HasOne(p => p.Hotel) // Assuming Hotel is the navigation property in Photo entity
            .WithMany(h => h.Photos) // Assuming Photos is the navigation property in Hotel entity
            .HasForeignKey(p => p.HotelId).OnDelete(DeleteBehavior.Cascade);
      
            
        builder.ApplyConfiguration(new UserHotelConfiguration());
        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new HotelConfiguration());
        builder.ApplyConfiguration(new OfferConfiguration());
        
        
        var sqlite = Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(double));

            foreach (var property in properties)
            {
                if (sqlite)
                    builder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                else
                    builder.Entity(entityType.Name).Property(property.Name)
                        .HasPrecision(38, 3);
            }
        }
    }
}