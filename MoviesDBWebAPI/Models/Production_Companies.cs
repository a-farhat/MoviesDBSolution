namespace MoviesDBWebAPI.Models
{
    public class Production_Companies
    {
        public int Id { get; set; }
        public int MovieDetailsId { get; set; }
        public string  logo_path { get; set; }
        public string Name { get; set; }
        public string Origin_country { get; set; }
        public MovieDetails MovieDetails { get; set; }
    }
}
