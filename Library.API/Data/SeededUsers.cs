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
    public static class SeededUsers
    {
        static readonly User Admin1 = new User { Id= "1", CreatedAt = DateTime.Now, Email = "admin1@library.com", FirstName = "Admin1", LastName = "SeededAdmin", UserName = "Admin1" };
        static readonly User Admin2 = new User { Id = "2", CreatedAt = DateTime.Now, Email = "admin2@library.com", FirstName = "Admin2", LastName = "SeededAdmin", UserName = "Admin2" };
        static readonly User Author1 = new User { Id = "3", CreatedAt = DateTime.Now, Email = "author1@library.com", FirstName = "author1", LastName = "SeededAuthor", UserName = "Author1" };
        static readonly User User1 = new User { Id = "5", CreatedAt = DateTime.Now, Email = "user1@library.com", FirstName = "user1", LastName = "SeededUser", UserName = "User1" };
        static readonly User User2 = new User { Id = "6", CreatedAt = DateTime.Now, Email = "user2@library.com", FirstName = "user2", LastName = "SeededUser", UserName = "User2" };
        static readonly User User3 = new User { Id = "7", CreatedAt = DateTime.Now, Email = "user3@library.com", FirstName = "user3", LastName = "SeededUser", UserName = "User3" };
        
        static UserManager<User> _userManager;

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<LibraryContext>();
            if (context.Database.IsRelational())
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    await context.Database.MigrateAsync();

            _userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<User>>();
            
                        
            await CreateUser(Admin1, "Admin", "Secret@");
            await CreateUser(Admin2, "Admin", "Secret@");
            await CreateUser(Author1, "Author", "Secret@");
            await CreateUser(User1, "User", "Secret@");
            await CreateUser(User2, "User", "Secret@");
            await CreateUser(User3, "User", "Secret@");
           
            static async Task CreateUser(User defaultUser, string role, string password)
            {
                var user = await _userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                    user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user != null) return;

                var createUser = await _userManager.CreateAsync(defaultUser, password);
                if (createUser.Succeeded) await _userManager.AddToRoleAsync(defaultUser, role);
            }             
            
        } 
    }                   
           
              
}
