using Microsoft.AspNetCore.Mvc;

namespace VisionBuyBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyzeController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "Görsel yüklenemedi." });
            }

            // Burada görseli işleyebilir, benzer ürünleri bulabilirsin.
            // Şimdilik dummy veriler döndürelim:

            var dummyProducts = new[]
            {
                new {
                    name = "Mavi Elbise",
                    price = "499₺",
                    image = "https://via.placeholder.com/200x200.png?text=Elbise",
                    link = "https://www.ornek-magaza.com/urun/elbise"
                },
                new {
                    name = "Kırmızı Çanta",
                    price = "299₺",
                    image = "https://via.placeholder.com/200x200.png?text=Çanta",
                    link = "https://www.ornek-magaza.com/urun/canta"
                },
                new {
                    name = "Spor Ayakkabı",
                    price = "699₺",
                    image = "https://via.placeholder.com/200x200.png?text=Ayakkabı",
                    link = "https://www.ornek-magaza.com/urun/ayakkabi"
                }
            };

            return Ok(new { products = dummyProducts });
        }
    }
}
