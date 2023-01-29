using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    public class GenresController : BaseController
    {
        private IGenreService _genreService;
        private IMovieService _movieService;
        public GenresController(IGenreService genreService, IMovieService movieService)
        {
            _genreService = genreService;
            _movieService = movieService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ResponseDto<IEnumerable<string>>>> GetGenresList()
        {
            var items = await _genreService.GetGenreNames();
            return HandleResponse(items);
        }

        [HttpGet]
        [Route("genre-recommendations/{genreName}")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetGenreRecommendations([FromRoute] string genreName)
        {
            var items = await _genreService.GetGenreRecommendations(genreName);
            List<Movie> movies = new List<Movie>();
            foreach(var item in items)
            {
                var movie = await _movieService.GetItemByImdbId(item.imdb_id);
                if (movie == null)
                    continue;
                movies.Add(movie);
            }
            return HandleResponse(movies);
        }
    }
}
