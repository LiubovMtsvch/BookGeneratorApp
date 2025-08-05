namespace BookGeneratorApp.Models
{
    public class Review
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set;  }
        public string LanguageCode { get; set; }

    }
}
