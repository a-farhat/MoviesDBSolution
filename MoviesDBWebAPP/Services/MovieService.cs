using MoviesDBWebAPP.Models;

namespace MoviesDBWebAPP.Services
{
    public class MovieService : IMovieService
    {
        private List<Movie> _movies = new List<Movie>(); 

        public void AddMovie(Movie movie)
        {
            // Mimic database operation to add movie
            _movies.Add(movie);
        }
        // Implement other methods as needed
    }
}
