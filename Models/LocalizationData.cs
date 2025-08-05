namespace BookGeneratorApp.Models
{
    public class LocalizationData
    {
        public List<string> Genres { get; set; }
        public List<string> Titles { get; set; }
        public List<string> FirstNames { get; set; }
        public List<string> LastNames { get; set; }
        public List<string> Publishers { get; set; }
        public List<string> ReviewComments { get; set; }
        public List<string> CoverBackgrounds { get; set; }
        public string LanguageCode { get; set; }

    }
}