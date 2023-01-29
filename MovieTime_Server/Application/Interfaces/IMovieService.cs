using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMovieService
    {
        public Task<int> RegisterMovie(MovieDto movie);
        public Task<IEnumerable<Movie>> GetOverview();
        public Task<IEnumerable<Movie>> GetPaginated(int pageIndex, int itemsPerPage);
        public Task<IEnumerable<Movie>> GetPaginatedForGenre(string genreName, int pageIndex, int itemsPerPage);
        public Task<Movie> GetItem(Guid movieId);
        public Task<Movie> GetItemByImdbId(string imdbId);
        public Task<Movie> GetItemByName(string movieName);
        public Task<int> UpdatePosterPath(Guid movieId, string posterPath);
        public Task<bool> LoadMoviesMetadataAsync();
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieDescriptionBasedRecommendations(string movieName);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieDetailsBasedRecommendations(string movieName);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieUserActivityBasedRecommendations(IEnumerable<Movie> wishListMovies, IEnumerable<Movie> goodRatingMovies);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieKeywordsBasedRecommendations(string keywords);
        public Task<List<Movie>> GetMoviesFilteredByKeywords(string keywords);
        public Task<List<Movie>> GetMoviesById(List<Guid> ids);
        public Task<List<Movie>> GetMoviesByImdbId(List<string> ids);
        public Task<List<Movie>> GetMoviesByName(List<string> titles);
    }
}
