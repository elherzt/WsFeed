namespace WSFeed.Common
{
    public class NewsResponse
    {
        public List<NewsItem> Results { get; set; }
    }

    public class NewsItem
    {
        public string Title { get; set; }
        public string Date { get; set; }

        //public List<string> Type { get; set; } // json inconsistente con este campo a veces es un string y a veces es un array "type": ["periodical"] o "type": "zoom" en la misma llamada
        public List<string> PartOf { get; set; }
        public List<string> Language { get; set; }
    }
}
