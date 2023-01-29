using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface ILanguageRepository: IRepository<Language>
    {
        Task<Language> GetLanguageByCode(string code);
    }
}
