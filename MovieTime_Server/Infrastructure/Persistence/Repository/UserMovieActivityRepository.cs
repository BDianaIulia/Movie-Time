using Domain.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class UserMovieActivityRepository : Repository<UserMovieActivity>, IUserMovieActivityRepository
    {
        public UserMovieActivityRepository(MovieTimeDbContext dbContext) : base(dbContext) { }
        public async Task<UserMovieActivity> GetActivity(Movie movie, IdentityUser user)
        {
            return await _dbSet
                        .FirstOrDefaultAsync(activity => activity.Movie.Id == movie.Id &&
                                                         activity.User.Id == user.Id);
        }
        public async Task<IEnumerable<UserMovieActivity>> GetWishList(IdentityUser user, int take)
        {
            return await _dbSet
                        .Where(activity => activity.User.Id == user.Id && activity.Wishlist == true)
                        .Include(activity => activity.Movie)
                        .Take(take).ToListAsync();
        }
        public async Task<IEnumerable<UserMovieActivity>> Get5StarsRatedMovies(IdentityUser user, int take)
        {
            return await _dbSet
                        .Where(activity => activity.User.Id == user.Id && activity.Rating == 5)
                        .Include(activity => activity.Movie)
                        .Take(take).ToListAsync();
        }
    }
}
