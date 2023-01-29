using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserMovieActivityService
    {
        public Task<IEnumerable<UserMovieActivity>> GetUserMoviesWishList(IdentityUser user, int take);
        public Task<IEnumerable<UserMovieActivity>> Get5StarsRatedMovies(IdentityUser user, int take);
        public Task<UserMovieActivity> UpdateActivity(UserMovieActivityDto userMovieActivityDto, Guid movieId, IdentityUser user);
    }
}
