using Application.Interfaces;
using Domain.Core;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserMovieActivityService : IUserMovieActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        public UserMovieActivityService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<UserMovieActivity>> GetUserMoviesWishList(IdentityUser user, int take)
        {
            return await _unitOfWork.UserMovieActivities.GetWishList(user, take);
        }
        public async Task<IEnumerable<UserMovieActivity>> Get5StarsRatedMovies(IdentityUser user, int take)
        {
            return await _unitOfWork.UserMovieActivities.Get5StarsRatedMovies(user, take);
        }
        public async Task<UserMovieActivity> UpdateActivity(UserMovieActivityDto userMovieActivityDto, Guid movieId, 
            IdentityUser user)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Movie? existingMovie = await _unitOfWork.Movies.GetById(movieId);
                    if (existingMovie == null)
                        return await Task.FromResult<UserMovieActivity>(null);

                    UserMovieActivity userMovieActivity = await _unitOfWork.UserMovieActivities.GetActivity(existingMovie, user);
                    if (userMovieActivity != null)
                    {
                        var userActivityToRegister = new UserMovieActivity
                        {
                            Id = userMovieActivity.Id,
                            Movie = existingMovie,
                            User = user,
                            Rating = userMovieActivityDto.Rating != 0 ? userMovieActivityDto.Rating : userMovieActivity.Rating,
                            Wishlist = userMovieActivityDto.Wishlist ? userMovieActivityDto.Wishlist : userMovieActivity.Wishlist,
                        };
                        await _unitOfWork.UserMovieActivities.Update(userActivityToRegister);
                        return await Task.FromResult(userActivityToRegister);
                    }

                    var activityId = Guid.NewGuid();
                    var isTransactionSucceeded = true;
                    var activityToRegister = new UserMovieActivity
                    {
                        Id = activityId,
                        Movie = existingMovie,
                        User = user,
                        Rating = userMovieActivityDto.Rating,
                        Wishlist = userMovieActivityDto.Wishlist
                    };
                    var isActivityRegistered = await _unitOfWork.UserMovieActivities.Register(activityToRegister);
                    if (isActivityRegistered == 0)
                    {
                        isTransactionSucceeded = false;
                    }
                    if (isTransactionSucceeded)
                    {
                        transaction.Commit();
                        return await Task.FromResult(activityToRegister);
                    }
                    return await Task.FromResult<UserMovieActivity>(null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return await Task.FromResult<UserMovieActivity>(null);
                }
            }
        }
    }
}
