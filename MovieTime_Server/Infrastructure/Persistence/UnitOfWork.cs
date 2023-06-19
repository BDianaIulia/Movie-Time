using Domain.Core;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly MovieTimeDbContext _dbContext;
        public UnitOfWork(MovieTimeDbContext dbContext)
        {
            this._dbContext = dbContext;

            this.Movies = new MovieRepository(_dbContext);
            this.Genres = new GenreRepository(_dbContext);
            this.Languages = new LanguageRepository(_dbContext);
            this.UserMovieActivities = new UserMovieActivityRepository(_dbContext);
        }
        public IMovieRepository Movies { get; }
        public IGenreRepository Genres { get; }
        public ILanguageRepository Languages { get; }
        public IUserMovieActivityRepository UserMovieActivities { get; }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(_dbContext);
        }
    }
}
