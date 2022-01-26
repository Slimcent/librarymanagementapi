using Library.API;
using Library.API.Data;
using Library.Entities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Integration.Test
{
    public class LibraryWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        //to configure services
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {            
            builder.ConfigureServices(services =>
            {
                //get SUT context
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<LibraryContext>));
                //remove SUT context
                services.Remove(descriptor);
                //create database
                                
                services.AddDbContext<LibraryContext>( options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<LibraryContext>();
                    //create in memory database
                    db.Database.EnsureCreated();                                   
                }
            });
        }
    }    
}
