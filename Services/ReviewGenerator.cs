using BookGeneratorApp.Models;
using Bogus;
using System;
using System.Collections.Generic;

namespace BookGeneratorApp.Services
{
    public class ReviewGenerator
    {
        // Нормализация локали до двухбуквенного кода
        private string NormalizeLocale(string regionCode)
        {
            return regionCode?.Substring(0, 2).ToLower() ?? "en";
        }

        // Предустановленные фразы отзывов по языкам
        private static readonly Dictionary<string, string[]> ReviewPhrases = new()
        {
            ["en"] = new[] {
                "A captivating story.",
                "Didn't meet my expectations.",
                "Highly recommended!",
                "A light and entertaining read.",
                "I couldn't put it down.",
                "The characters were well developed.",
                "The plot twists surprised me.",
                "An emotional journey worth experiencing."
            },
            ["es"] = new[] {
                "Una historia fascinante.",
                "No cumplió mis expectativas.",
                "¡Recomendado totalmente!",
                "Lectura ligera y entretenida.",
                "No pude dejar de leerlo.",
                "Los personajes estaban bien desarrollados.",
                "Los giros de la trama me sorprendieron.",
                "Una travesía emocional que vale la pena."
            },
            ["tr"] = new[] {
                "Büyüleyici bir hikaye.",
                "Beklentilerimi karşılamadı.",
                "Kesinlikle tavsiye ederim!",
                "Hafif ve eğlenceli bir okuma.",
                "Elimden bırakamadım.",
                "Karakterler çok iyi yazılmıştı.",
                "Hikâyedeki ters köşeler şaşırttı.",
                "Duygusal bir yolculuk, denemeye değer."
            }
        };

        // Метод генерации отзывов
        public List<Review> Generate(double avgReviews, int bookSeed, string locale)
        {
            var localeKey = NormalizeLocale(locale);

            // Получение пула фраз для нужного языка
            string[] pool = ReviewPhrases.ContainsKey(localeKey)
                ? ReviewPhrases[localeKey]
                : ReviewPhrases["en"];

            // Вычисление количества отзывов через индивидуальный сид
            var countRng = new Random(bookSeed + 3);
            int count;
            if (avgReviews < 1.0)
            {
                count = countRng.NextDouble() < avgReviews ? 1 : 0;
            }
            else
            {
                int baseCount = (int)Math.Floor(avgReviews);
                double extraChance = avgReviews - baseCount;
                count = baseCount + (countRng.NextDouble() < extraChance ? 1 : 0);
            }

            var reviews = new List<Review>();
            for (int i = 0; i < count; i++)
            {
                int reviewSeed = bookSeed * 1000 + i; // уникальный сид для каждого отзыва

                var faker = new Faker(localeKey)
                {
                    Random = new Randomizer(reviewSeed)
                };

                var review = new Review
                {
                    Author = faker.Name.FullName(),
                    Rating = faker.Random.Int(1, 5),
                    Date = faker.Date.Past(2),
                    Text = faker.Random.ArrayElement(pool),
                    LanguageCode = locale
                };

                reviews.Add(review);
            }

            return reviews;
        }
    }
}
