using Domain.Entities;

namespace Domain.Core
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        ILanguageRepository Languages { get; }
        IUserMovieActivityRepository UserMovieActivities { get; }
        IDatabaseTransaction BeginTransaction();
    }
}
