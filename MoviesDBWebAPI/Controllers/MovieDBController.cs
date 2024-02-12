using Microsoft.AspNetCore.Mvc;
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

        public MovieDBController(MoviesDBContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI5MGYxYTk3NDE3NzIxZDY2ZDQwNWY1NTRlMDkyYTRiYSIsInN1YiI6IjU0ZjQ5OGE2OTI1MTQxNzk5ZjAwMjFmMiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.AmH9ahcSsro2udQ5FbFbLBM6d62_nlZH8oyKlCJ8x8w");
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
            var response = await _httpClient.GetAsync(id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(data);
                movieDetails = JsonConvert.DeserializeObject<MovieDetails>(responseData.ToString());
            }
            return Ok(movieDetails);
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