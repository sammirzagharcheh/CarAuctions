using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> _userManager)
    {
        this._userManager = _userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        if (user != null)
        {
            var existingClams = await _userManager.GetClaimsAsync(user);


            if (user.UserName != null)
            {
                // Adding new custom claim
                var claims = new List<Claim>()
                {
                    new Claim("username", user.UserName),
                };

                context.IssuedClaims.AddRange(claims);

                // Adding Full name of User in the existing Claim
                context.IssuedClaims.Add(existingClams.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
            }
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}