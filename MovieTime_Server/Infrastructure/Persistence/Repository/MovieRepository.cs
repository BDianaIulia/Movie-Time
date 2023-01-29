using Domain.Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieTimeDbContext dbContext) : base(dbContext) { }
        public async Task<ICollection<Movie>> GetOverview()
        {
            var result =  await this._dbSet.Skip(0).Take(10).ToListAsync();
            return result;
            //.OrderByDescending(movie => movie.ImdbVoteAverage)
        }
        public async Task<ICollection<Movie>> GetPaginated(int pageIndex, int itemsPerPage)
        {
            return await this._dbSet.Skip(pageIndex * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }
        public async Task<ICollection<Movie>> GetPaginatedForGenre(string genreName, int pageIndex, int itemsPerPage)
        {
            return await this._dbSet.AsNoTracking().Include(movie => movie.Genres)
                .Where(b => b.Genres.Where(a => a.Name == genreName).ToList().Count > 0)
                .Skip(pageIndex * itemsPerPage).Take(itemsPerPage).ToListAsync();
        }
        public async Task<Movie> GetMovie(Guid id)
        {
            return await _dbSet
                        .AsNoTracking()
                        .Include(movie => movie.Genres)
                        .Include(movie => movie.SpokenLanguages)
                        .FirstOrDefaultAsync(movie => movie.Id.Equals(id));
        }
        public async Task<Movie> GetByName(string movieName)
        {
            return await _dbSet
                        .FirstOrDefaultAsync(movie => movie.Title == movieName);
        }
        public async Task<Movie> GetByImdbId(string imdbId)
        {
            return await _dbSet
                        .FirstOrDefaultAsync(movie => movie.ImdbId == imdbId);
        }
        public async Task<List<Movie>> GetByFilter(string keywords)
        {
            return await _dbSet
                        .Where(movie => movie.Title.Contains(keywords) || movie.Overview.Contains(keywords)).Take(20).ToListAsync();
        }
        public async Task<int> UpdatePosterPath(Movie movie)
        {
            this._dbSet.Attach(movie);
            return await this._dbContext.SaveChangesAsync();
        }
    }
}
