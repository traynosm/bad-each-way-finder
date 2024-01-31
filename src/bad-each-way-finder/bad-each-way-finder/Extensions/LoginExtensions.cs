using Microsoft.AspNetCore.Identity;

namespace bad_each_way_finder.Extensions
{
    public static class LoginExtensions
    {
        public static async Task<IdentityUser> UpsertUser(this IdentityUser user, UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any(r => r == user))
            {
                await userManager.CreateAsync(user);
            }

            return user;
        }

        public static async Task<IdentityUser> AddUserToRole(this IdentityUser user, UserManager<IdentityUser> userManager, string role)
        {
            await userManager.AddToRoleAsync(user, role);

            return user;
        }

        public static async Task<IdentityUser> UpdateRoles(this IdentityUser user, UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                user = await user.AddUserToRole(userManager, role);
            }

            return user;
        }
    }
}
