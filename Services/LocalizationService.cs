using System.Text.Json;
using BookGeneratorApp.Models;
using Microsoft.AspNetCore.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookGeneratorApp.Services
{
    public class LocalizationService
    {
        private readonly IWebHostEnvironment _env;

        public LocalizationService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public LocalizationData Load(string region)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Localization", $"{region}.json");
            var json = File.ReadAllText(filePath);

            var data = JsonSerializer.Deserialize<LocalizationData>(json);

            if (data == null)
                throw new Exception($"Не удалось загрузить файл локализации: {region}.json");

            // 🎯 Назначаем язык после загрузки
            data.LanguageCode = region;

            return data;
        }


    }
}
