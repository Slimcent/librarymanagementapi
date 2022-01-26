using Library.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json;
using Library.Entities.Entities.DTO;
using Library.Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Integration.Test.Controllers
{    
    public class AdminControllerIntegrationTests : IClassFixture<LibraryWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AdminControllerIntegrationTests(LibraryWebApplicationFactory<Program> factory)
            => _client = factory.CreateClient();
         
        //check for validation of model
        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnsOkResult()
        {
            var response = await _client.GetAsync("api/Admin/GetAllUsers");
            response.EnsureSuccessStatusCode();

        }
                
        [Theory]
        [InlineData("1", "Admin1")]        
        public async Task GetUserById_GetsForAValidUser_ReturnsOkResult(string id, string userFirstName)
        {
            var response = await _client.GetAsync("api/Admin/GetUserById/"+id);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(stringResponse);
            Assert.True(user.FirstName == userFirstName);
        }
        
        [Fact]
        public async Task GetUserById_GetsInvalidUser_ReturnsNotContentObjectResult()
        {
            var response = await _client.GetAsync("api/Admin/GetUserById/-1");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);            
        }

        [Fact]
        public async Task RegisterUser_UserAlreadyExists_ReturnsBadRequestResult()
        {
            var content = new RegisterModel() { Email = "admin1@library.com", FirstName = "Admin1", LastName = "SeededAdmin", UserName = "Admin1", Password= "@Secret" };
            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync("api/Admin/RegisterUser", stringContent);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUser_CreatedSuccessfully_ReturnsOkRequestObject()
        {
            var content = new RegisterModel() { Email = "Kach@library.com", FirstName = "Try", LastName = "Kach", UserName = "try_kach", Password = "@Secret" };
            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Admin/RegisterUser", stringContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
       
        [Fact]
        public async Task DeleteUserById_NoUserFound_ReturnsNotContentRequestObject()
        {
            var response = await _client.DeleteAsync("api/Admin/DeleteUserById/0");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUserById_ReturnsOkRequestObject()
        {
            var response = await _client.DeleteAsync("api/Admin/DeleteUserById/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }               
    }
}