using Domain.Dtos;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILanguageService
    {
        public Task<Language> RegisterLanguage(LanguageDto language);

        public Task<Language> GetLanguageByCode(string name);
    }
}
