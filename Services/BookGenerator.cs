using BookGeneratorApp.Models;
using BookGeneratorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class BookGenerator
{
    private readonly LocalizationData _locData;
    private readonly Random _rnd;
    private readonly int _page;
    private readonly double _likesAvg;
    private readonly double _reviewsAvg;
    private readonly ReviewGenerator _reviewGenerator;
    private readonly int _seedHash;

    public BookGenerator(string seed, string region, int page, double likesAvg, double reviewsAvg, LocalizationService localization)
    {
        _seedHash = seed.GetHashCode();
        _locData = localization.Load(region);
        _rnd = new Random(_seedHash + page); // генератор зависит от seed
        _page = page;
        _likesAvg = likesAvg;
        _reviewsAvg = reviewsAvg;
        _reviewGenerator = new ReviewGenerator();
    }

    public List<Book> GenerateBooks(int count = 20)
    {
        var books = new List<Book>();

        for (int i = 0; i < count; i++)
        {
            int bookSeed = _seedHash + (_page - 1) * 100 + i;

            var title = GenerateStructuredTitle(bookSeed);

            var publisher = _locData.Publishers[_rnd.Next(_locData.Publishers.Count)];

            var authorCount = _rnd.Next(1, 3);
            var authors = Enumerable.Range(0, authorCount)
                .Select(_ =>
                {
                    var first = _locData.FirstNames[_rnd.Next(_locData.FirstNames.Count)];
                    var last = _locData.LastNames[_rnd.Next(_locData.LastNames.Count)];
                    return $"{first} {last}";
                }).ToList();

            int likes = GenerateLikes(bookSeed, _likesAvg);
            var reviews = _reviewGenerator.Generate(_reviewsAvg, bookSeed, _locData.LanguageCode.Substring(0, 2));

            var genre = _locData.Genres[_rnd.Next(_locData.Genres.Count)];
            var background = _locData.CoverBackgrounds[_rnd.Next(_locData.CoverBackgrounds.Count)];

            var book = new Book
            {
                Index = i + 1,
                ISBN = GenerateFakeIsbn(bookSeed),
                Title = title,
                Publisher = publisher,
                Authors = authors,
                Likes = likes,
                Reviews = reviews,
                Genre = genre,
                CoverBackground = background
            };

            books.Add(book);
        }

        return books;
    }
    private string GenerateStructuredTitle(int bookSeed)
    {
        var rng = new Random(bookSeed + 99); // seed
        var templates = _locData.TitleTemplates;
        var adjectives = _locData.TitleAdjectives;
        var nouns = _locData.TitleNouns;

        if (templates.Count == 0 || nouns.Count == 0)
            return "Başlıksız Kitap";

        var template = templates[rng.Next(templates.Count)];
        var noun = nouns[rng.Next(nouns.Count)];
        var adjective = adjectives.Count > 0 ? adjectives[rng.Next(adjectives.Count)] : null;

        string title = template
            .Replace("{Adjective}", adjective ?? "")
            .Replace("{Noun}", noun);

        return title.Trim();
    }


    private int GenerateLikes(int bookSeed, double avgLikes)
    {
        int baseLikes = (int)Math.Floor(avgLikes);
        double extraProbability = avgLikes - baseLikes;
        var likeRng = new Random(bookSeed + 1);
        return baseLikes + (likeRng.NextDouble() < extraProbability ? 1 : 0);
    }

    private string GenerateFakeIsbn(int seed)
    {
        var isbnRng = new Random(seed);
        return $"{_rnd.Next(100, 999)}-{_rnd.Next(1000, 9999)}-{_rnd.Next(100, 999)}";
    }
}
