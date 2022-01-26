using Library.Entities.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Data
{
    public class SeededRoles
    {
        static readonly Role AdminRole = new Role { Name = "Admin" };
        static readonly Role AuthorRole = new Role { Name = "Author" };
        static readonly Role UserRole = new Role { Name = "User" };

        static RoleManager<Role> _roleManager;

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<LibraryContext>();
            if (context.Database.IsRelational())
                if ((await context.Database.GetPendingMigrationsAsync()).Any())          
                    await context.Database.MigrateAsync();                
            

            _roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<Role>>();
                        
            await CreateRole(AdminRole);
            await CreateRole(AuthorRole);
            await CreateRole(UserRole);
                        
            static async Task CreateRole(Role role)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }

            }
        }
    }
}
