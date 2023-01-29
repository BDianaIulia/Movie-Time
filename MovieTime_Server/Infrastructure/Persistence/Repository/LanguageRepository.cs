using Domain.Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(MovieTimeDbContext dbContext) : base(dbContext) { }

        public async Task<Language> GetLanguageByCode(string code)
        {
            return await this._dbSet.FirstOrDefaultAsync(language => language.Code.ToLower().Equals(code.ToLower()));
        }
    }
}
