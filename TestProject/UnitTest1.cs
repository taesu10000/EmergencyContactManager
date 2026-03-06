using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit.Abstractions;

namespace TestProject
{
    public class ContactsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper helper;
        public ContactsApiTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _client = factory.CreateClient();
            this.helper = helper;
        }
 
        [Fact]
        public async Task Post_CSV_Returns201()
        {
            var scv = """
                김철수, charles@clovf.com, 01075312468, 2018.03.07
                박영희, matilda@clovf.com, 01087654321, 2021.04.28
                홍길동, kildong.hong@clovf.com, 01012345678, 2015.08.15
                """;

            using var content = new StringContent(scv, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task Post_SCV_File_Returns201()
        {
            var scvPath = """..\..\..\random_CSV_1000.txt""";
            var fstream = File.OpenRead(scvPath);
            using var content = new StreamContent(fstream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            var response = await _client.PostAsync("/api/employee", content);

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

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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
            helper.WriteLine(body);
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
            helper.WriteLine(body);
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
            helper.WriteLine(body);
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
        [Fact]
        public async Task RemoveAll()
        {
            var response = await _client.DeleteAsync("/api/employee/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
     
        }
        [Fact]
        public async Task Post_WithInvalidJson_Returns415()
        {
            var json = """
                { "name":"조유진"
                """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);

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
        [Fact]
        public async Task Post_WithEmptyBody_Returns400()
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/employee", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}