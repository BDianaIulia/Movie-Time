using Application.Interfaces;
using Domain.Core;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GenreService: IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecommendationsClient _recommendationsClient;
        public GenreService(IUnitOfWork unitOfWork, IRecommendationsClient recommendationsClient)
        {
            _recommendationsClient = recommendationsClient;
            _unitOfWork = unitOfWork;
        }
        public async Task<Genre> RegisterGenre(GenreDto genre)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Genre? existingGenre = await _unitOfWork.Genres.GetGenreByName(genre.Name);
                    if (existingGenre != null)
                        return await Task.FromResult(existingGenre);

                    var genreId = Guid.NewGuid();
                    var isTransactionSucceeded = true;
                    var genreToRegister = new Genre
                    {
                        Id = genreId,
                        Name = genre.Name
                    };
                    var isGenreRegistered = await _unitOfWork.Genres.Register(genreToRegister);
                    if (isGenreRegistered == 0)
                    {
                        isTransactionSucceeded = false;
                    }
                    if (isTransactionSucceeded)
                    {
                        transaction.Commit();
                        return await Task.FromResult(genreToRegister);
                    }
                    return await Task.FromResult<Genre>(null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return await Task.FromResult<Genre>(null);
                }
            }
        }
        public async Task<IEnumerable<string>> GetGenreNames()
        {
            var items = await _unitOfWork.Genres.GetAll();
            List<string> genresNames = new List<string>();
            foreach (var item in items)
            {
                genresNames.Add(item.Name);
            }
            return genresNames;
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetGenreRecommendations(string genreName)
        {
            return await _recommendationsClient.GetGenreRecommendations(genreName);
        }
    }
}
