using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit.Abstractions;

namespace TestProject
{
    public class ContactsApiCSVTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper helper;
        public ContactsApiCSVTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
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
        public async Task Post_SCV_File_Returns201()
        {
            var scvPath = """..\..\..\random_CSV_1000.txt""";
            var fstream = File.OpenRead(scvPath);
            using var content = new StreamContent(fstream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            var response = await _client.PostAsync("/api/employee", content);
            var messge = await response.Content.ReadAsStringAsync();
            helper.WriteLine(messge);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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