using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace VisionBuyBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyzeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnalyzeController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { message = "Görsel yüklenemedi." });

            // Görseli base64'e çevir
            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
            string base64Image = Convert.ToBase64String(imageBytes);

            // Gemini API'ye istek at
            string apiKey = _configuration["Gemini:ApiKey"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new object[]
                        {
                            new {
                                inline_data = new {
                                    mime_type = image.ContentType,
                                    data = base64Image
                                }
                            },
                            new {
                                text = "Bu görselde hangi ürünler var ve benzerlerini önerebilir misin?"
                            }
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-pro-vision:generateContent?key=" + apiKey, content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new { message = "Gemini API hatası oluştu." });
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var resultJson = JsonDocument.Parse(responseString);
            var text = resultJson.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return Ok(new { result = text });
        }
    }
}
