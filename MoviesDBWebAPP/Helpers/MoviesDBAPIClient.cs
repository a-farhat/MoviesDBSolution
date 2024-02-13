using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MoviesDBWebAPP.Models;
using System.Text;

namespace MoviesDBWebAPP.Helpers
{
    public interface IMoviesDBAPIClient
    {
        Task<List<Movie>> GetPopularMovies();
        Task<MovieDetails> GetMovieDetails(int id);
        Task<Credits> GetMovieCredits(int id);
        Task SaveMovieDetails(MovieDetails movieDetails);
    }

    public class MoviesDBAPIClient : IMoviesDBAPIClient
    {
        private readonly HttpClient _httpClient;

        public MoviesDBAPIClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7046");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Movie>> GetPopularMovies()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/MovieDB/GetPopularMovies");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
            if (movies == null)
            {
                return new List<Movie>();
            }

            return movies;
        }

        public async Task<MovieDetails> GetMovieDetails(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/MovieDB/GetMovieDetails?id={id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(responseBody);
            if (movieDetails == null)
            {
                return new MovieDetails();
            }

            return movieDetails;
        }

        public async Task<Credits> GetMovieCredits(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/MovieDB/GetMovieCredits?id={id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var movieCredits = JsonConvert.DeserializeObject<Credits>(responseBody);
            if (movieCredits == null)
            {
                return new Credits();
            }

            return movieCredits;
        }

        public async Task SaveMovieDetails(MovieDetails movieDetails)
        {
            var serializedMovieDetails = JsonConvert.SerializeObject(movieDetails);
            var content = new StringContent(serializedMovieDetails, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/SaveMovieDetails", content);
            response.EnsureSuccessStatusCode();
        }
    }
}


