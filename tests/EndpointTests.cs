using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace tests
{
    public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EndpointTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllV1_Returns_All_25()
        {
            var res = await _client.GetAsync("/api/v1/Products");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);

            var data = await res.Content.ReadFromJsonAsync<List<JsonElement>>();
            Assert.NotNull(data);
            Assert.Equal(25, data!.Count);
        }

        [Fact]
        public async Task GetAllV2_Returns_Second_Page()
        {
            var res = await _client.GetAsync("/api/v2/Products?page=2&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);

            var doc = await res.Content.ReadFromJsonAsync<JsonDocument>();
            Assert.NotNull(doc);

            var root = doc.RootElement;

            var data = root.GetProperty("data");
            Assert.Equal(10, data.GetArrayLength());

            var meta = root.GetProperty("meta");
            Assert.Equal(2, meta.GetProperty("page").GetInt32());
            Assert.Equal(10, meta.GetProperty("pageSize").GetInt32());
            Assert.Equal(3, meta.GetProperty("totalPages").GetInt32());
        }

        [Fact]
        public async Task GetById_Found_And_NotFound()
        {
            var ok = await _client.GetAsync("/api/v1/Products/1");
            Assert.Equal(HttpStatusCode.OK, ok.StatusCode);

            var nf = await _client.GetAsync("/api/v1/Products/9999");
            Assert.Equal(HttpStatusCode.NotFound, nf.StatusCode);
        }

        [Fact]
        public async Task UpdateDescription_Successfull()
        {
            const int productId = 1;
            const string newDescription = "NEW";
            var requestUri = $"/api/v1/Products/{productId}/description";

            var response = await _client.PatchAsJsonAsync(requestUri, new { description = newDescription });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
