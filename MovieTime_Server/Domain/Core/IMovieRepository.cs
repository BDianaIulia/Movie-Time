using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IMovieRepository: IRepository<Movie>
    {
        Task<ICollection<Movie>> GetOverview();
        Task<ICollection<Movie>> GetPaginated(int pageIndex, int itemsPerPage);
        Task<ICollection<Movie>> GetPaginatedForGenre(string genreName, int pageIndex, int itemsPerPage);
        Task<Movie> GetMovie(Guid id);
        Task<Movie> GetByName(string movieName);
        Task<Movie> GetByImdbId(string imdbId);
        Task<int> UpdatePosterPath(Movie movie);
        Task<List<Movie>> GetByFilter(string keywords);
    }
}
