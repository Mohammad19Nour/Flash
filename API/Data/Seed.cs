using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ProjectP.Data.Entities;
using ProjectP.Data.Entities.Identity;
using ProjectP.Enums;
using ProjectP.Interfaces;

namespace ProjectP.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager,IMapper mapper)
    {
        await SeedRoles(roleManager);
        await SeedCategories(context);
        await SeedHotels(context);
        await SeedOffers(context);
    }

    private static async Task SeedOffers(DataContext context)
    {
        if (await context.Offers.AnyAsync())return;
        var hotels = await context.Hotels.Include(v => v.Offer).ToListAsync();

        int cnt = 1;
        foreach (var hotel in hotels)
        {
            if (hotel.Offer != null) continue;
            hotel.Offer = new Offer
            {
                StartAt = DateTime.Now.AddHours(-3*cnt),
                EndAt = DateTime.Now.AddDays(cnt),
                Discount = cnt * cnt 
            };
            cnt++;
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedHotels(DataContext context)
    {
        if (await context.Hotels.AnyAsync()) return;
        var count = 5;
        var hotels = new List<Hotel>();

        for (int i = 1; i <= count; i++)
        {
            var hotel = new Hotel
            {
                ArabicName = $"Arabic Hotel {i}",
                EnglishName = $"English Hotel {i}",
                Email = $"hotel{i}@example.com",
                Stars = i % 5 + 1, 
                Reviews = i * 10,
                ArabicDescription = $"Arabic description for Hotel {i}",
                EnglishDescription = $"English description for Hotel {i}",
                MinPrice = i * 100,
                MaxPrice = (i + 1) * 100,
                Location =  new Location
                {
                    City = $"City {i}",
                    StreetName = $"Street Name {i}",
                    Longitude = 1.0 * i,
                    Latitude = 1.0 * i 
                },
                OfferId = null,
                Photos = Enumerable.Range(1, 2).Select(j => new Photo
                {
                    PictureUrl = $"images/{(j)%2+1}.jpg"
                }).ToList()
            };

            hotels.Add(hotel);
        }
        context.Hotels.AddRange(hotels);
        await context.SaveChangesAsync();
    }


    private static async Task SeedCategories(DataContext context)
    {
        if ( await context.Categories.AnyAsync())return;
        context.Categories.Add(new Category { ArabicName = "الكل",EnglishName = "All",PictureUrl = "No photo"});
        await context.SaveChangesAsync();
    }

    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new() { Name = Roles.User.GetDisplayName() },
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }
}