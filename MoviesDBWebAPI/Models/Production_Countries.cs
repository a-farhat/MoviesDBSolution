namespace MoviesDBWebAPI.Models
{
    public class Production_Countries
    {
        public int Id { get; set; }
        public int MovieDetailsId { get; set; }
        public string Iso_3166_1 { get; set; }
        public string  Name { get; set; }
        public MovieDetails MovieDetails { get; set; }
    }
}
