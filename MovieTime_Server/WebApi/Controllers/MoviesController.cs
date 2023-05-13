using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Threading.Tasks;
using WebApi.Dtos;
using System.Collections.Generic;
using Domain.Entities;
using System;
using WebApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Domain.Dtos;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;
        private readonly IUserMovieActivityService _activityService;
        private readonly TokenAuthenticationHandler _tokenAuthenticationHandler;
        public MoviesController(IMovieService movieService, IUserMovieActivityService activityService, 
            TokenAuthenticationHandler tokenAuthenticationHandler) 
        {
            _movieService = movieService;
            _activityService = activityService;
            _tokenAuthenticationHandler = tokenAuthenticationHandler;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }
        [HttpGet]
        [Route("overview")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Movie>>>> GetOverview()
        {
            var items = await _movieService.GetOverview();
            return HandleResponse(items);
        }
        [HttpGet]
        [Route("paginated")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Movie>>>> GetPaginated([FromQuery] int pageIndex, [FromQuery] int itemsNumber)
        {
            var items = await _movieService.GetPaginated(pageIndex, itemsNumber);
            return HandleResponse(items);
        }
        [HttpGet]
        [Route("paginated/{genreName}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<Movie>>>> GetPaginatedForGenre([FromQuery] int pageIndex, 
            [FromQuery] int itemsNumber,
            [FromRoute] string genreName)
        {
            var items = await _movieService.GetPaginatedForGenre(genreName, pageIndex, itemsNumber);
            return HandleResponse(items);
        }
        [HttpGet]
        [Route("{movieId}")]
        public async Task<ActionResult<ResponseDto<Movie>>> GetItem([FromRoute] Guid movieId)
        {
            var item = await _movieService.GetItem(movieId);
            return HandleResponse(item);
        }
        [HttpGet]
        [Route("movie-description-based-recommender/{movieName}")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetMovieDescriptionBasedRecommendations([FromRoute] string movieName)
        {
            try
            {
                var items = await _movieService.GetMovieDescriptionBasedRecommendations(movieName);
                var titles = items.Select(item => item.title).ToList();
                List<Movie> movies = await _movieService.GetMoviesByName(titles);
                return HandleResponse(movies);
            }
            catch
            {
                return base.HandleResponse(new List<Movie>());
            }
        }
        [HttpGet]
        [Route("movie-details-based-recommender/{movieName}")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetMovieDetailsBasedRecommendations([FromRoute] string movieName)
        {
            try
            {
                var items = await _movieService.GetMovieDetailsBasedRecommendations(movieName);
                var ids = items.Select(item => item.imdb_id).ToList();
                List<Movie> movies = await _movieService.GetMoviesByImdbId(ids);
                return HandleResponse(movies);
            }
            catch
            {
                return base.HandleResponse(new List<Movie>());
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("movie-user-activity-based-recommender")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetMovieUserActivityBasedRecommendations()
        {
            try
            {
                var user = await _tokenAuthenticationHandler.GetCurrentUserAsync(HttpContext);
                if (user == null) return HandleResponse(new List<Movie>(), DefaultErrors.GetErrors(DefaultErrors.BadRequestError));

                IEnumerable<UserMovieActivity> wishList = await _activityService.GetUserMoviesWishList(user, 10);
                var ids = wishList.Select(item => item.Movie.Id).ToList();
                List<Movie> wishListMovies = await _movieService.GetMoviesById(ids);

                IEnumerable<UserMovieActivity> goodRating = await _activityService.Get5StarsRatedMovies(user, 10);
                ids = goodRating.Select(item => item.Movie.Id).ToList();
                List<Movie> goodRatingMovies = await _movieService.GetMoviesById(ids);

                var items = await _movieService.GetMovieUserActivityBasedRecommendations(wishListMovies, goodRatingMovies);
                var titles = items.Select(item => item.title).ToList();
                List<Movie> movies = await _movieService.GetMoviesByName(titles);
                return HandleResponse(movies);
            }
            catch
            {
                return base.HandleResponse(new List<Movie>());
            }
        }
        [HttpGet]
        [Route("movie-keywords-based-recommender/{keywords}")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetMovieKeywordsBasedRecommendations([FromRoute] string keywords)
        {
            try 
            {
                var items = await _movieService.GetMovieKeywordsBasedRecommendations(keywords);
                var ids = items.Select(item => item.imdb_id).ToList();
                List<Movie> movies = await _movieService.GetMoviesByImdbId(ids);
                return HandleResponse(movies);
            }
            catch
            {
                return base.HandleResponse(new List<Movie>());
            }
        }
        [HttpGet]
        [Route("simple-search/{keywords}")]
        public async Task<ActionResult<ResponseDto<List<Movie>>>> GetMoviesByKeywords([FromRoute] string keywords)
        {
            List<Movie> movies = await _movieService.GetMoviesFilteredByKeywords(keywords);
            return HandleResponse(movies);
        }
        [HttpPut]
        [Route("{itemId}")]
        public async Task<ActionResult<ResponseDto>> UpdateItemAsync([FromRoute] Guid itemId, [FromBody] string posterPath)
        {
            var response = await _movieService.UpdatePosterPath(itemId, posterPath);
            if (response == 1)
            {
                return HandleResponse();
            }
            else
            {
                return HandleResponse(DefaultErrors.GetErrors(DefaultErrors.BadRequestError));
            }
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("userActivity/{movieId}")]
        public async Task<ActionResult<ResponseDto>> UpdateUserActivity([FromRoute] Guid movieId, [FromBody] UserMovieActivityDto activityDto)
        {
            var user = await _tokenAuthenticationHandler.GetCurrentUserAsync(HttpContext);
            if (user == null) return HandleResponse(DefaultErrors.GetErrors(DefaultErrors.BadRequestError));

            var response = await _activityService.UpdateActivity(activityDto, movieId, user);
            if (response != null)
            {
                return HandleResponse();
            }
            else
            {
                return HandleResponse(DefaultErrors.GetErrors(DefaultErrors.BadRequestError));
            }
        }
    }
}
