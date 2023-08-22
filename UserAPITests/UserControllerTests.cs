using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using UserAPI.Models;

namespace UserAPITests
{
    [TestClass]
    public class UserControllerTests
    {
        private HttpClient _httpClient;

        public UserControllerTests()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _httpClient = appFactory.CreateDefaultClient();
        }

        [TestMethod]
        public async Task User_id_set_on_create()
        {           
            var payload = new User
            {
                Id = Guid.Empty,
                FirstName = RandomString(5),
                LastName = RandomString(10),
                Address = "USA",
                DateOfBirth = "1965-05-25"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/user")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.IsFalse(result.Id == Guid.Empty);
        }

        [TestMethod]
        public async Task User_create_checks_null_fields()
        {
            var payload = $"{{\"Id\":\"{Guid.Empty}\",\"FirstName\":\"\",\"LastName\":\"\",\"Address\":\"\",\"DateOfBirth\":\"\"}}";

            var request = new HttpRequestMessage(HttpMethod.Post, "/user")
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        static string RandomString(int length) => Path.GetRandomFileName().Replace(".", null).Substring(0, length);
    }
}