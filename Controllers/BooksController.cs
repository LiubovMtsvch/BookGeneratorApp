using Microsoft.AspNetCore.Mvc;
using BookGeneratorApp.Services;
using BookGeneratorApp.Models;

namespace BookGeneratorApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LocalizationService _localization;

        public BooksController(LocalizationService localization)
        {
            _localization = localization;
        }

        // 🔧 Генерация списка книг
        [HttpGet]
        public IActionResult GetBooks(
            string seed = "default",
            string region = "es-ES",
            int page = 1,
            double likesAvg = 5.0,
            double reviewsAvg = 1.0)
        {
            var generator = new BookGenerator(seed, region, page, likesAvg, reviewsAvg, _localization);
            var books = generator.GenerateBooks();
            return Ok(books);
        }

        // 🧪 Тестовая книга для проверки сериализации
        [HttpGet("test")]
        public IActionResult TestGenre()
        {
            var generator = new BookGenerator("testSeed", "es-ES", 1, 5.0, 1.0, _localization);
            var book = generator.GenerateBooks().First();
            return Ok(book);
        }
    }
}
