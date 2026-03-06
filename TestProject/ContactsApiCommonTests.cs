using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit.Abstractions;

namespace TestProject
{
    public class ContactsApiCommonTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper helper;
        public ContactsApiCommonTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _client = factory.CreateClient();
            this.helper = helper;
        }
 
        [Fact]
        public async Task Get_AllContacts_Returns200()
        {
            // Act
            int? page = null;
            int? pageSize = null;
            var response = await _client.GetAsync($"/api/employee?page={page}&pageSize={pageSize}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
        [Fact]
        public async Task Get_ByName_Returns200()
        {
            // Act
            var name = "김은지";
            var response = await _client.GetAsync($"/api/employee/{name}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
        [Fact]
        public async Task Search_Query_Returns200()
        {
            // Act
            var q = "김은";
            var name = "";
            var email = "";
            var tel = "3359";
            DateTimeOffset? joined = null;
            int? page = 1;
            int? pageSize = 30;
            var response = await _client.GetAsync($"/api/employee/search?q={q}&tel={tel}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
        [Fact]
        public async Task Post_WithEmptyBody_Returns400()
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveAll()
        {
            var response = await _client.DeleteAsync("/api/employee/all");

            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}