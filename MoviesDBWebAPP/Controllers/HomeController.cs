using Microsoft.AspNetCore.Mvc;
using MoviesDBWebAPP.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using MoviesDBWebAPP.Helpers;

namespace MoviesDBWebAPP.Controllers
{
    public class HomeController : Controller
    { 
        private readonly ILogger<HomeController> _logger;
        private readonly IMoviesDBAPIClient _moviesDBAPIClient;

        public HomeController(ILogger<HomeController> logger, IMoviesDBAPIClient moviesDBAPIClient )
        {
            _logger = logger;            
            _moviesDBAPIClient = moviesDBAPIClient;
        }

        public IActionResult Index()
        {
            try
            {               
                var popularMovies = _moviesDBAPIClient.GetPopularMovies().Result;

                MovieListViewModel model = new MovieListViewModel();
                model.Movies = popularMovies;

                ModelState.Clear();
                return View(model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View("Error");
            }
        }

        public IActionResult ShowQuickDetails(int id)
        {
            try
            {               
                var movieDetails = _moviesDBAPIClient.GetMovieDetails(id).Result;
                MovieDetailsViewModel model = new MovieDetailsViewModel();

                model.MovieDetails = movieDetails;
                

                ModelState.Clear();
                return View("_MovieDetails", model); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
        }

        public IActionResult ShowMovieDetails(int id)
        {
            try
            {
                var movieDetails = _moviesDBAPIClient.GetMovieDetails(id).Result;
                MovieDetailsViewModel model = new MovieDetailsViewModel();

                model.MovieDetails = movieDetails;


                ModelState.Clear();
                return View("MovieDetails", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
        }

        public IActionResult SaveMovie(MovieDetailsViewModel model)
        {
            try
            {
                var movieDetails = _moviesDBAPIClient.SaveMovieDetails(model.MovieDetails);

                ModelState.Clear();
                return View("MovieDetails", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}