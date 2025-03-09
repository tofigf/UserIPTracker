using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace UserIPTracker.Tests
{
    public class UserConnectionsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserConnectionsApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ConnectUser_ShouldReturnOk()
        {
            var request = new { UserId = 1, IpAddress = "127.0.0.1" };

            var response = await _client.PostAsJsonAsync("/api/user-connections/connect", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchUsers_ShouldReturnUserIds()
        {
            string ip = "31.214";
            var response = await _client.GetAsync($"/api/user-connections/search?ip={ip}");

            var responseBody = await response.Content.ReadAsStringAsync();

            var users = await response.Content.ReadFromJsonAsync<List<long>>();

            Assert.NotNull(users);
            Assert.Contains(1, users);
        }

    }
}
