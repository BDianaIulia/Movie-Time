using Domain.Entities;

namespace Domain.Core
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        IItemTypeRepository ItemTypes { get; }
        ITeamMemberRoleRepository TeamMemberRoles { get; }
        IRepository<Sprint> Sprints { get; }
        IStateRepository States { get; }
        IRepository<Team> Teams { get; }
        ITeamMemberRepository TeamMembers { get; }
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        ILanguageRepository Languages { get; }
        IUserMovieActivityRepository UserMovieActivities { get; }
        IDatabaseTransaction BeginTransaction();
    }
}
