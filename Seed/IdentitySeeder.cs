using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
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
                email: seedOptions.WorkerEmail,
                password: seedOptions.WorkerPassword,
                role: "Worker");

            await EnsureUserWithRoleAsync(userManager,
                email: seedOptions.UserEmail,
                password: seedOptions.UserPassword,
                role: "User");
            
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


        }
    }
}