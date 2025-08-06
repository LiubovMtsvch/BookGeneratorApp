namespace BookGeneratorApp.Models
{
    public class Book
    {

        
        public string Genre { get; set; }

        public int Index { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public int Likes { get; set; }
        public List<Review> Reviews { get; set; }
        public string CoverBackground { get; set; }  
    }
}
