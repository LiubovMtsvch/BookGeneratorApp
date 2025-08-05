using BookGeneratorApp.Models;
using BookGeneratorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using BookGeneratorApp.Models;
public class BookGenerator
{
    private readonly LocalizationData _locData;
    private readonly Random _rnd;
    private readonly int _page;
    private readonly double _likesAvg;
    private readonly double _reviewsAvg;

    public BookGenerator(string seed, string region, int page, double likesAvg, double reviewsAvg, LocalizationService localization)
    {
        _locData = localization.Load(region);
        _rnd = new Random(seed.GetHashCode() + page); // Предсказуемый генератор
        _page = page;
        _likesAvg = likesAvg;
        _reviewsAvg = reviewsAvg;
    }

    public List<Book> GenerateBooks()
    {
        var books = new List<Book>();

        for (int i = 0; i < 20; i++)
        {
            int bookSeed = (_page - 1) * 10 + i + 1;

            var title = _locData.Titles[_rnd.Next(_locData.Titles.Count)];
            var publisher = _locData.Publishers[_rnd.Next(_locData.Publishers.Count)];

            var authorCount = _rnd.Next(1, 3);
            var authors = Enumerable.Range(0, authorCount)
                .Select(_ => $"{_locData.FirstNames[_rnd.Next(_locData.FirstNames.Count)]} {_locData.LastNames[_rnd.Next(_locData.LastNames.Count)]}")
                .ToList();

            int likes = GenerateLikes(bookSeed, _likesAvg);
            var reviews = GenerateReviews(bookSeed, _reviewsAvg);
            var genre = _locData.Genres[_rnd.Next(_locData.Genres.Count)];
            var book = new Book
            {
                Index = bookSeed,
                ISBN = GenerateFakeIsbn(),
                Title = title,
                Publisher = publisher,
                Authors = authors,
                Likes = likes,
                Reviews = reviews,
                Genre = genre,
                CoverBackground = _locData.CoverBackgrounds[_rnd.Next(_locData.CoverBackgrounds.Count)]
            };
          

            books.Add(book);
        }

        return books;
    }

    private int GenerateLikes(int bookSeed, double avgLikes)
    {
        int baseLikes = (int)Math.Floor(avgLikes);
        double extraProbability = avgLikes - baseLikes;
        var likeRng = new Random(bookSeed + 1);
        return baseLikes + (likeRng.NextDouble() < extraProbability ? 1 : 0);
    }

    private List<Review> GenerateReviews(int bookSeed, double avgReviews)
    {
        var rng = new Random(bookSeed + 2);
        int count = 0;

        if (avgReviews < 1.0)
        {
            if (rng.NextDouble() < avgReviews)
                count = 1;
        }
        else
        {
            count = (int)Math.Round(avgReviews);
        }

        var reviews = new List<Review>();

        for (int i = 0; i < count; i++)
        {
            var review = new Review
            {
                Author = $"{_locData.FirstNames[rng.Next(_locData.FirstNames.Count)]} {_locData.LastNames[rng.Next(_locData.LastNames.Count)]}",
                Text = _locData.ReviewComments[rng.Next(_locData.ReviewComments.Count)],
                Rating = rng.Next(1, 6),
                Date = DateTime.Today.AddDays(-rng.Next(30, 700)), // случайная дата за последние ~2 года
                LanguageCode = _locData.LanguageCode // если ты добавила это свойство
            };
            reviews.Add(review);

        }

        return reviews;
    }

    private string GenerateFakeIsbn()
    {
        return $"{_rnd.Next(100, 999)}-{_rnd.Next(1000, 9999)}-{_rnd.Next(100, 999)}";
    }
}
