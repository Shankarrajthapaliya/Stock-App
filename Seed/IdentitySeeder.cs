using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace web.Seed
{
    public class IdentitySeeder
    {
        public static  async  Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            var seedOptions = services.GetRequiredService<IOptions<SeedOptions>>().Value;

            var roles = new [] { "Admin", "Employee","User" };

            foreach (var roleName in roles)
            {
                var exists = await roleManager.RoleExistsAsync(roleName);
                if (!exists)
                {
                    
                    var createRole = await roleManager.CreateAsync(new IdentityRole(roleName));

                    if (!createRole.Succeeded)
                    {
                        return;
                    }
                }
            }
          await EnsureUserWithRoleAsync(userManager,
                email: seedOptions.AdminEmail,
                password: seedOptions.AdminPassword,
                role: "Admin");

            await EnsureUserWithRoleAsync(userManager,
                email: seedOptions.EmployeeEmail,
                password: seedOptions.EmployeePassword,
                role: "Employee");

            await EnsureUserWithRoleAsync(userManager,
                email: seedOptions.UserEmail,
                password: seedOptions.UserPassword,
                role: "User");

                  await EnsureUserWithRoleAsync(userManager,
                email: seedOptions.AdminOwnerEmail,
                password: seedOptions.AdminOwnerPassword,
                role: "Admin");
            
        }

        public static async Task  EnsureUserWithRoleAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null )
            {
                 user = new IdentityUser
                {
                  UserName = email,
                  Email = email,
                  EmailConfirmed = true

                  
                   
                };


              var result =  await userManager.CreateAsync(user, password);
              if (!result.Succeeded)
                {
                    return;
                }
            }
    

  
  var isInRole = await userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                await userManager.AddToRoleAsync(user, role);
            }
 

     if (string.Equals(user.Email, "adminOwner@local.test", StringComparison.OrdinalIgnoreCase)&& role.Equals("Admin"))
            {
              var  claims  =   new Claim("permission", "delete:stock");
                var existingClaims = await userManager.GetClaimsAsync(user);
               
                if(!existingClaims.Any(c => c.Value == claims.Value && c.Type == claims.Type )){
                      await userManager.AddClaimAsync(user, claims); 
                }
            }

        }
    }
}