using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Extensions
{
    public static class UserRoleDataSeedExtension
    {
        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            AppRole appRole = new()
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString() //Bu ifade sisteminizin yeni bir Guid yaratmasını saglar
            };

            modelBuilder.Entity<AppRole>().HasData(appRole);

            PasswordHasher<AppUser> passwordHasher = new();

            AppUser user = new()
            {
                Id = 1,
                UserName = "ahmet123",
                NormalizedUserName = "AHMET123",
                Email = "ahmet@gmail.com",
                NormalizedEmail = "AHMET@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = passwordHasher.HashPassword(null, "ahmet123")
            };

            modelBuilder.Entity<AppUser>().HasData(user);

            modelBuilder.Entity<AppUserRole>().HasData(new AppUserRole
            {
                RoleId = appRole.Id,
                UserId = user.Id
            });

        }

    }
}

