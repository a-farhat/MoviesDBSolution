

namespace MoviesDBWebAPP.Models
{
    public class MovieDetails : Movie   
    {
        public int Id { get; set; }
        public List<Production_Companies> Production_Companies { get; set; }
        public List<Genres> Genres { get; set; }
        public List<Production_Countries>  Production_Countries { get; set; }

        public long Budget { get; set; }

        public long Revenue { get; set; }

        public int Runtime { get; set; }

        public string Tagline { get; set; }
    }
}
