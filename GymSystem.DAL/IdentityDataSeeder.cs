using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedIdentityDataAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager , CancellationToken ct = default)
        {
            bool hasUsers = await userManager.Users.AnyAsync(ct);
            bool hasRoles = await roleManager.Roles.AnyAsync(ct);

            if (hasUsers || hasRoles) return;

            var roles = new List<IdentityRole>
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole ("Admin")
            };

            foreach (var role in roles)
            {
                var rolrResult = await roleManager.CreateAsync(role);
                if (!rolrResult.Succeeded)
                {
                    return;
                }
            }

            var User01 = new ApplicationUser()
            {
                Email = "mohamed@gmail.com",
                FirstName = "Mohamed",
                LastName = "Talat",
                PhoneNumber = "01027751025",
                UserName = "MohamedTalat"
            };

            await userManager.CreateAsync(User01 , "P@ssw0rd");
            await userManager.AddToRoleAsync(User01, "SuperAdmin");

            //User 02
            var User02 = new ApplicationUser()
            {
                Email = "Nour@gmail.com",
                FirstName = "Nour",
                LastName = "Ali",
                PhoneNumber = "01027751325",
                UserName = "nourali"
            };

            await userManager.CreateAsync(User02, "P@ssw0rd");
            await userManager.AddToRoleAsync(User02, "Admin");




        }
    }
}
