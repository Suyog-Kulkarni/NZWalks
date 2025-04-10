using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RegionsController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionsDTO> regions = new List<RegionsDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();

                var responseMessage = await client.GetAsync("https://localhost:7002/api/regions");

                responseMessage.EnsureSuccessStatusCode();

                regions.AddRange(await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDTO>>());

            }
            catch (Exception)
            {

                throw;
            }
            return View(regions);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel addRegion)
        {
            var client = _httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7002/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json"),

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionsDTO>();

            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
            // this method send a post request to /api/region using a json body and then api returns the created object 
            // then it is decelerize into an object of regionDTO

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            var reponseMessage = await client.GetAsync($"https://localhost:7002/api/regions/{id.ToString()}");

            reponseMessage.EnsureSuccessStatusCode();

            var region = await reponseMessage.Content.ReadFromJsonAsync<RegionsDTO>();
            return View(region);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionsDTO regions)
        {
            var client = _httpClientFactory.CreateClient();

            /* var httpRequestMessage = new HttpRequestMessage()
             {
                 Method = HttpMethod.Put,
                 RequestUri = new Uri($"https://localhost:7002/api/regions/{regions.Id}"),
                 Content = new StringContent(JsonSerializer.Serialize(regions), Encoding.UTF8, "application/json")
             };*/

            var Content = new StringContent(JsonSerializer.Serialize(regions), Encoding.UTF8, "application/json");
            var reponseMessage = await client.PutAsync($"https://localhost:7002/api/regions/{regions.Id}", Content);

            reponseMessage.EnsureSuccessStatusCode();
            var response = await reponseMessage.Content.ReadFromJsonAsync<RegionsDTO>();
            if (response != null)
            {
                RedirectToAction("Index", "Regions");
            }

            return Redirect("https://localhost:7016/Regions");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.DeleteAsync($"https://localhost:7002/api/regions/{id}");

            responseMessage.EnsureSuccessStatusCode();

            var httpResponseMessage = await responseMessage.Content.ReadFromJsonAsync<RegionsDTO>();

            if (httpResponseMessage != null)
            {
                RedirectToAction("Index", "Regions");
            }
            return Redirect("https://localhost:7016/Regions");
        }

    }

}
