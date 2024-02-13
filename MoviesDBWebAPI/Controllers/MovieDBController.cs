using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MoviesDBWebAPI.DBContext;
using MoviesDBWebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MoviesDBWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDBController : ControllerBase
    {
        private readonly MoviesDBContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public MovieDBController(MoviesDBContext dbContext, HttpClient httpClient, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI5MGYxYTk3NDE3NzIxZDY2ZDQwNWY1NTRlMDkyYTRiYSIsInN1YiI6IjU0ZjQ5OGE2OTI1MTQxNzk5ZjAwMjFmMiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.AmH9ahcSsro2udQ5FbFbLBM6d62_nlZH8oyKlCJ8x8w");

            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet("GetPopularMovies")]
        public async Task<IActionResult> GetPopularMovies()
        {
            List<Movie> movies = new List<Movie>();
            var response = await _httpClient.GetAsync("popular");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(data);
                var results = responseData.results;
                movies = JsonConvert.DeserializeObject<List<Movie>>(results.ToString());
            }
            return Ok(movies);
        }

        [HttpGet("GetMovieDetails")]
        public async Task<IActionResult> GetMovieDetails(int id)
        {
            MovieDetails movieDetails = new MovieDetails();
            string cacheKey = $"MovieDetails_{id}";
            if (_memoryCache.TryGetValue(cacheKey, out MovieDetails cachedDetails))
            {
                return Ok(cachedDetails); 
            }
            var response = await _httpClient.GetAsync(id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(data);
                movieDetails = JsonConvert.DeserializeObject<MovieDetails>(responseData.ToString());
            }

            // Cache for 10 minutes
            _memoryCache.Set(cacheKey, movieDetails, TimeSpan.FromMinutes(10)); 

            return Ok(movieDetails);
        }

        [HttpGet("GetMovieCredits")]
        public async Task<IActionResult> GetMovieCredits(int id)
        {
            Credits movieCredits = new Credits();
            string cacheKey = $"MovieCredits_{id}";
            if (_memoryCache.TryGetValue(cacheKey, out MovieDetails cachedDetails))
            {
                return Ok(cachedDetails);
            }
            var response = await _httpClient.GetAsync(id.ToString()+"/credits");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(data);
                movieCredits = JsonConvert.DeserializeObject<Credits>(responseData.ToString());
            }

            // Cache for 10 minutes
            _memoryCache.Set(cacheKey, movieCredits, TimeSpan.FromMinutes(10));
            return Ok(movieCredits);
        }

        [HttpPost("/SaveMovieDetails")]
        public async Task<IActionResult> SaveMovieDetails(MovieDetails movieDetails)
        {
            _dbContext.MovieDetails.Add(movieDetails);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/GetMoviesFromDB")]
        public IActionResult GetMoviesFromDB()
        {
            var movies = _dbContext.Movies.ToList();
            if (movies.Count == 0)
            {
                return NotFound("Products Not Available");
            }
            return Ok(movies);
        }
    }
}