using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        public WalksController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            List<WalksDTO> walks = new List<WalksDTO>();
            try
            {
                var client = _httpClient.CreateClient();


                var makeRequest = await client.GetAsync("https://localhost:7002/api/walks");

                makeRequest.EnsureSuccessStatusCode();

                walks.AddRange(await makeRequest.Content.ReadFromJsonAsync<IEnumerable<WalksDTO>>());



            }
            catch (Exception)
            {
                throw;
            }
            return View(walks);
        }
    }
}
