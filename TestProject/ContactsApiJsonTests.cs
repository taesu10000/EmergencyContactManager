using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit.Abstractions;

namespace TestProject
{
    public class ContactsApiJsonTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper helper;
        public ContactsApiJsonTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _client = factory.CreateClient();
            this.helper = helper;
        }
 
        [Fact]
        public async Task Post_Json_Returns201()
        {
            var json = """
                [
                  {
                    "name": "김클로",
                    "email": "clo@clovf.com",
                    "tel": "010-1111-2424",
                    "joined": "2012-01-05"
                  },
                  {
                    "name": "박마블",
                    "email": "md@clovf.com",
                    "tel": "010-3535-7979",
                    "joined": "2013-07-01"
                  },
                  {
                    "name": "홍커넥",
                    "email": "connect@clovf.com",
                    "tel": "010-8531-7942",
                    "joined": "2019-12-05"
                  }
                ]
                """;

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task Post_JSON_File_Returns201()
        {
            var jsonPath = """..\..\..\random_JSON_1000.txt""";
            var fstream = File.OpenRead(jsonPath);
            using var content = new StreamContent(fstream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/json");

            var response = await _client.PostAsync("/api/employee", content);
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task Post_WithInvalidJson_Returns415()
        {
            var json = """
                { "name":"조유진"
                """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }
        [Fact]
        public async Task Post_WithInvalidCsv_Returns400()
        {
            var csv = """
                name,email
                조유진,a@test.com
                """;

            var content = new StringContent(csv, Encoding.UTF8, "text/plain");

            var response = await _client.PostAsync("/api/employee", content);

            Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
        }
    }
}