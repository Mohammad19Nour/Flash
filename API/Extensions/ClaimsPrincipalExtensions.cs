using System.Security.Claims;

namespace ProjectP.Extensions;
public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.Claims.First(x => x.Type == ClaimTypes.Email).Value.ToLower();
    }
    public static string? GetRole(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value.ToLower();
    }
}