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
        UserManager<AppUser> userManager, IMapper mapper)
    {
        await SeedRoomTypes(context);
        await SeedRoles(roleManager);
        await SeedCategories(context);
        await SeedHotels(context);
        await SeedOffers(context);
        await SeedUsers(userManager);
        await SeedPlaces(context);
    }

    private static async Task SeedRoomTypes(DataContext context)
    {
        List<string> englishRoomTypes = new List<string>
        {
            "Single Room",
            "Double Room",
            "Twin Room",
            "Suite",
            "Deluxe Room",
            "Executive Suite",
            "Standard Room",
            "Family Room",
            "Connecting Room",
            "Junior Suite",
            "Presidential Suite",
            "Villa",
            "Bungalow",
            "Duplex Room",
            "Studio Room"
        };

        List<string> arabicRoomTypes = new List<string>
        {
            "غرفة فردية",
            "غرفة مزدوجة",
            "غرفة توأم",
            "جناح",
            "غرفة ديلوكس",
            "جناح تنفيذي",
            "غرفة قياسية",
            "غرفة عائلية",
            "غرفة متصلة",
            "جناح صغير",
            "جناح رئاسي",
            "فيلا",
            "بنغلو",
            "غرفة دوبلكس",
            "غرفة استوديو"
        };


        if (await context.RoomTypes.AnyAsync()) return;

        int cnt = 0;
        foreach (var type in englishRoomTypes)
        {
            context.RoomTypes.Add(new RoomType
            {
                ArabicName = arabicRoomTypes[cnt],
                EnglishName = type
            });
            cnt++;
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedPlaces(DataContext context)
    {
        if (await context.TouristPlaces.AnyAsync()) return;
        var count = 5;
        var places = new List<TouristPlace>();

        for (int i = 1; i <= count; i++)
        {
            var hotel = new TouristPlace
            {
                ArabicName = $"Arabic Place {i}",
                EnglishName = $"English Place {i}",

                ArabicDescription = $"Arabic description for Place {i}",
                EnglishDescription = $"English description for Place {i}",

                Location = new Location
                {
                    City = $"City {i}",
                    StreetName = $"Street Name {i}",
                    Longitude = 1.0 * i,
                    Latitude = 1.0 * i
                },

                Photos = Enumerable.Range(1, 2).Select(j => new Photo
                {
                    PictureUrl = $"images/{(j) % 2 + 1}.jpg"
                }).ToList()
            };

            places.Add(hotel);
        }

        context.TouristPlaces.AddRange(places);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        if (await userManager.Users.AnyAsync())
            return;
        var user = new AppUser
        {
            Email = "u@u.u",
            UserName = "u@u.u",
            FirstName = "alic",
            LastName = "bob",
            PictureUrl = "images/1.jpg",
            CountryPrifix = "+971",
            EmailConfirmed = true,
            Gender = Gender.Male,
            PhoneNumber = "971356205634"
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
        user = new AppUser
        {
            Email = "uu@u.u",
            UserName = "u@uu.u",
            FirstName = "bob",
            LastName = "alic",
            PictureUrl = "images/2.jpg",
            CountryPrifix = "+971",
            EmailConfirmed = true,
            Gender = Gender.Female,
            PhoneNumber = "97139205634"
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
    }

    private static async Task SeedOffers(DataContext context)
    {
        if (await context.Offers.AnyAsync()) return;
        var hotels = await context.Hotels.Include(v => v.Offer).ToListAsync();

        int cnt = 1;
        foreach (var hotel in hotels)
        {
            if (hotel.Offer != null) continue;
            hotel.Offer = new Offer
            {
                StartAt = DateTime.Now.AddHours(-3 * cnt),
                EndAt = DateTime.Now.AddDays(cnt),
                Discount = cnt * cnt,
                HotelId = hotel.Id
            };
            cnt++;
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedHotels(DataContext context)
    {
        if (await context.Hotels.AnyAsync()) return;
        var roomTypes = await context.RoomTypes.ToListAsync();
        int sz = roomTypes.Count;
        var count = 5;
        var hotels = new List<Hotel>();

        for (int i = 1; i <= count; i++)
        {
            var hotel = new Hotel
            {
                HotelRoomTypes = new List<HotelRoomType>
                {
                    new HotelRoomType
                    {
                        RoomType = roomTypes[(i*i+1+i) % sz]
                    },
                    
                    new HotelRoomType
                    {
                        RoomType = roomTypes[(i*i+2+i) % sz]
                    },
                },
                ArabicName = $"Arabic Hotel {i}",
                EnglishName = $"English Hotel {i}",
                Email = $"hotel{i}@example.com",
                Stars = i % 5 + 1,
                Reviews = i * 10,
                ArabicDescription = $"Arabic description for Hotel {i}",
                EnglishDescription = $"English description for Hotel {i}",
                MinPrice = i * 100,
                MaxPrice = (i + 1) * 100,
                Location = new Location
                {
                    City = $"City {i}",
                    StreetName = $"Street Name {i}",
                    Longitude = 1.0 * i,
                    Latitude = 1.0 * i
                },
                OfferId = null,
                Photos = Enumerable.Range(1, 2).Select(j => new Photo
                {
                    PictureUrl = $"images/{(j) % 2 + 1}.jpg"
                }).ToList()
            };

            hotels.Add(hotel);
        }

        context.Hotels.AddRange(hotels);
        await context.SaveChangesAsync();
    }


    private static async Task SeedCategories(DataContext context)
    {
        if (await context.Categories.AnyAsync()) return;
        context.Categories.Add(new Category { ArabicName = "الكل", EnglishName = "All", PictureUrl = "No photo" });
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