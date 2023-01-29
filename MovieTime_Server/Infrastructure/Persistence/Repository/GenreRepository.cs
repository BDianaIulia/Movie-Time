using Domain.Core;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieTimeDbContext dbContext) : base(dbContext) { }

        public async Task<Genre?> GetGenreByName(string genreName)
        {
            return await this._dbSet.FirstOrDefaultAsync(genre => genre.Name.ToLower().Equals(genreName.ToLower()));
        }
    }
}
