using Domain.Dtos;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenreService
    {
        public Task<Genre> RegisterGenre(GenreDto genre);
        public Task<IEnumerable<string>> GetGenreNames();
        public Task<IEnumerable<RecommendedMovieDto>> GetGenreRecommendations(string genreName);
    }
}
