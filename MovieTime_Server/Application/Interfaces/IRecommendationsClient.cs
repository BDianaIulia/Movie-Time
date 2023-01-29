using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRecommendationsClient
    {
        public Task<IEnumerable<RecommendedMovieDto>> GetGenreRecommendations(string genreName);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieDescriptionBasedRecommendations(string movieName);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieDetailsBasedRecommendations(string movieName);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieKeywordsBasedRecommendations(string keywords);
        public Task<IEnumerable<RecommendedMovieDto>> GetMovieUserActivityBasedRecommendations(IEnumerable<string> moviesTitle);
    }
}
