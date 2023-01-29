using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IUserMovieActivityRepository: IRepository<UserMovieActivity>
    {
        Task<UserMovieActivity> GetActivity(Movie movie, IdentityUser user);
        Task<IEnumerable<UserMovieActivity>> GetWishList(IdentityUser user, int take);
        Task<IEnumerable<UserMovieActivity>> Get5StarsRatedMovies(IdentityUser user, int take);
    }
}
