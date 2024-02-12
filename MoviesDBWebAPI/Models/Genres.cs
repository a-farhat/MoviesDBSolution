namespace MoviesDBWebAPI.Models
{
    public class Genres
    {
        public int Id{ get; set; }
        public int MovieDetailsId { get; set; }
        public string Name { get; set; }
    }
}
