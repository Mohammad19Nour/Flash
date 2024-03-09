using AsparagusN.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ProjectP.Data.Entities.Identity;
using ProjectP.Entities;

namespace ProjectP.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager,IMapper mapper)
    {
        await SeedRoles(roleManager);
    }
    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        // await roleManager.CreateAsync(new() { Name = Roles.Employee.GetDisplayName() });
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