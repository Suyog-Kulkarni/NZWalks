using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using System.Text.Json;
using System.Text;

namespace NZWalks.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public WalksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<WalksDTO> walks = new List<WalksDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();

                var responseMessage = await client.GetAsync("https://localhost:7002/api/walks");

                responseMessage.EnsureSuccessStatusCode();

                walks.AddRange(await responseMessage.Content.ReadFromJsonAsync<IEnumerable<WalksDTO>>());
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(walks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddWalkViewModel addWalk)
        {
            if (!ModelState.IsValid)
            {
                return View(addWalk);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7002/api/walks"),
                    Content = new StringContent(JsonSerializer.Serialize(addWalk), Encoding.UTF8, "application/json"),
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Walks");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong trying to add the walk: {ex.Message}");
                return View(addWalk);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync($"https://localhost:7002/api/walks/{id}");

                responseMessage.EnsureSuccessStatusCode();

                var walk = await responseMessage.Content.ReadFromJsonAsync<WalksDTO>();

                if (walk == null)
                {
                    return NotFound();
                }

                return View(walk);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Walks");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WalksDTO walkDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(walkDto), Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync($"https://localhost:7002/api/walks/{walkDto.Id}", content);

                responseMessage.EnsureSuccessStatusCode();

                TempData["SuccessMessage"] = "Walk updated successfully!";
                return RedirectToAction("Index", "Walks");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong trying to update the walk: {ex.Message}");
                return View(walkDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.DeleteAsync($"https://localhost:7002/api/walks/{id}");

                responseMessage.EnsureSuccessStatusCode();

                TempData["SuccessMessage"] = "Walk deleted successfully!";
                return RedirectToAction("Index", "Walks");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete walk: {ex.Message}";
                return RedirectToAction("Index", "Walks");
            }
        }
    }
}
