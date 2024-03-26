using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ProjectP.Data.Entities;
using ProjectP.Data.Entities.Identity;
using ProjectP.Enums;

namespace ProjectP.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager,IMapper mapper)
    {
        await SeedRoles(roleManager);
        await SeedCategories(context);
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